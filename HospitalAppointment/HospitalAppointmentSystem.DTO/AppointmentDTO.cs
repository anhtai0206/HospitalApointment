using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.DTO;

public class AppointmentDTO
{
    public int AppointmentId { get; set; }

    [Required]
    public int PatientId { get; set; } = 1; // demo patient

    [Required]
    public int DoctorId { get; set; }

    [Required]
    public int ScheduleId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập lý do khám")]
    public string Reason { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }

    // Các field hỗ trợ form đăng ký trên web
    public string PatientName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string Address { get; set; } = string.Empty;
    public string HealthInsuranceNo { get; set; } = string.Empty;
}
