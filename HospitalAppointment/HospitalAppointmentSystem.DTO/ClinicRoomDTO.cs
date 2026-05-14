namespace HospitalAppointmentSystem.DTO;

public class ClinicRoomDTO
{
    public int ClinicRoomId { get; set; }
    public int SpecialtyId { get; set; }
    public string SpecialtyName { get; set; } = string.Empty;
    public string RoomName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
