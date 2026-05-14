USE HospitalAppointmentDB;
GO

CREATE VIEW vw_AppointmentDetails
AS
SELECT 
    a.AppointmentId,
    up.FullName AS PatientName,
    ud.FullName AS DoctorName,
    s.SpecialtyName,
    ms.ServiceName AS MedicalServiceName,
    cr.RoomName AS ClinicRoomName,
    ds.WorkDate,
    ds.StartTime,
    ds.EndTime,
    a.Reason,
    a.Status,
    a.CreatedAt
FROM Appointments a
JOIN Patients p ON a.PatientId = p.PatientId
JOIN Users up ON p.UserId = up.UserId
JOIN Doctors d ON a.DoctorId = d.DoctorId
JOIN Users ud ON d.UserId = ud.UserId
JOIN Specialties s ON d.SpecialtyId = s.SpecialtyId
JOIN DoctorSchedules ds ON a.ScheduleId = ds.ScheduleId
JOIN MedicalServices ms ON a.MedicalServiceId = ms.MedicalServiceId
JOIN ClinicRooms cr ON a.ClinicRoomId = cr.ClinicRoomId;
GO

CREATE FUNCTION fn_GetAvailableSlot(@ScheduleId INT)
RETURNS INT
AS
BEGIN
    DECLARE @AvailableSlot INT;
    SELECT @AvailableSlot = MaxPatients - CurrentPatients
    FROM DoctorSchedules
    WHERE ScheduleId = @ScheduleId;
    RETURN ISNULL(@AvailableSlot, 0);
END;
GO

CREATE PROCEDURE sp_BookAppointment
    @PatientId INT,
    @DoctorId INT,
    @ScheduleId INT,
    @MedicalServiceId INT,
    @ClinicRoomId INT,
    @Reason NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1 FROM DoctorSchedules
            WHERE ScheduleId = @ScheduleId
              AND DoctorId = @DoctorId
              AND Status = N'Available'
        )
        BEGIN
            RAISERROR(N'Lịch khám không tồn tại hoặc không khả dụng.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        IF dbo.fn_GetAvailableSlot(@ScheduleId) <= 0
        BEGIN
            RAISERROR(N'Lịch khám đã đầy.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;


        IF NOT EXISTS (SELECT 1 FROM MedicalServices WHERE MedicalServiceId = @MedicalServiceId)
        BEGIN
            RAISERROR(N'Dịch vụ khám không tồn tại.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        IF NOT EXISTS (
            SELECT 1 FROM ClinicRooms cr
            JOIN Doctors d ON cr.SpecialtyId = d.SpecialtyId
            WHERE cr.ClinicRoomId = @ClinicRoomId
              AND d.DoctorId = @DoctorId
              AND cr.Status = N'Active'
        )
        BEGIN
            RAISERROR(N'Phòng khám không phù hợp với chuyên khoa của bác sĩ.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        IF EXISTS (
            SELECT 1 FROM Appointments
            WHERE PatientId = @PatientId
              AND ScheduleId = @ScheduleId
              AND Status <> N'Cancelled'
        )
        BEGIN
            RAISERROR(N'Bệnh nhân đã đăng ký lịch khám này.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        INSERT INTO Appointments(PatientId, DoctorId, ScheduleId, MedicalServiceId, ClinicRoomId, Reason, Status, CreatedAt)
        VALUES(@PatientId, @DoctorId, @ScheduleId, @MedicalServiceId, @ClinicRoomId, @Reason, N'Pending', GETDATE());

        UPDATE DoctorSchedules
        SET CurrentPatients = CurrentPatients + 1
        WHERE ScheduleId = @ScheduleId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE TRIGGER trg_UpdateScheduleStatus
ON DoctorSchedules
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE DoctorSchedules
    SET Status = N'Full'
    WHERE CurrentPatients >= MaxPatients;

    UPDATE DoctorSchedules
    SET Status = N'Available'
    WHERE CurrentPatients < MaxPatients AND Status = N'Full';
END;
GO

CREATE TRIGGER trg_LogAppointmentInsert
ON Appointments
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO AppointmentLogs(AppointmentId, ActionName, ActionDate)
    SELECT AppointmentId, N'Created', GETDATE()
    FROM inserted;
END;
GO
