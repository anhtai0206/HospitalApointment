using HospitalAppointmentSystem.DAL.Data;
using HospitalAppointmentSystem.DAL.Entities;
using HospitalAppointmentSystem.DTO;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.DAL.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly HospitalDbContext _db;
    public AuthRepository(HospitalDbContext db) => _db = db;

    public async Task<UserSessionDTO?> LoginAsync(LoginDTO dto)
    {
        var email = dto.Email.Trim().ToLower();
        var user = await _db.Users
            .Include(u => u.Patient)
            .Include(u => u.Doctor)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email
                                      && u.PasswordHash == dto.Password
                                      && (u.Role == "Admin" || u.Role == "Doctor" || u.Role == "Patient"));

        return user == null ? null : ToSession(user);
    }

    public async Task<UserSessionDTO?> GetByUserIdAsync(int userId)
    {
        var user = await _db.Users
            .Include(u => u.Patient)
            .Include(u => u.Doctor)
            .FirstOrDefaultAsync(u => u.UserId == userId);

        return user == null ? null : ToSession(user);
    }

    public async Task<ApiResponse> RegisterPatientAsync(RegisterPatientDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return ApiResponse.Fail("Vui lòng nhập email và mật khẩu");

        if (dto.Password != dto.ConfirmPassword)
            return ApiResponse.Fail("Xác nhận mật khẩu không khớp");

        var email = dto.Email.Trim().ToLower();
        if (await _db.Users.AnyAsync(u => u.Email.ToLower() == email))
            return ApiResponse.Fail("Email đã tồn tại trong hệ thống");

        var displayName = email.Contains('@') ? email.Split('@')[0] : email;
        var user = new User
        {
            FullName = displayName,
            Email = dto.Email.Trim(),
            PasswordHash = dto.Password,
            Phone = string.Empty,
            Role = "Patient",
            CreatedAt = DateTime.Now
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        _db.Patients.Add(new Patient
        {
            UserId = user.UserId,
            Gender = string.Empty,
            DateOfBirth = null,
            Address = string.Empty,
            HealthInsuranceNo = string.Empty
        });
        await _db.SaveChangesAsync();
        return ApiResponse.Ok("Đăng ký tài khoản thành công. Vui lòng đăng nhập và cập nhật thông tin bệnh nhân.");
    }

    public async Task<ApiResponse> ChangePasswordAsync(int userId, ChangePasswordDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.OldPassword) || string.IsNullOrWhiteSpace(dto.NewPassword))
            return ApiResponse.Fail("Vui lòng nhập đầy đủ mật khẩu");

        if (dto.NewPassword != dto.ConfirmPassword)
            return ApiResponse.Fail("Xác nhận mật khẩu mới không khớp");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null) return ApiResponse.Fail("Không tìm thấy người dùng");
        if (user.PasswordHash != dto.OldPassword) return ApiResponse.Fail("Mật khẩu cũ không đúng");

        user.PasswordHash = dto.NewPassword;
        await _db.SaveChangesAsync();
        return ApiResponse.Ok("Đổi mật khẩu thành công");
    }

    public async Task<ApiResponse> ResetPasswordAsync(ForgotPasswordDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.NewPassword))
            return ApiResponse.Fail("Vui lòng nhập email và mật khẩu mới");

        if (dto.NewPassword != dto.ConfirmPassword)
            return ApiResponse.Fail("Xác nhận mật khẩu mới không khớp");

        var email = dto.Email.Trim().ToLower();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);
        if (user == null) return ApiResponse.Fail("Không tìm thấy tài khoản có email này");

        user.PasswordHash = dto.NewPassword;
        await _db.SaveChangesAsync();
        return ApiResponse.Ok("Đặt lại mật khẩu thành công. Bạn có thể đăng nhập bằng mật khẩu mới.");
    }

    public async Task<PatientProfileDTO?> GetPatientProfileAsync(int patientId)
    {
        return await _db.Patients
            .Include(p => p.User)
            .Where(p => p.PatientId == patientId)
            .Select(p => new PatientProfileDTO
            {
                PatientId = p.PatientId,
                UserId = p.UserId,
                FullName = p.User!.FullName,
                Email = p.User.Email,
                Phone = p.User.Phone,
                Gender = p.Gender,
                DateOfBirth = p.DateOfBirth,
                Address = p.Address,
                HealthInsuranceNo = p.HealthInsuranceNo
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ApiResponse> UpdatePatientProfileAsync(int patientId, PatientProfileDTO dto)
    {
        var patient = await _db.Patients.Include(p => p.User).FirstOrDefaultAsync(p => p.PatientId == patientId);
        if (patient == null || patient.User == null) return ApiResponse.Fail("Không tìm thấy hồ sơ bệnh nhân");

        if (string.IsNullOrWhiteSpace(dto.FullName))
            return ApiResponse.Fail("Vui lòng nhập họ tên bệnh nhân");

        patient.User.FullName = dto.FullName.Trim();
        patient.User.Phone = dto.Phone ?? string.Empty;
        patient.Gender = dto.Gender ?? string.Empty;
        patient.DateOfBirth = dto.DateOfBirth;
        patient.Address = dto.Address ?? string.Empty;
        patient.HealthInsuranceNo = dto.HealthInsuranceNo ?? string.Empty;

        await _db.SaveChangesAsync();
        return ApiResponse.Ok("Cập nhật thông tin bệnh nhân thành công");
    }

    private static UserSessionDTO ToSession(User user) => new()
    {
        UserId = user.UserId,
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role,
        PatientId = user.Patient?.PatientId,
        DoctorId = user.Doctor?.DoctorId
    };
}
