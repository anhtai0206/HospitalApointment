using HospitalAppointmentSystem.DTO;

namespace HospitalAppointmentSystem.DAL.Repositories;

public interface ISpecialtyRepository
{
    Task<List<SpecialtyDTO>> GetAllAsync();
}

public interface IDoctorRepository
{
    Task<List<DoctorDTO>> GetAllAsync(int? specialtyId = null);
    Task<DoctorDTO?> GetByIdAsync(int id);
}

public interface IScheduleRepository
{
    Task<List<ScheduleDTO>> GetAvailableAsync(int? doctorId = null);
    Task<ScheduleDTO?> GetByIdAsync(int id);
}

public interface IAppointmentRepository
{
    Task<List<AppointmentDetailDTO>> GetAllAsync();
    Task<List<AppointmentDetailDTO>> GetByPatientAsync(int patientId);
    Task<ApiResponse> BookAppointmentAsync(AppointmentDTO dto);
    Task<ApiResponse> CancelAsync(int appointmentId);
    Task<ApiResponse> ConfirmAsync(int appointmentId);
    Task<System.Data.DataSet> GetAppointmentsDisconnectedAsync();
}
