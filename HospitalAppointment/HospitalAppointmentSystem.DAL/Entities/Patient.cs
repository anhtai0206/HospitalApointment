using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.DAL.Entities;

public class Patient
{
    [Key]
    public int PatientId { get; set; }
    public int UserId { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string Address { get; set; } = string.Empty;
    public string HealthInsuranceNo { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;

    public User? User { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
