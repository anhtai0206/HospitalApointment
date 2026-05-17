# MEDBOOK - Website đăng ký lịch khám bệnh

## 1. Thông tin đồ án

**Tên đề tài:** Website đăng ký lịch khám bệnh MEDBOOK  
**Loại ứng dụng:** Website ASP.NET Core MVC kết hợp Web API  
**Cơ sở dữ liệu:** SQL Server  
**Kiến trúc:** 3 lớp theo mô hình nhiều project  
**Mục tiêu:** Hỗ trợ bệnh nhân xem bác sĩ, chọn chuyên khoa, đặt lịch khám, quản lý lịch khám cá nhân; hỗ trợ bác sĩ và quản trị viên quản lý lịch hẹn theo vai trò.

---

## 2. Phiên bản phần mềm và công nghệ sử dụng

| Thành phần | Phiên bản sử dụng trong đồ án |
|---|---|
| Visual Studio | Visual Studio 2022 |
| .NET SDK | .NET 8.0 |
| ASP.NET Core | ASP.NET Core 8.0 |
| Ngôn ngữ lập trình | C# 12 |
| SQL Server | SQL Server 2022 |
| SQL Server Management Studio | SSMS 20.x |
| Entity Framework Core | 8.0.8 |
| Microsoft.Data.SqlClient | 5.2.2 |
| Swashbuckle.AspNetCore / Swagger | 6.7.3 |
| CloudinaryDotNet | Dùng cho upload ảnh bệnh nhân |
| Frontend | Razor View, HTML5, CSS3, JavaScript |
| Authentication | ASP.NET Core Cookie Authentication |
| Định dạng trao đổi dữ liệu API | JSON |
| Quản lý mã nguồn | Git / GitHub |

---

## 3. Cấu trúc project

Đồ án được tổ chức theo mô hình nhiều project để thể hiện rõ kiến trúc 3 lớp.

```text
HospitalAppointmentSystem
│
├── HospitalAppointmentSystem.API
├── HospitalAppointmentSystem.BLL
├── HospitalAppointmentSystem.DAL
└── HospitalAppointmentSystem.DTO
```

### 3.1. HospitalAppointmentSystem.API

Đây là lớp giao diện và dịch vụ.

Thành phần chính:

```text
Controllers
Views
wwwroot
Program.cs
appsettings.json
Database
```

Vai trò:

- Hiển thị giao diện website cho người dùng.
- Xử lý request từ trình duyệt.
- Cung cấp Web API theo chuẩn RESTful.
- Trả dữ liệu JSON cho các API.
- Xử lý đăng nhập, đăng xuất, phân quyền theo vai trò.
- Gọi lớp BLL để xử lý nghiệp vụ.

---

### 3.2. HospitalAppointmentSystem.BLL

Đây là lớp xử lý nghiệp vụ.

Vai trò:

- Kiểm tra logic đăng ký lịch khám.
- Kiểm tra quyền của người dùng theo vai trò.
- Điều phối xử lý giữa Controller và DAL.
- Xử lý nghiệp vụ đăng nhập, đổi mật khẩu, cập nhật thông tin bệnh nhân.
- Xử lý nghiệp vụ xác nhận, hủy, hoàn tất lịch khám.

Các service tiêu biểu:

```text
AppointmentService
DoctorService
SpecialtyService
ScheduleService
AuthService
PatientService
```

---

### 3.3. HospitalAppointmentSystem.DAL

Đây là lớp truy xuất dữ liệu.

Vai trò:

- Kết nối với SQL Server.
- Truy vấn dữ liệu bằng Entity Framework Core.
- Sử dụng LINQ để lọc, sắp xếp và lấy dữ liệu.
- Gọi Stored Procedure bằng ADO.NET.
- Quản lý DbContext và Repository.

Các thành phần tiêu biểu:

```text
HospitalDbContext
AppointmentRepository
DoctorRepository
SpecialtyRepository
ScheduleRepository
AuthRepository
PatientRepository
```

---

### 3.4. HospitalAppointmentSystem.DTO

