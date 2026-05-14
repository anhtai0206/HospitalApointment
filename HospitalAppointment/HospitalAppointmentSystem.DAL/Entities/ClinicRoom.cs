using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.DAL.Entities;

public class ClinicRoom
{
    [Key]
    public int ClinicRoomId { get; set; }
    public int SpecialtyId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";

    public Specialty? Specialty { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
