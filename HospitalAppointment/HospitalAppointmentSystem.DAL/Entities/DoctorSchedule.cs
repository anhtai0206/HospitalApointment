using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.DAL.Entities;

public class DoctorSchedule
{
    [Key]
    public int ScheduleId { get; set; }
    public int DoctorId { get; set; }
    public DateTime WorkDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int MaxPatients { get; set; }
    public int CurrentPatients { get; set; }
    public string Status { get; set; } = "Available";

    public Doctor? Doctor { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