Đây là lớp chứa các đối tượng trao đổi dữ liệu giữa các lớp.

Vai trò:

- Truyền dữ liệu giữa API, BLL và DAL.
- Giúp đóng gói dữ liệu.
- Hạn chế truyền trực tiếp Entity lên giao diện.
- Giúp hệ thống dễ bảo trì và mở rộng.

Các DTO tiêu biểu:

```text
AppointmentDTO
AppointmentDetailDTO
DoctorDTO
SpecialtyDTO
ScheduleDTO
PatientInfoDTO
LoginDTO
RegisterDTO
ChangePasswordDTO
UserSessionDTO
ApiResponse
```

---

## 4. Chức năng chính của hệ thống

### 4.1. Chức năng bệnh nhân

- Đăng ký tài khoản bệnh nhân.
- Đăng nhập hệ thống.
- Cập nhật thông tin bệnh nhân.
- Tải ảnh hồ sơ bệnh nhân lên Cloudinary.
- Xem danh sách bác sĩ.
- Tìm kiếm bác sĩ theo tên.
- Lọc bác sĩ theo chuyên khoa.
- Chọn chuyên khoa, bác sĩ, ngày khám, ca khám, dịch vụ và phòng khám.
- Đặt lịch khám.
- Xem danh sách lịch khám của bản thân.
- Xem trạng thái lịch khám.
- Xem lý do hủy nếu lịch bị hủy.

### 4.2. Chức năng bác sĩ

- Đăng nhập bằng tài khoản bác sĩ.
- Xem danh sách lịch khám của mình.
- Xác nhận lịch hẹn.
- Cập nhật trạng thái đã khám.
- Hủy lịch hẹn kèm lý do hủy nếu cần.

### 4.3. Chức năng quản trị viên

- Đăng nhập bằng tài khoản Admin.
- Quản lý danh sách lịch hẹn.
- Xác nhận lịch hẹn.
- Hủy lịch hẹn kèm lý do hủy nếu cần.
- Cập nhật trạng thái đã khám.
- Theo dõi dữ liệu lịch hẹn toàn hệ thống.
- Sử dụng Swagger để kiểm thử Web API.

### 4.4. Chức năng giao diện

- Giao diện trang chủ tích hợp chọn bác sĩ và đặt lịch khám.
- Chuyển đổi ngôn ngữ Tiếng Việt / English.
- Giao diện đăng nhập, đăng ký và đổi mật khẩu.
- Footer chứa thông tin phòng khám, giờ làm việc, liên hệ và Google Map.
- Nút liên hệ nổi gồm Hotline, Facebook và Zalo.

---

## 5. Các kỹ thuật trọng tâm đã sử dụng trong đồ án

## 5.1. Lập trình cơ sở dữ liệu bằng T-SQL

Đồ án có sử dụng T-SQL để xây dựng các thành phần lập trình phía máy chủ trong SQL Server.

| Kỹ thuật | Trạng thái | Vị trí áp dụng |
|---|---|---|
| View | Có sử dụng | File `02_Programmability.sql` |
| Function | Có sử dụng | File `02_Programmability.sql` |
| Stored Procedure | Có sử dụng | File `02_Programmability.sql` |
| Trigger | Có sử dụng | File `02_Programmability.sql` |
| Transaction | Có sử dụng | Stored Procedure đặt lịch khám |

### View

View được dùng để tổng hợp dữ liệu lịch hẹn từ nhiều bảng.

Ví dụ áp dụng:

```text
vw_AppointmentDetails
```

Mục đích:

- Hiển thị lịch khám của bệnh nhân.
- Hiển thị lịch khám của bác sĩ.
- Hiển thị danh sách lịch hẹn cho Admin.
- Gom dữ liệu từ bảng bệnh nhân, bác sĩ, chuyên khoa, lịch làm việc, dịch vụ và phòng khám.

---

### Function

Function được sử dụng để hỗ trợ xử lý dữ liệu trong SQL Server.

Ví dụ áp dụng:

