USE HospitalAppointmentDB;
GO

INSERT INTO Specialties(SpecialtyName, Description) VALUES
(N'Nội tổng quát', N'Khám bệnh nội khoa thông thường'),
(N'Tim mạch', N'Khám và tư vấn bệnh tim mạch'),
(N'Da liễu', N'Khám bệnh da liễu'),
(N'Tai mũi họng', N'Khám tai mũi họng'),
(N'Nhi khoa', N'Khám bệnh trẻ em');

INSERT INTO Users(FullName, Email, PasswordHash, Phone, Role) VALUES
(N'Quản trị viên', 'admin@clinic.vn', '123456', '0900000000', 'Admin'),
(N'Bệnh nhân Demo', 'patient.demo@clinic.vn', '123456', '0911111111', 'Patient'),
(N'BS. Nguyễn Văn An', 'an@clinic.vn', '123456', '0922222222', 'Doctor'),
(N'BS. Trần Thị Bình', 'binh@clinic.vn', '123456', '0933333333', 'Doctor'),
(N'BS. Lê Văn Cường', 'cuong@clinic.vn', '123456', '0944444444', 'Doctor');

INSERT INTO Patients(UserId, Gender, DateOfBirth, Address, HealthInsuranceNo)
VALUES (2, N'Nam', '2005-12-27', N'TP.HCM', 'BHYT001');

INSERT INTO Doctors(UserId, SpecialtyId, Qualification, ExperienceYears, Description) VALUES
(3, 1, N'Thạc sĩ', 8, N'Bác sĩ nội tổng quát'),
(4, 2, N'Chuyên khoa I', 10, N'Bác sĩ tim mạch'),
(5, 3, N'Bác sĩ', 6, N'Bác sĩ da liễu');

INSERT INTO DoctorSchedules(DoctorId, WorkDate, StartTime, EndTime, MaxPatients, CurrentPatients, Status) VALUES
(1, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), '08:00', '09:00', 5, 0, N'Available'),
(1, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), '09:00', '10:00', 5, 0, N'Available'),
(2, DATEADD(DAY, 2, CAST(GETDATE() AS DATE)), '08:00', '09:00', 4, 0, N'Available'),
(2, DATEADD(DAY, 2, CAST(GETDATE() AS DATE)), '09:00', '10:00', 4, 0, N'Available'),
(3, DATEADD(DAY, 3, CAST(GETDATE() AS DATE)), '13:00', '14:00', 6, 0, N'Available'),
(3, DATEADD(DAY, 3, CAST(GETDATE() AS DATE)), '14:00', '15:00', 6, 0, N'Available');
GO
