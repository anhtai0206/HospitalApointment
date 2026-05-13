using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.DAL.Entities;

public class Appointment
{
    [Key]
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int ScheduleId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }

    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    public DoctorSchedule? DoctorSchedule { get; set; }
}
