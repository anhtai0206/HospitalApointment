using HospitalAppointmentSystem.DTO;

namespace HospitalAppointmentSystem.BLL.Services;

public interface IDoctorService
{
    Task<List<DoctorDTO>> GetAllAsync(int? specialtyId = null);
    Task<DoctorDTO?> GetByIdAsync(int id);
}

public interface ISpecialtyService
{
    Task<List<SpecialtyDTO>> GetAllAsync();
}

public interface IAppointmentService
{
    Task<ApiResponse> BookAppointmentAsync(AppointmentDTO dto);
    Task<List<AppointmentDetailDTO>> GetByPatientAsync(int patientId);
    Task<List<AppointmentDetailDTO>> GetAllAsync();
    Task<List<AppointmentDetailDTO>> GetByDoctorAsync(int doctorId);
    Task<ApiResponse> ConfirmAsync(int id);
    Task<ApiResponse> CompleteAsync(int id);
    Task<ApiResponse> CancelAsync(int id);
}

public interface IScheduleService
{
    Task<List<ScheduleDTO>> GetAvailableAsync(int? doctorId = null, int? specialtyId = null);
}

public interface IAuthService
{
    Task<UserSessionDTO?> LoginAsync(LoginDTO dto);
    Task<UserSessionDTO?> GetByUserIdAsync(int userId);
    Task<ApiResponse> RegisterPatientAsync(RegisterPatientDTO dto);
    Task<ApiResponse> ChangePasswordAsync(int userId, ChangePasswordDTO dto);
    Task<ApiResponse> ResetPasswordAsync(ForgotPasswordDTO dto);
    Task<PatientProfileDTO?> GetPatientProfileAsync(int patientId);
    Task<ApiResponse> UpdatePatientProfileAsync(int patientId, PatientProfileDTO dto);
}

public interface ILookupService
{
    Task<List<MedicalServiceDTO>> GetMedicalServicesAsync();
    Task<List<ClinicRoomDTO>> GetClinicRoomsAsync(int? specialtyId = null);
}
