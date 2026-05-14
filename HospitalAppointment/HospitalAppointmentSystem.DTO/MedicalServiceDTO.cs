namespace HospitalAppointmentSystem.DTO;

public class MedicalServiceDTO
{
    public int MedicalServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
}
