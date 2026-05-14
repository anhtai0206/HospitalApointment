using HospitalAppointmentSystem.DAL.Repositories;
using HospitalAppointmentSystem.DTO;

namespace HospitalAppointmentSystem.BLL.Services;

public class LookupService : ILookupService
{
    private readonly ILookupRepository _repository;
    public LookupService(ILookupRepository repository) => _repository = repository;

    public Task<List<MedicalServiceDTO>> GetMedicalServicesAsync() => _repository.GetMedicalServicesAsync();
    public Task<List<ClinicRoomDTO>> GetClinicRoomsAsync(int? specialtyId = null) => _repository.GetClinicRoomsAsync(specialtyId);
}
