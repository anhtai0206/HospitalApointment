using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.DAL.Entities;

public class Doctor
{
    [Key]
    public int DoctorId { get; set; }
    public int UserId { get; set; }
    public int SpecialtyId { get; set; }
    public string Qualification { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public string Description { get; set; } = string.Empty;

    public User? User { get; set; }
    public Specialty? Specialty { get; set; }
    public ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
