# HospitalAppointmentSystem - Multi Project + Authentication

Project website đăng ký lịch khám bệnh theo kiến trúc nhiều project:

- `HospitalAppointmentSystem.API`: ASP.NET Core MVC + Web API + Swagger + Cookie Authentication
- `HospitalAppointmentSystem.BLL`: xử lý nghiệp vụ
- `HospitalAppointmentSystem.DAL`: Entity Framework, LINQ, ADO.NET, Repository, DbContext
- `HospitalAppointmentSystem.DTO`: đối tượng truyền dữ liệu

## Chức năng đã bổ sung

- Đăng nhập có chứng thực bằng Cookie Authentication.
- Phân quyền theo role: `Admin`, `Doctor`, `Patient`.
- Menu tự ẩn/hiện theo role.
- Patient: đăng ký lịch khám, xem lịch của tôi, hủy lịch của tôi, đổi mật khẩu.
- Doctor: xem lịch khám của bác sĩ, cập nhật trạng thái đã khám.
- Admin: quản lý toàn bộ lịch hẹn, xác nhận, hủy, cập nhật đã khám.
- Đăng ký tài khoản bệnh nhân.
- Đổi mật khẩu.
- Seed database mở rộng: 10 chuyên khoa, 15 bác sĩ, 7 bệnh nhân, 45 lịch làm việc và 10 lịch hẹn mẫu.

## Tài khoản demo

Mật khẩu của tất cả tài khoản mẫu: `123456`

### Admin

- `admin@clinic.vn`

### Bác sĩ

- `an@clinic.vn`
- `binh@clinic.vn`
- `cuong@clinic.vn`
- `duc@clinic.vn`
- `hanh@clinic.vn`
- `khanh@clinic.vn`
- `lan@clinic.vn`
- `nam@clinic.vn`
- `mai@clinic.vn`
- `phuc@clinic.vn`
- `son@clinic.vn`
- `thao@clinic.vn`
- `tu@clinic.vn`
- `quan@clinic.vn`
- `vy@clinic.vn`

### Bệnh nhân

- `khang.patient@clinic.vn`
- `an.patient@clinic.vn`
- `tam.patient@clinic.vn`
- `bao.patient@clinic.vn`
- `ha.patient@clinic.vn`
- `kiet.patient@clinic.vn`
- `patient.demo@clinic.vn`

## Cách chạy

1. Chạy các file SQL trong `HospitalAppointmentSystem.API/Database` theo thứ tự:
   - `01_CreateDatabase.sql`
   - `02_Programmability.sql`
   - `03_SeedData.sql`
2. Mở `HospitalAppointmentSystem.sln` bằng Visual Studio.
3. Set startup project là `HospitalAppointmentSystem.API`.
4. Kiểm tra connection string trong `HospitalAppointmentSystem.API/appsettings.json`.
5. Chạy project.

## Trang chính

- `/Account/Login`: Đăng nhập
- `/Account/Register`: Đăng ký bệnh nhân
- `/Appointment/Create`: Bệnh nhân đăng ký lịch khám
- `/Appointment/MyAppointments`: Bệnh nhân xem/hủy lịch của mình
- `/Doctor/MySchedule`: Bác sĩ xem lịch khám của mình
- `/Admin/Appointments`: Admin quản lý lịch hẹn
- `/Account/ChangePassword`: Đổi mật khẩu
- `/swagger`: Test Web API
