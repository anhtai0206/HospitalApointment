USE HospitalAppointmentDB;
GO

-- =========================================================
-- SEED DATA: dữ liệu mẫu cho hệ thống đăng ký lịch khám
-- Tài khoản đăng nhập demo: 8 tài khoản
--   + 1 Admin
--   + 2 Bác sĩ có quyền đăng nhập
--   + 5 Bệnh nhân có quyền đăng nhập
-- Dữ liệu hiển thị đặt lịch: 15 bác sĩ, 45 lịch làm việc
-- Lưu ý: Do bảng Doctors liên kết Users bằng UserId nên các bác sĩ hiển thị thêm
-- vẫn cần một dòng Users để lưu họ tên/email/sđt. Các dòng này có Role = DoctorProfile
-- và không dùng để đăng nhập hệ thống.
-- =========================================================

INSERT INTO Specialties(SpecialtyName, Description) VALUES
(N'Nội tổng quát', N'Khám và tư vấn các bệnh nội khoa thông thường'),
(N'Tim mạch', N'Khám, tư vấn và theo dõi bệnh tim mạch'),
(N'Da liễu', N'Khám các bệnh về da, tóc và móng'),
(N'Tai mũi họng', N'Khám tai, mũi, họng và các bệnh liên quan'),
(N'Nhi khoa', N'Khám bệnh trẻ em'),
(N'Răng hàm mặt', N'Khám và điều trị các vấn đề răng miệng'),
(N'Mắt', N'Khám mắt, đo thị lực và tư vấn bệnh lý về mắt'),
(N'Sản phụ khoa', N'Khám phụ khoa và tư vấn sức khỏe sinh sản'),
(N'Cơ xương khớp', N'Khám đau nhức xương khớp, thoái hóa, chấn thương'),
(N'Thần kinh', N'Khám đau đầu, mất ngủ, chóng mặt và bệnh lý thần kinh');

INSERT INTO MedicalServices(ServiceName, Price, Description) VALUES
(N'Khám dịch vụ', 250000, N'Ưu tiên thời gian, tư vấn kỹ hơn'),
(N'Khám thường', 120000, N'Khám theo quy trình thông thường'),
(N'Khám VIP - thương nhân', 600000, N'Dịch vụ cao cấp, thời gian chờ thấp'),
(N'Tái khám nội trú', 80000, N'Tái khám sau quá trình điều trị nội trú');

-- Mỗi chuyên khoa có 5 phòng khám để bệnh nhân chọn khi đặt lịch
INSERT INTO ClinicRooms(SpecialtyId, RoomName, Status) VALUES
(1, N'Phòng nội tổng quát 1', N'Active'), (1, N'Phòng nội tổng quát 2', N'Active'), (1, N'Phòng nội tổng quát 3', N'Active'), (1, N'Phòng nội tổng quát 4', N'Active'), (1, N'Phòng nội tổng quát 5', N'Active'),
(2, N'Phòng tim mạch 1', N'Active'), (2, N'Phòng tim mạch 2', N'Active'), (2, N'Phòng tim mạch 3', N'Active'), (2, N'Phòng tim mạch 4', N'Active'), (2, N'Phòng tim mạch 5', N'Active'),
(3, N'Phòng da liễu 1', N'Active'), (3, N'Phòng da liễu 2', N'Active'), (3, N'Phòng da liễu 3', N'Active'), (3, N'Phòng da liễu 4', N'Active'), (3, N'Phòng da liễu 5', N'Active'),
(4, N'Phòng tai mũi họng 1', N'Active'), (4, N'Phòng tai mũi họng 2', N'Active'), (4, N'Phòng tai mũi họng 3', N'Active'), (4, N'Phòng tai mũi họng 4', N'Active'), (4, N'Phòng tai mũi họng 5', N'Active'),
(5, N'Phòng nhi khoa 1', N'Active'), (5, N'Phòng nhi khoa 2', N'Active'), (5, N'Phòng nhi khoa 3', N'Active'), (5, N'Phòng nhi khoa 4', N'Active'), (5, N'Phòng nhi khoa 5', N'Active'),
(6, N'Phòng răng hàm mặt 1', N'Active'), (6, N'Phòng răng hàm mặt 2', N'Active'), (6, N'Phòng răng hàm mặt 3', N'Active'), (6, N'Phòng răng hàm mặt 4', N'Active'), (6, N'Phòng răng hàm mặt 5', N'Active'),
(7, N'Phòng mắt 1', N'Active'), (7, N'Phòng mắt 2', N'Active'), (7, N'Phòng mắt 3', N'Active'), (7, N'Phòng mắt 4', N'Active'), (7, N'Phòng mắt 5', N'Active'),
(8, N'Phòng sản phụ khoa 1', N'Active'), (8, N'Phòng sản phụ khoa 2', N'Active'), (8, N'Phòng sản phụ khoa 3', N'Active'), (8, N'Phòng sản phụ khoa 4', N'Active'), (8, N'Phòng sản phụ khoa 5', N'Active'),
(9, N'Phòng cơ xương khớp 1', N'Active'), (9, N'Phòng cơ xương khớp 2', N'Active'), (9, N'Phòng cơ xương khớp 3', N'Active'), (9, N'Phòng cơ xương khớp 4', N'Active'), (9, N'Phòng cơ xương khớp 5', N'Active'),
(10, N'Phòng thần kinh 1', N'Active'), (10, N'Phòng thần kinh 2', N'Active'), (10, N'Phòng thần kinh 3', N'Active'), (10, N'Phòng thần kinh 4', N'Active'), (10, N'Phòng thần kinh 5', N'Active');