```text
fn_GetAvailableSlot
```

Trong các phiên bản trước, function này được dùng để tính số lượt khám còn lại. Ở phiên bản hiện tại, logic giới hạn slot đã được bỏ theo yêu cầu nghiệp vụ mới, nhưng function vẫn là minh chứng cho thành phần T-SQL bắt buộc của đồ án.

---

### Stored Procedure

Stored Procedure được sử dụng để thực hiện nghiệp vụ đặt lịch khám.

Ví dụ áp dụng:

```text
sp_BookAppointment
```

Mục đích:

- Nhận thông tin bệnh nhân, bác sĩ, ngày khám, ca khám, dịch vụ, phòng khám và lý do khám.
- Tự tạo lịch làm việc của bác sĩ nếu lịch đó chưa tồn tại.
- Tạo lịch hẹn khám bệnh.
- Kiểm tra ràng buộc bệnh nhân không đặt trùng cùng một ca trong cùng một ngày.
- Đảm bảo dữ liệu được xử lý trực tiếp tại SQL Server.

---

### Trigger

Trigger được sử dụng để tự động ghi nhận một số thao tác trong hệ thống.

Ví dụ áp dụng:

```text
trg_LogAppointmentInsert
```

Mục đích:

- Tự động ghi log khi có lịch hẹn mới được tạo.
- Hỗ trợ theo dõi lịch sử thao tác dữ liệu.

---

### Transaction

Transaction được sử dụng trong Stored Procedure đặt lịch khám.

Mục đích:

- Đảm bảo khi tạo lịch hẹn, dữ liệu phải được xử lý đồng bộ.
- Nếu một bước lỗi thì toàn bộ giao tác được rollback.
- Tránh tình trạng dữ liệu lịch hẹn bị thêm thiếu hoặc sai lệch.

---

## 5.2. Kiến trúc phần mềm 3 lớp

Đồ án được xây dựng theo kiến trúc 3 lớp.

| Lớp | Project tương ứng | Vai trò |
|---|---|---|
| Presentation Layer | `HospitalAppointmentSystem.API` | Giao diện web, Controller, Web API |
| Business Logic Layer | `HospitalAppointmentSystem.BLL` | Xử lý nghiệp vụ |
| Data Access Layer | `HospitalAppointmentSystem.DAL` | Truy xuất dữ liệu |
| DTO Layer | `HospitalAppointmentSystem.DTO` | Đối tượng truyền dữ liệu |

Luồng xử lý chính:

```text
View / API Controller
        ↓
BLL Service
        ↓
DAL Repository
        ↓
SQL Server
```

Ý nghĩa:

- Tách giao diện khỏi xử lý nghiệp vụ.
- Tách xử lý nghiệp vụ khỏi truy xuất dữ liệu.
- Dễ bảo trì, dễ mở rộng.
- Phù hợp yêu cầu kiến trúc 3 lớp của môn học.

---

## 5.3. Đối tượng trao đổi dữ liệu DTO

Đồ án có sử dụng DTO để truyền dữ liệu giữa các lớp.

Ví dụ:

```text
AppointmentDTO
AppointmentDetailDTO
DoctorDTO
PatientInfoDTO
LoginDTO
RegisterDTO
ApiResponse
```

Ý nghĩa:

- Không truyền trực tiếp Entity database lên View.
- Giúp dữ liệu truyền giữa các lớp rõ ràng hơn.
- Tăng tính đóng gói.
- Giảm phụ thuộc giữa giao diện và cấu trúc database.

---

## 5.4. ADO.NET

Đồ án có sử dụng ADO.NET để gọi Stored Procedure.

Kỹ thuật áp dụng:

```text
SqlConnection
SqlCommand
SqlParameter
CommandType.StoredProcedure
ExecuteNonQueryAsync
```

Vị trí áp dụng:

```text
AppointmentRepository
```

Chức năng áp dụng:

```text
Đăng ký lịch khám
```

Mục đích:

