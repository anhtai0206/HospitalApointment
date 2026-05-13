using HospitalAppointmentSystem.DAL.Repositories;
using HospitalAppointmentSystem.DTO;

namespace HospitalAppointmentSystem.BLL.Services;

public class SpecialtyService : ISpecialtyService
{
    private readonly ISpecialtyRepository _repository;
    public SpecialtyService(ISpecialtyRepository repository) => _repository = repository;
    public Task<List<SpecialtyDTO>> GetAllAsync() => _repository.GetAllAsync();
}