-- 8 tài khoản đăng nhập thật để demo
INSERT INTO Users(FullName, Email, PasswordHash, Phone, Role) VALUES
(N'Quản trị viên hệ thống', 'admin@clinic.vn', '123456', '0900000000', 'Admin'),
(N'BS. Nguyễn Văn An', 'an@clinic.vn', '123456', '0901000001', 'Doctor'),
(N'BS. Trần Thị Bình', 'binh@clinic.vn', '123456', '0901000002', 'Doctor'),
(N'Bệnh nhân Nguyễn Minh Khang', 'khang.patient@clinic.vn', '123456', '0912000001', 'Patient'),
(N'Bệnh nhân Trần Hoài An', 'an.patient@clinic.vn', '123456', '0912000002', 'Patient'),
(N'Bệnh nhân Lê Thanh Tâm', 'tam.patient@clinic.vn', '123456', '0912000003', 'Patient'),
(N'Bệnh nhân Phạm Quốc Bảo', 'bao.patient@clinic.vn', '123456', '0912000004', 'Patient'),
(N'Bệnh nhân Demo', 'patient.demo@clinic.vn', '123456', '0911111111', 'Patient');

-- 13 hồ sơ bác sĩ bổ sung để hiển thị ở trang đặt lịch, không dùng làm tài khoản đăng nhập
INSERT INTO Users(FullName, Email, PasswordHash, Phone, Role) VALUES
(N'BS. Lê Văn Cường', 'cuong.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000003', 'DoctorProfile'),
(N'BS. Phạm Minh Đức', 'duc.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000004', 'DoctorProfile'),
(N'BS. Hoàng Thị Hạnh', 'hanh.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000005', 'DoctorProfile'),
(N'BS. Võ Quốc Khánh', 'khanh.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000006', 'DoctorProfile'),
(N'BS. Đặng Thu Lan', 'lan.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000007', 'DoctorProfile'),
(N'BS. Bùi Hải Nam', 'nam.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000008', 'DoctorProfile'),
(N'BS. Nguyễn Ngọc Mai', 'mai.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000009', 'DoctorProfile'),
(N'BS. Trương Gia Phúc', 'phuc.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000010', 'DoctorProfile'),
(N'BS. Huỳnh Thanh Sơn', 'son.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000011', 'DoctorProfile'),
(N'BS. Đỗ Thị Thảo', 'thao.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000012', 'DoctorProfile'),
(N'BS. Lâm Anh Tú', 'tu.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000013', 'DoctorProfile'),
(N'BS. Vũ Minh Quân', 'quan.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000014', 'DoctorProfile'),
(N'BS. Đinh Hoài Vy', 'vy.profile@clinic.vn', CONVERT(VARCHAR(36), NEWID()), '0901000015', 'DoctorProfile');

-- DoctorId 1..15 tương ứng 15 bác sĩ hiển thị ở trang đặt lịch
INSERT INTO Doctors(UserId, SpecialtyId, Qualification, ExperienceYears, Description) VALUES
(2, 1, N'Thạc sĩ', 8, N'Bác sĩ nội tổng quát, tư vấn bệnh lý người lớn'),
(3, 2, N'Chuyên khoa I', 10, N'Bác sĩ chuyên tim mạch và huyết áp'),
(9, 3, N'Bác sĩ', 6, N'Bác sĩ da liễu, điều trị mụn và viêm da'),
(10, 4, N'Chuyên khoa I', 9, N'Bác sĩ tai mũi họng'),
(11, 5, N'Thạc sĩ', 12, N'Bác sĩ nhi khoa'),
(12, 6, N'Bác sĩ', 7, N'Bác sĩ răng hàm mặt'),
(13, 7, N'Chuyên khoa I', 11, N'Bác sĩ chuyên khoa mắt'),
(14, 8, N'Thạc sĩ', 13, N'Bác sĩ sản phụ khoa'),
(15, 9, N'Chuyên khoa I', 10, N'Bác sĩ cơ xương khớp'),
(16, 10, N'Thạc sĩ', 14, N'Bác sĩ chuyên khoa thần kinh'),
(17, 1, N'Bác sĩ', 5, N'Bác sĩ nội tổng quát'),
(18, 2, N'Thạc sĩ', 9, N'Bác sĩ tim mạch can thiệp'),
(19, 3, N'Chuyên khoa I', 8, N'Bác sĩ da liễu thẩm mỹ'),
(20, 4, N'Bác sĩ', 6, N'Bác sĩ tai mũi họng trẻ em'),
(21, 5, N'Chuyên khoa I', 10, N'Bác sĩ nhi khoa tổng quát');

