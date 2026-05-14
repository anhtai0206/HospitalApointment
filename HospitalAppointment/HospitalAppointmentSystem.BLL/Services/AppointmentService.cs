using HospitalAppointmentSystem.DAL.Repositories;
using HospitalAppointmentSystem.DTO;

namespace HospitalAppointmentSystem.BLL.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IScheduleRepository _scheduleRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository, IScheduleRepository scheduleRepository)
    {
        _appointmentRepository = appointmentRepository;
        _scheduleRepository = scheduleRepository;
    }

    public Task<List<AppointmentDetailDTO>> GetAllAsync() => _appointmentRepository.GetAllAsync();
    public Task<List<AppointmentDetailDTO>> GetByPatientAsync(int patientId) => _appointmentRepository.GetByPatientAsync(patientId);
    public Task<List<AppointmentDetailDTO>> GetByDoctorAsync(int doctorId) => _appointmentRepository.GetByDoctorAsync(doctorId);

    public async Task<ApiResponse> BookAppointmentAsync(AppointmentDTO dto)
    {
        if (dto.PatientId <= 0) return ApiResponse.Fail("Thiếu mã bệnh nhân");
        if (dto.DoctorId <= 0) return ApiResponse.Fail("Vui lòng chọn bác sĩ");
        if (dto.ScheduleId <= 0) return ApiResponse.Fail("Vui lòng chọn lịch khám");
        if (dto.MedicalServiceId <= 0) return ApiResponse.Fail("Vui lòng chọn dịch vụ khám");
        if (dto.ClinicRoomId <= 0) return ApiResponse.Fail("Vui lòng chọn phòng khám");
        if (string.IsNullOrWhiteSpace(dto.Reason)) return ApiResponse.Fail("Vui lòng nhập lý do khám");

        var schedule = await _scheduleRepository.GetByIdAsync(dto.ScheduleId);
        if (schedule == null) return ApiResponse.Fail("Lịch khám không tồn tại");
        if (schedule.WorkDate.Date < DateTime.Today) return ApiResponse.Fail("Không thể đặt lịch trong quá khứ");
        if (schedule.AvailableSlots <= 0) return ApiResponse.Fail("Lịch khám đã đầy");
        if (schedule.DoctorId != dto.DoctorId) return ApiResponse.Fail("Bác sĩ không khớp với lịch khám đã chọn");

        return await _appointmentRepository.BookAppointmentAsync(dto);
    }

    public Task<ApiResponse> CancelAsync(int appointmentId) => _appointmentRepository.CancelAsync(appointmentId);
    public Task<ApiResponse> ConfirmAsync(int appointmentId) => _appointmentRepository.ConfirmAsync(appointmentId);
    public Task<ApiResponse> CompleteAsync(int appointmentId) => _appointmentRepository.CompleteAsync(appointmentId);
}
