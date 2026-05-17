# HospitalAppointmentSystem - Multi Project + Authentication

Project website đăng ký lịch khám bệnh theo kiến trúc nhiều project:

- `HospitalAppointmentSystem.API`: ASP.NET Core MVC + Web API + Swagger + Cookie Authentication
- `HospitalAppointmentSystem.BLL`: xử lý nghiệp vụ
- `HospitalAppointmentSystem.DAL`: Entity Framework, LINQ, ADO.NET, Repository, DbContext
- `HospitalAppointmentSystem.DTO`: đối tượng truyền dữ liệu

## Các chỉnh sửa mới

- Đổi font chữ sang nhóm font hỗ trợ tiếng Việt tốt hơn: `Segoe UI`, `Roboto`, `Arial`.
- Mật khẩu tài khoản mẫu đổi thành `A12345@`.
- Đăng ký, đổi mật khẩu và quên mật khẩu đều kiểm tra mật khẩu phải có ít nhất 1 chữ viết hoa và 1 ký tự đặc biệt.
- Xóa tài khoản `DoctorProfile`; database mẫu chỉ còn 1 Admin, 1 Bệnh nhân và 6 Bác sĩ.
- Danh sách bác sĩ/lịch khám được sắp xếp theo mã bác sĩ để không bị hiển thị ngẫu nhiên.
- Thông báo thành công/lỗi đổi từ box đầu trang sang popup giữa màn hình.
- Lý do khám không bắt buộc, có thể để trống vẫn đăng ký được.

## Tài khoản demo

Mật khẩu của tất cả tài khoản mẫu: `A12345@`

### Admin

- `admin@clinic.vn`

### Bác sĩ

- `an@clinic.vn`
- `binh@clinic.vn`
- `cuong@clinic.vn`
- `duc@clinic.vn`
- `hanh@clinic.vn`
- `khanh@clinic.vn`

### Bệnh nhân

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

Lưu ý: cần chạy lại `01_CreateDatabase.sql` để xóa dữ liệu cũ có `DoctorProfile` và tạo lại database mới.

## Trang chính

- `/Account/Login`: Đăng nhập
- `/Account/Register`: Đăng ký bệnh nhân
- `/Appointment/Create`: Bệnh nhân đăng ký lịch khám
- `/Appointment/MyAppointments`: Bệnh nhân xem/hủy lịch của mình
- `/Doctor/MySchedule`: Bác sĩ xem lịch khám của mình
- `/Admin/Appointments`: Admin quản lý lịch hẹn
- `/Account/PatientInfo`: Cập nhật thông tin bệnh nhân
- `/Account/ChangePassword`: Đổi mật khẩu
- `/swagger`: Test Web API, chỉ hiển thị trong menu của Admin

## Cấu hình Cloudinary để lưu ảnh bệnh nhân

Chức năng Thông tin bệnh nhân cho phép tải ảnh hồ sơ 3x4 lên Cloudinary. Trước khi dùng chức năng này, mở file:

```text
HospitalAppointmentSystem.API/appsettings.json
```

Thay các giá trị sau bằng thông tin tài khoản Cloudinary của bạn:

```json
"Cloudinary": {
  "CloudName": "YOUR_CLOUD_NAME",
  "ApiKey": "YOUR_API_KEY",
  "ApiSecret": "YOUR_API_SECRET",
  "Folder": "hospital-patient-photos"
}
```

Sau đó chạy lại database vì bảng Patients đã thêm cột PhotoUrl:

```text
01_CreateDatabase.sql
02_Programmability.sql
03_SeedData.sql
```

Ảnh cho phép tải lên: JPG, PNG, WEBP; dung lượng tối đa 2MB. Đường dẫn ảnh Cloudinary được lưu trong cột Patients.PhotoUrl.

## Bản tích hợp trang chủ - bác sĩ - đặt lịch

Bản này đã gộp Trang chủ, Danh sách bác sĩ và Đăng ký lịch khám vào một trang chính `/`.

Quy trình đặt lịch mới:
1. Chọn chuyên khoa.
2. Chọn bác sĩ thuộc chuyên khoa.
3. Chọn ngày khám, mặc định là ngày hiện tại theo máy người dùng.
4. Chọn 1 trong 4 ca khám:
   - Ca sáng: 07:30 - 11:30
   - Ca chiều: 13:30 - 17:30
5. Chọn dịch vụ, phòng khám và nhập lý do khám nếu có.

Database đã được cập nhật để không tạo sẵn lịch theo tháng; lịch được tự tạo khi bệnh nhân đăng ký, gồm 2 ca cố định và không giới hạn slot.

## Bản Fix cuối: giao diện, lịch hẹn, avatar bác sĩ

Các điểm đã chỉnh:
- Nút con mắt hiện/ẩn mật khẩu hoạt động đồng bộ ở đăng nhập, đăng ký, quên mật khẩu và đổi mật khẩu.
- Danh sách bác sĩ dùng avatar mặc định riêng cho bác sĩ; có thêm cột `Doctors.PhotoUrl` để sau này có thể cập nhật ảnh bác sĩ.
- Tìm kiếm bác sĩ tự lọc theo tên ngay khi nhập, vẫn giữ nút Tìm kiếm.
- Chọn ngày khám trong quá khứ sẽ hiện popup cảnh báo và tự chuyển về ngày hiện tại.
- Bác sĩ có thêm nút Xác nhận lịch hẹn trước khi cập nhật Đã khám.
- Sửa lỗi hủy lịch hẹn ở trang Admin.
- Cookie đăng nhập đổi sang `MedBook.Auth.v2`, chỉ lưu lâu khi tích Ghi nhớ đăng nhập.
- Cho phép bệnh nhân đặt nhiều lịch với cùng một bác sĩ miễn là lịch còn slot và hợp lệ.
- Lý do khám được phép để trống; khi trống sẽ hiển thị “Không ghi”, không còn lỗi `SqlNullValueException`.

Lưu ý: bản này có thêm cột `Doctors.PhotoUrl`, nên nếu database cũ chưa có cột này, hãy chạy lại 3 file SQL trong thư mục `HospitalAppointmentSystem.API/Database` theo thứ tự.
