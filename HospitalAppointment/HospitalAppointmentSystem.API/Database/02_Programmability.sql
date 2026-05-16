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
    ISNULL(a.Reason, N'') AS Reason,
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
    @WorkDate DATE,
    @ShiftCode VARCHAR(10),
    @MedicalServiceId INT,
    @ClinicRoomId INT,
    @Reason NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @ScheduleId INT;
    DECLARE @StartTime TIME;
    DECLARE @EndTime TIME;
    DECLARE @MaxPatients INT = 20;

    IF @ShiftCode = 'CA1'
    BEGIN
        SET @StartTime = '07:30';
        SET @EndTime = '09:30';
    END
    ELSE IF @ShiftCode = 'CA2'
    BEGIN
        SET @StartTime = '09:45';
        SET @EndTime = '12:00';
    END
    ELSE IF @ShiftCode = 'CA3'
    BEGIN
        SET @StartTime = '13:30';
        SET @EndTime = '15:30';
    END
    ELSE IF @ShiftCode = 'CA4'
    BEGIN
        SET @StartTime = '15:45';
        SET @EndTime = '18:00';
    END
    ELSE
    BEGIN
        RAISERROR(N'Ca khám không hợp lệ.', 16, 1);
        RETURN;
    END;

    IF @WorkDate < CONVERT(DATE, GETDATE())
    BEGIN
        RAISERROR(N'Không thể đặt lịch trong quá khứ.', 16, 1);
        RETURN;
    END;

    BEGIN TRY
        SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Patients WHERE PatientId = @PatientId)
        BEGIN
            RAISERROR(N'Bệnh nhân không tồn tại.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        IF NOT EXISTS (SELECT 1 FROM Doctors WHERE DoctorId = @DoctorId)
        BEGIN
            RAISERROR(N'Bác sĩ không tồn tại.', 16, 1);
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

        -- Tìm lịch theo bác sĩ + ngày + ca. Nếu chưa có thì tự tạo mới.
        SELECT @ScheduleId = ScheduleId
        FROM DoctorSchedules WITH (UPDLOCK, HOLDLOCK)
        WHERE DoctorId = @DoctorId
          AND WorkDate = @WorkDate
          AND StartTime = @StartTime
          AND EndTime = @EndTime;

        IF @ScheduleId IS NULL
        BEGIN
            INSERT INTO DoctorSchedules(DoctorId, WorkDate, StartTime, EndTime, MaxPatients, CurrentPatients, Status)
            VALUES(@DoctorId, @WorkDate, @StartTime, @EndTime, @MaxPatients, 0, N'Available');

            SET @ScheduleId = SCOPE_IDENTITY();
        END;

        IF EXISTS (
            SELECT 1
            FROM DoctorSchedules WITH (UPDLOCK, HOLDLOCK)
            WHERE ScheduleId = @ScheduleId
              AND (Status = N'Cancelled' OR CurrentPatients >= MaxPatients)
        )
        BEGIN
            RAISERROR(N'Ca khám đã đầy hoặc đã bị hủy.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        -- Một bệnh nhân không được đặt trùng cùng một ngày và cùng một ca.
        -- Ví dụ: đã đặt ngày 15 ca 1 thì không được đặt thêm ca 1 ngày 15 nữa,
        -- nhưng vẫn được đặt ca 2, ca 3 hoặc ca 4 trong cùng ngày nếu còn slot.
        -- Các bệnh nhân khác vẫn được đặt cùng bác sĩ, cùng ngày, cùng ca cho đến khi đủ 20 slot.
        IF EXISTS (
            SELECT 1
            FROM Appointments a WITH (UPDLOCK, HOLDLOCK)
            JOIN DoctorSchedules ds ON a.ScheduleId = ds.ScheduleId
            WHERE a.PatientId = @PatientId
              AND ds.WorkDate = @WorkDate
              AND ds.StartTime = @StartTime
              AND ds.EndTime = @EndTime
              AND a.Status <> N'Cancelled'
        )
        BEGIN
            RAISERROR(N'Bạn đã đăng ký ca khám này trong ngày đã chọn. Vui lòng chọn ca khác hoặc ngày khác.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        INSERT INTO Appointments(PatientId, DoctorId, ScheduleId, MedicalServiceId, ClinicRoomId, Reason, Status, CreatedAt)
        VALUES(@PatientId, @DoctorId, @ScheduleId, @MedicalServiceId, @ClinicRoomId, NULLIF(LTRIM(RTRIM(@Reason)), N''), N'Pending', GETDATE());

        UPDATE DoctorSchedules
        SET CurrentPatients = CurrentPatients + 1,
            Status = CASE WHEN CurrentPatients + 1 >= MaxPatients THEN N'Full' ELSE N'Available' END
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
