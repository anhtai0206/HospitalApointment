using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.DAL.Entities;

public class AppointmentLog
{
    [Key]
    public int LogId { get; set; }
    public int AppointmentId { get; set; }
    public string ActionName { get; set; } = string.Empty;
    public DateTime ActionDate { get; set; }
}