-- PatientId 1..5 tương ứng 5 tài khoản bệnh nhân demo
INSERT INTO Patients(UserId, Gender, DateOfBirth, Address, HealthInsuranceNo) VALUES
(4, N'Nam', '2001-04-12', N'Quận 1, TP.HCM', 'BHYT001'),
(5, N'Nữ', '1999-08-23', N'Quận 3, TP.HCM', 'BHYT002'),
(6, N'Nam', '2003-01-05', N'Bình Thạnh, TP.HCM', 'BHYT003'),
(7, N'Nam', '1995-11-18', N'Gò Vấp, TP.HCM', 'BHYT004'),
(8, N'Nam', '2005-12-27', N'TP.HCM', 'BHYT005');

-- Tạo lịch làm việc cho 15 bác sĩ, mỗi bác sĩ 3 khung giờ để bệnh nhân có nhiều lựa chọn khi đặt lịch
INSERT INTO DoctorSchedules(DoctorId, WorkDate, StartTime, EndTime, MaxPatients, CurrentPatients, Status) VALUES
(1, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), '08:00', '09:00', 5, 0, N'Available'),
(1, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), '09:00', '10:00', 5, 0, N'Available'),
(1, DATEADD(DAY, 2, CAST(GETDATE() AS DATE)), '13:00', '14:00', 5, 0, N'Available'),
(2, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), '08:00', '09:00', 4, 0, N'Available'),
(2, DATEADD(DAY, 2, CAST(GETDATE() AS DATE)), '09:00', '10:00', 4, 0, N'Available'),
(2, DATEADD(DAY, 3, CAST(GETDATE() AS DATE)), '14:00', '15:00', 4, 0, N'Available'),
(3, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), '10:00', '11:00', 6, 0, N'Available'),
(3, DATEADD(DAY, 2, CAST(GETDATE() AS DATE)), '13:00', '14:00', 6, 0, N'Available'),
(3, DATEADD(DAY, 4, CAST(GETDATE() AS DATE)), '15:00', '16:00', 6, 0, N'Available'),
(4, DATEADD(DAY, 2, CAST(GETDATE() AS DATE)), '08:00', '09:00', 5, 0, N'Available'),
(4, DATEADD(DAY, 3, CAST(GETDATE() AS DATE)), '09:00', '10:00', 5, 0, N'Available'),
(4, DATEADD(DAY, 5, CAST(GETDATE() AS DATE)), '14:00', '15:00', 5, 0, N'Available'),
(5, DATEADD(DAY, 2, CAST(GETDATE() AS DATE)), '10:00', '11:00', 5, 0, N'Available'),
(5, DATEADD(DAY, 3, CAST(GETDATE() AS DATE)), '13:00', '14:00', 5, 0, N'Available'),
(5, DATEADD(DAY, 5, CAST(GETDATE() AS DATE)), '15:00', '16:00', 5, 0, N'Available'),
(6, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), '13:00', '14:00', 4, 0, N'Available'),
(6, DATEADD(DAY, 2, CAST(GETDATE() AS DATE)), '14:00', '15:00', 4, 0, N'Available'),
(6, DATEADD(DAY, 6, CAST(GETDATE() AS DATE)), '08:00', '09:00', 4, 0, N'Available'),
(7, DATEADD(DAY, 3, CAST(GETDATE() AS DATE)), '08:00', '09:00', 5, 0, N'Available'),
(7, DATEADD(DAY, 3, CAST(GETDATE() AS DATE)), '09:00', '10:00', 5, 0, N'Available'),
(7, DATEADD(DAY, 6, CAST(GETDATE() AS DATE)), '13:00', '14:00', 5, 0, N'Available'),
(8, DATEADD(DAY, 3, CAST(GETDATE() AS DATE)), '10:00', '11:00', 5, 0, N'Available'),
(8, DATEADD(DAY, 4, CAST(GETDATE() AS DATE)), '08:00', '09:00', 5, 0, N'Available'),
(8, DATEADD(DAY, 6, CAST(GETDATE() AS DATE)), '14:00', '15:00', 5, 0, N'Available'),
(9, DATEADD(DAY, 4, CAST(GETDATE() AS DATE)), '09:00', '10:00', 4, 0, N'Available'),
(9, DATEADD(DAY, 4, CAST(GETDATE() AS DATE)), '10:00', '11:00', 4, 0, N'Available'),
(9, DATEADD(DAY, 7, CAST(GETDATE() AS DATE)), '13:00', '14:00', 4, 0, N'Available'),
(10, DATEADD(DAY, 4, CAST(GETDATE() AS DATE)), '13:00', '14:00', 5, 0, N'Available'),
(10, DATEADD(DAY, 5, CAST(GETDATE() AS DATE)), '08:00', '09:00', 5, 0, N'Available'),
(10, DATEADD(DAY, 7, CAST(GETDATE() AS DATE)), '14:00', '15:00', 5, 0, N'Available'),
(11, DATEADD(DAY, 5, CAST(GETDATE() AS DATE)), '09:00', '10:00', 6, 0, N'Available'),
(11, DATEADD(DAY, 5, CAST(GETDATE() AS DATE)), '10:00', '11:00', 6, 0, N'Available'),
(11, DATEADD(DAY, 7, CAST(GETDATE() AS DATE)), '15:00', '16:00', 6, 0, N'Available'),
(12, DATEADD(DAY, 5, CAST(GETDATE() AS DATE)), '13:00', '14:00', 5, 0, N'Available'),
(12, DATEADD(DAY, 6, CAST(GETDATE() AS DATE)), '09:00', '10:00', 5, 0, N'Available'),
(12, DATEADD(DAY, 8, CAST(GETDATE() AS DATE)), '08:00', '09:00', 5, 0, N'Available'),
(13, DATEADD(DAY, 6, CAST(GETDATE() AS DATE)), '10:00', '11:00', 5, 0, N'Available'),
(13, DATEADD(DAY, 6, CAST(GETDATE() AS DATE)), '13:00', '14:00', 5, 0, N'Available'),
(13, DATEADD(DAY, 8, CAST(GETDATE() AS DATE)), '09:00', '10:00', 5, 0, N'Available'),
(14, DATEADD(DAY, 6, CAST(GETDATE() AS DATE)), '14:00', '15:00', 4, 0, N'Available'),
(14, DATEADD(DAY, 7, CAST(GETDATE() AS DATE)), '08:00', '09:00', 4, 0, N'Available'),
(14, DATEADD(DAY, 8, CAST(GETDATE() AS DATE)), '10:00', '11:00', 4, 0, N'Available'),
(15, DATEADD(DAY, 7, CAST(GETDATE() AS DATE)), '09:00', '10:00', 5, 0, N'Available'),
(15, DATEADD(DAY, 7, CAST(GETDATE() AS DATE)), '13:00', '14:00', 5, 0, N'Available'),
(15, DATEADD(DAY, 8, CAST(GETDATE() AS DATE)), '14:00', '15:00', 5, 0, N'Available');

