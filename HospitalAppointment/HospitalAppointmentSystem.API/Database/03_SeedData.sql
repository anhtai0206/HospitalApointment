USE HospitalAppointmentDB;
GO

-- =========================================================
-- SEED DATA: dữ liệu mẫu gọn, đúng vai trò đăng nhập
-- Mật khẩu tất cả tài khoản: A12345@
-- Tài khoản mẫu:
--   + 1 Admin
--   + 6 Bác sĩ
--   + 1 Bệnh nhân
-- Không dùng tài khoản DoctorProfile để tránh phát sinh mật khẩu không hợp lệ.
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

INSERT INTO Users(FullName, Email, PasswordHash, Phone, Role) VALUES
(N'Quản trị viên hệ thống', 'admin@clinic.vn', 'A12345@', '0900000000', 'Admin'),
(N'BS. Nguyễn Văn An', 'an@clinic.vn', 'A12345@', '0901000001', 'Doctor'),
(N'BS. Trần Thị Bình', 'binh@clinic.vn', 'A12345@', '0901000002', 'Doctor'),
(N'BS. Lê Văn Cường', 'cuong@clinic.vn', 'A12345@', '0901000003', 'Doctor'),
(N'BS. Phạm Minh Đức', 'duc@clinic.vn', 'A12345@', '0901000004', 'Doctor'),
(N'BS. Hoàng Thị Hạnh', 'hanh@clinic.vn', 'A12345@', '0901000005', 'Doctor'),
(N'BS. Võ Quốc Khánh', 'khanh@clinic.vn', 'A12345@', '0901000006', 'Doctor'),
(N'Bệnh nhân Demo', 'patient.demo@clinic.vn', 'A12345@', '0911111111', 'Patient');

INSERT INTO Doctors(UserId, SpecialtyId, Qualification, ExperienceYears, Description, PhotoUrl) VALUES
(2, 1, N'Thạc sĩ', 8, N'Bác sĩ nội tổng quát, tư vấn bệnh lý người lớn', NULL),
(3, 2, N'Chuyên khoa I', 10, N'Bác sĩ chuyên tim mạch và huyết áp', N'/images/default-female-doctor.png'),
(4, 3, N'Bác sĩ', 6, N'Bác sĩ da liễu, điều trị mụn và viêm da', NULL),
(5, 4, N'Chuyên khoa I', 9, N'Bác sĩ tai mũi họng', NULL),
(6, 5, N'Thạc sĩ', 12, N'Bác sĩ nhi khoa', N'/images/default-female-doctor.png'),
(7, 6, N'Bác sĩ', 7, N'Bác sĩ răng hàm mặt', NULL);

INSERT INTO Patients(UserId, Gender, DateOfBirth, Address, HealthInsuranceNo, PhotoUrl) VALUES
(8, N'Nam', '2005-12-27', N'TP.HCM', 'BHYT005', N'');


-- Không tạo sẵn lịch khám cho cả tháng.
-- Khi bệnh nhân đăng ký, Stored Procedure sp_BookAppointment sẽ tự tìm hoặc tạo lịch mới
-- theo bác sĩ + ngày khám + ca khám. Không tạo sẵn lịch và không giới hạn slot.
GO
