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
        if (dto.WorkDate == default) return ApiResponse.Fail("Vui lòng chọn ngày khám");
        if (dto.WorkDate.Date < DateTime.Today) return ApiResponse.Fail("Không thể đặt lịch trong quá khứ");
        if (string.IsNullOrWhiteSpace(dto.ShiftCode)) return ApiResponse.Fail("Vui lòng chọn ca khám");

        var validShifts = new[] { "MORNING", "AFTERNOON" };
        if (!validShifts.Contains(dto.ShiftCode.Trim().ToUpper()))
            return ApiResponse.Fail("Ca khám không hợp lệ");

        dto.ShiftCode = dto.ShiftCode.Trim().ToUpper();

        if (dto.MedicalServiceId <= 0) return ApiResponse.Fail("Vui lòng chọn dịch vụ khám");
        if (dto.ClinicRoomId <= 0) return ApiResponse.Fail("Vui lòng chọn phòng khám");

        return await _appointmentRepository.BookAppointmentAsync(dto);
    }

    public Task<ApiResponse> CancelAsync(int appointmentId, string? cancelReason = null) => _appointmentRepository.CancelAsync(appointmentId, cancelReason);
    public Task<ApiResponse> ConfirmAsync(int appointmentId) => _appointmentRepository.ConfirmAsync(appointmentId);
    public Task<ApiResponse> CompleteAsync(int appointmentId) => _appointmentRepository.CompleteAsync(appointmentId);
}
