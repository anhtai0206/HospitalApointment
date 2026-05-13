# HospitalAppointmentSystem - Multi Project

Cấu trúc theo mô hình nhiều project:

- `HospitalAppointmentSystem.API`: ASP.NET Core MVC + Web API, Controllers, Views, Swagger.
- `HospitalAppointmentSystem.BLL`: Business Logic Layer, xử lý nghiệp vụ.
- `HospitalAppointmentSystem.DAL`: Data Access Layer, Entity Framework, LINQ, ADO.NET, Repository, DbContext.
- `HospitalAppointmentSystem.DTO`: Data Transfer Object dùng để truyền dữ liệu giữa các lớp.

## Cách chạy

1. Mở SQL Server Management Studio.
2. Chạy lần lượt:
   - `HospitalAppointmentSystem.API/Database/01_CreateDatabase.sql`
   - `HospitalAppointmentSystem.API/Database/02_Programmability.sql`
   - `HospitalAppointmentSystem.API/Database/03_SeedData.sql`
3. Mở solution bằng Visual Studio 2022.
4. Set `HospitalAppointmentSystem.API` làm Startup Project.
5. Kiểm tra chuỗi kết nối trong `HospitalAppointmentSystem.API/appsettings.json`.
6. Chạy project.

URL chính:

- `/` Trang chủ
- `/Doctor` Danh sách bác sĩ
- `/Appointment/Create` Đăng ký lịch khám
- `/Appointment/MyAppointments` Lịch khám của bệnh nhân demo
- `/Admin/Appointments` Quản lý lịch hẹn
- `/swagger` Test Web API

## Công nghệ đã dùng

- ASP.NET Core MVC
- ASP.NET Core Web API RESTful
- SQL Server
- Entity Framework Core
- LINQ
- ADO.NET
- Stored Procedure
- View
- Function
- Trigger
- Transaction
- JSON