- Gọi Stored Procedure `sp_BookAppointment`.
- Thực hiện nghiệp vụ đặt lịch khám trực tiếp thông qua SQL Server.
- Kết hợp với Transaction trong T-SQL để bảo đảm toàn vẹn dữ liệu.

### Ghi chú về DataSet và DataAdapter

Yêu cầu môn học có đề cập mô hình phi kết nối như `DataSet` và `DataAdapter`. Đồ án có thể bổ sung phần báo cáo hoặc chức năng thống kê lịch hẹn bằng `SqlDataAdapter` và `DataSet` để minh chứng thêm cho mô hình phi kết nối.

---

## 5.5. LINQ

Đồ án có sử dụng LINQ trong mã nguồn C#.

Các kỹ thuật LINQ được dùng:

```text
Where
Select
OrderBy
FirstOrDefault
ToList
Any
Include
```

Vị trí áp dụng:

```text
DoctorRepository
AppointmentRepository
ScheduleRepository
SpecialtyRepository
PatientRepository
```

Chức năng áp dụng:

- Tìm kiếm bác sĩ theo tên.
- Lọc bác sĩ theo chuyên khoa.
- Sắp xếp danh sách bác sĩ theo mã.
- Lấy lịch khám theo bệnh nhân.
- Lấy lịch khám theo bác sĩ.
- Lấy danh sách chuyên khoa, dịch vụ và phòng khám.

### Ghi chú về LINQ to XML

Yêu cầu môn học có đề cập LINQ to XML. Đồ án hiện tập trung dùng LINQ với Entity Framework và LINQ to Objects. Có thể bổ sung thêm file XML cấu hình dịch vụ khám hoặc chuyên khoa để minh chứng LINQ to XML nếu giảng viên yêu cầu đầy đủ tuyệt đối.

---

## 5.6. Entity Framework Core

Đồ án có sử dụng Entity Framework Core để thao tác với SQL Server.

Thành phần sử dụng:

```text
DbContext
DbSet
Entity
Repository
LINQ Query
Relationship Mapping
```

Vị trí áp dụng:

```text
HospitalDbContext
```

Các DbSet tiêu biểu:

```text
Users
Doctors
Patients
Specialties
DoctorSchedules
Appointments
AppointmentLogs
MedicalServices
ClinicRooms
```

Mục đích:

- Ánh xạ bảng trong SQL Server sang class C#.
- Truy vấn dữ liệu bằng LINQ.
- Quản lý quan hệ giữa các bảng.
- Kết hợp với Repository để thao tác dữ liệu.

### Database First và Code First

Đồ án sử dụng cách tiếp cận kết hợp:

- Database được tạo bằng script SQL Server.
- Entity, DbContext và DbSet được xây dựng trong C# để ánh xạ dữ liệu.
- Cách làm này thể hiện tư duy Database First ở phần thiết kế CSDL và Code First ở phần mô hình hóa entity trong mã nguồn.

---

## 5.7. Web API

Đồ án có xây dựng Web API để thao tác với cơ sở dữ liệu.

Vị trí áp dụng:

```text
HospitalAppointmentSystem.API/Controllers
```

Các nhóm API tiêu biểu:

```text
/api/doctors
/api/specialties
/api/schedules
/api/appointments
```

Mục đích:

- Lấy danh sách bác sĩ.
- Lấy danh sách chuyên khoa.
- Lấy thông tin lịch khám.
- Đăng ký lịch khám.
- Cập nhật trạng thái lịch khám.
- Hủy lịch hẹn.

---

## 5.8. RESTful API và JSON

Đồ án có áp dụng nguyên tắc RESTful API.

| HTTP Method | Mục đích |
|---|---|
| GET | Lấy dữ liệu |
| POST | Tạo mới dữ liệu |
| PUT | Cập nhật dữ liệu |
| DELETE / PUT Cancel | Hủy hoặc thay đổi trạng thái |

Dữ liệu trao đổi giữa client và server sử dụng định dạng JSON.

Ví dụ dữ liệu đặt lịch:

