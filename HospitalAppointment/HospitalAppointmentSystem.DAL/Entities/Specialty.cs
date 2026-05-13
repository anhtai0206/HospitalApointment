using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.DAL.Entities;

public class Specialty
{
    [Key]
    public int SpecialtyId { get; set; }
    public string SpecialtyName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
