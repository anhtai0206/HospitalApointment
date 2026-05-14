namespace HospitalAppointmentSystem.DTO;

public class UserSessionDTO
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int? PatientId { get; set; }
    public int? DoctorId { get; set; }
}