```json
{
  "patientId": 1,
  "doctorId": 2,
  "workDate": "2026-05-17",
  "shiftCode": "MORNING",
  "medicalServiceId": 1,
  "clinicRoomId": 1,
  "reason": "Đau đầu"
}
```

Ví dụ dữ liệu phản hồi:

```json
{
  "success": true,
  "message": "Đăng ký lịch khám thành công"
}
```

---

## 6. Phân quyền người dùng

Hệ thống có phân quyền theo vai trò.

| Vai trò | Quyền sử dụng |
|---|---|
| Admin | Quản lý lịch hẹn toàn hệ thống, xác nhận, hủy, cập nhật đã khám |
| Doctor | Xem lịch khám của mình, xác nhận, hủy, cập nhật đã khám |
| Patient | Cập nhật hồ sơ, đặt lịch, xem lịch khám của bản thân |

Cơ chế xác thực:

```text
ASP.NET Core Cookie Authentication
```

Cơ chế phân quyền:

```text
Role-based Authorization
```

---

## 7. Cấu hình dịch vụ ngoài

Đồ án có tích hợp Cloudinary để lưu ảnh hồ sơ bệnh nhân.

Thông tin cấu hình nằm trong:

```text
appsettings.json
```

Chức năng liên quan:

- Bệnh nhân tải ảnh hồ sơ.
- Ảnh được upload lên Cloudinary.
- Đường dẫn ảnh được lưu trong database.

---

## 8. Danh sách tài khoản mẫu

Mật khẩu tài khoản mẫu:

```text
A12345@
```

### Admin

```text
admin@clinic.vn
```

### Bác sĩ

```text
an@clinic.vn
binh@clinic.vn
cuong@clinic.vn
duc@clinic.vn
hanh@clinic.vn
khanh@clinic.vn
```

### Bệnh nhân

```text
patient.demo@clinic.vn
```

---

## 9. Tổng kết các yêu cầu đã đáp ứng

| Yêu cầu | Trạng thái |
|---|---|
| View T-SQL | Đã sử dụng |
| Function T-SQL | Đã sử dụng |
| Stored Procedure | Đã sử dụng |
| Trigger | Đã sử dụng |
| Transaction | Đã sử dụng |
| Kiến trúc 3 lớp | Đã sử dụng |
| DTO | Đã sử dụng |
| ADO.NET | Đã sử dụng |
| DataSet, DataAdapter | Có thể bổ sung thêm để minh chứng rõ hơn |
| LINQ | Đã sử dụng |
| LINQ to XML | Có thể bổ sung thêm nếu giảng viên yêu cầu |
| Entity Framework Core | Đã sử dụng |
| DbContext, DbSet | Đã sử dụng |
| Database First | Thể hiện qua thiết kế database bằng SQL script |
| Code First | Thể hiện qua Entity, DbContext, DbSet trong C# |
| Web API | Đã sử dụng |
| RESTful API | Đã sử dụng |
| JSON | Đã sử dụng |
| Authentication | Đã sử dụng |
| Role Authorization | Đã sử dụng |
| Cloudinary upload | Đã sử dụng |

---

## 10. Nội dung tự nghiên cứu và phát triển nâng cao

Trong quá trình thực hiện, đồ án có áp dụng thêm một số nội dung nâng cao ngoài CRUD cơ bản:

- Cookie Authentication trong ASP.NET Core.
- Phân quyền theo vai trò Admin, Doctor, Patient.
- Web API RESTful kết hợp Swagger.
- Upload ảnh bệnh nhân lên Cloudinary.
- Chuyển đổi ngôn ngữ Tiếng Việt / English.
- Tạo lịch khám tự động khi bệnh nhân đăng ký.
- Tích hợp Google Map trong footer.
- Popup thông báo thay cho alert hoặc box thông báo đầu trang.
- Tìm kiếm bác sĩ theo tên ngay khi nhập.
- Kiểm tra ngày khám trong quá khứ bằng JavaScript.
- Hủy lịch có ghi nhận lý do hủy.
- Ẩn thao tác không hợp lệ theo trạng thái lịch khám.

