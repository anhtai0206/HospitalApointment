using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.DTO;

public class AppointmentDTO
{
    public int AppointmentId { get; set; }

    [Required]
    public int PatientId { get; set; } = 1; // demo patient

    [Required]
    public int DoctorId { get; set; }

    // ScheduleId sẽ được tạo/tìm tự động ở SQL Server khi bệnh nhân đăng ký theo ngày và ca khám.
    public int ScheduleId { get; set; }

    [Required]
    public DateTime WorkDate { get; set; }

    [Required]
    public string ShiftCode { get; set; } = string.Empty;

    [Required]
    public int MedicalServiceId { get; set; }

    [Required]
    public int ClinicRoomId { get; set; }

    public string? Reason { get; set; }

    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
}