-- Lịch hẹn mẫu để Admin/Bác sĩ/Bệnh nhân có dữ liệu thao tác
EXEC sp_BookAppointment 1, 1, 1, 2, 1, N'Đau đầu, mệt mỏi';
EXEC sp_BookAppointment 2, 2, 4, 1, 6, N'Tư vấn huyết áp';
EXEC sp_BookAppointment 3, 3, 7, 1, 11, N'Nổi mẩn đỏ trên da';
EXEC sp_BookAppointment 4, 4, 10, 2, 16, N'Đau họng, nghẹt mũi';
EXEC sp_BookAppointment 5, 5, 13, 3, 21, N'Khám sức khỏe cho trẻ';
EXEC sp_BookAppointment 1, 6, 16, 2, 26, N'Đau răng';
EXEC sp_BookAppointment 2, 7, 19, 1, 31, N'Kiểm tra thị lực';
EXEC sp_BookAppointment 3, 8, 22, 3, 36, N'Tư vấn sức khỏe sinh sản';
EXEC sp_BookAppointment 4, 9, 25, 2, 41, N'Đau khớp gối';
EXEC sp_BookAppointment 5, 10, 28, 4, 46, N'Mất ngủ kéo dài';

UPDATE Appointments SET Status = N'Confirmed' WHERE AppointmentId IN (1, 2, 3);
UPDATE Appointments SET Status = N'Completed' WHERE AppointmentId IN (4);
GO
