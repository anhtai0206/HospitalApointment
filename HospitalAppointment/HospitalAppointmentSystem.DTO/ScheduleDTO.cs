namespace HospitalAppointmentSystem.DTO;

public class ScheduleDTO
{
    public int ScheduleId { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string SpecialtyName { get; set; } = string.Empty;
    public DateTime WorkDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int MaxPatients { get; set; }
    public int CurrentPatients { get; set; }
    public int AvailableSlots { get; set; }
    public string Status { get; set; } = string.Empty;
}
