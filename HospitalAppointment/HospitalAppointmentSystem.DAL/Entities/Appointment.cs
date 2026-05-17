using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.DAL.Entities;

public class Appointment
{
    [Key]
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int ScheduleId { get; set; }
    public int MedicalServiceId { get; set; }
    public int ClinicRoomId { get; set; }
    public string? Reason { get; set; }
    public string? CancelReason { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }

    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    public DoctorSchedule? DoctorSchedule { get; set; }
    public MedicalService? MedicalService { get; set; }
    public ClinicRoom? ClinicRoom { get; set; }
}
