using HospitalAppointmentSystem.DTO;

namespace HospitalAppointmentSystem.BLL.Services;

public interface ISpecialtyService
{
    Task<List<SpecialtyDTO>> GetAllAsync();
}

public interface IDoctorService
{
    Task<List<DoctorDTO>> GetAllAsync(int? specialtyId = null);
    Task<DoctorDTO?> GetByIdAsync(int id);
}

public interface IScheduleService
{
    Task<List<ScheduleDTO>> GetAvailableAsync(int? doctorId = null);
}

public interface IAppointmentService
{
    Task<List<AppointmentDetailDTO>> GetAllAsync();
    Task<List<AppointmentDetailDTO>> GetByPatientAsync(int patientId);
    Task<ApiResponse> BookAppointmentAsync(AppointmentDTO dto);
    Task<ApiResponse> CancelAsync(int appointmentId);
    Task<ApiResponse> ConfirmAsync(int appointmentId);
}
