using HospitalAppointmentSystem.DAL.Repositories;
using HospitalAppointmentSystem.DTO;

namespace HospitalAppointmentSystem.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repo;
    public AuthService(IAuthRepository repo) => _repo = repo;

    public Task<UserSessionDTO?> LoginAsync(LoginDTO dto) => _repo.LoginAsync(dto);
    public Task<UserSessionDTO?> GetByUserIdAsync(int userId) => _repo.GetByUserIdAsync(userId);
    public Task<ApiResponse> RegisterPatientAsync(RegisterPatientDTO dto) => _repo.RegisterPatientAsync(dto);
    public Task<ApiResponse> ChangePasswordAsync(int userId, ChangePasswordDTO dto) => _repo.ChangePasswordAsync(userId, dto);
    public Task<ApiResponse> ResetPasswordAsync(ForgotPasswordDTO dto) => _repo.ResetPasswordAsync(dto);
    public Task<PatientProfileDTO?> GetPatientProfileAsync(int patientId) => _repo.GetPatientProfileAsync(patientId);
    public Task<ApiResponse> UpdatePatientProfileAsync(int patientId, PatientProfileDTO dto) => _repo.UpdatePatientProfileAsync(patientId, dto);
}
