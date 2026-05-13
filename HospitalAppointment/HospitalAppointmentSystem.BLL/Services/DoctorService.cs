using HospitalAppointmentSystem.DAL.Repositories;
using HospitalAppointmentSystem.DTO;

namespace HospitalAppointmentSystem.BLL.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _repository;
    public DoctorService(IDoctorRepository repository) => _repository = repository;

    public Task<List<DoctorDTO>> GetAllAsync(int? specialtyId = null) => _repository.GetAllAsync(specialtyId);
    public Task<DoctorDTO?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
}
