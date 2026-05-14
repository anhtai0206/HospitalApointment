using System;

namespace HospitalAppointmentSystem.DTO;

public class PatientProfileDTO
{
    public int PatientId { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string Address { get; set; } = string.Empty;
    public string HealthInsuranceNo { get; set; } = string.Empty;
}
