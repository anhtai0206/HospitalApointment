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

    [Required]
    public int MedicalServiceId { get; set; }

    [Required]
    public int ClinicRoomId { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
}
