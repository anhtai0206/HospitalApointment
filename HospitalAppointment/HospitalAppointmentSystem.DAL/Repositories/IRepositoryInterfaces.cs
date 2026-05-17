using HospitalAppointmentSystem.DTO;

namespace HospitalAppointmentSystem.DAL.Repositories;

public interface IDoctorRepository
{
    Task<List<DoctorDTO>> GetAllAsync(int? specialtyId = null);
    Task<DoctorDTO?> GetByIdAsync(int id);
}

public interface ISpecialtyRepository
{
    Task<List<SpecialtyDTO>> GetAllAsync();
}

public interface IScheduleRepository
{
    Task<List<ScheduleDTO>> GetAvailableAsync(int? doctorId = null, int? specialtyId = null);
    Task<ScheduleDTO?> GetByIdAsync(int id);
}

public interface IAppointmentRepository
{
    Task<ApiResponse> BookAppointmentAsync(AppointmentDTO dto);
    Task<List<AppointmentDetailDTO>> GetByPatientAsync(int patientId);
    Task<List<AppointmentDetailDTO>> GetAllAsync();
    Task<List<AppointmentDetailDTO>> GetByDoctorAsync(int doctorId);
    Task<ApiResponse> CancelAsync(int appointmentId, string? cancelReason = null);
    Task<ApiResponse> ConfirmAsync(int appointmentId);
    Task<ApiResponse> CompleteAsync(int appointmentId);
}

public interface IAuthRepository
{
    Task<UserSessionDTO?> LoginAsync(LoginDTO dto);
    Task<UserSessionDTO?> GetByUserIdAsync(int userId);
    Task<ApiResponse> RegisterPatientAsync(RegisterPatientDTO dto);
    Task<ApiResponse> ChangePasswordAsync(int userId, ChangePasswordDTO dto);
    Task<ApiResponse> ResetPasswordAsync(ForgotPasswordDTO dto);
    Task<PatientProfileDTO?> GetPatientProfileAsync(int patientId);
    Task<ApiResponse> UpdatePatientProfileAsync(int patientId, PatientProfileDTO dto);
}

public interface ILookupRepository
{
    Task<List<MedicalServiceDTO>> GetMedicalServicesAsync();
    Task<List<ClinicRoomDTO>> GetClinicRoomsAsync(int? specialtyId = null);
}
