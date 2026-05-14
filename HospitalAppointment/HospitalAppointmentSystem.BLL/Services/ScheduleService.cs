using HospitalAppointmentSystem.DAL.Repositories;
using HospitalAppointmentSystem.DTO;

namespace HospitalAppointmentSystem.BLL.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _repository;
    public ScheduleService(IScheduleRepository repository) => _repository = repository;

    public Task<List<ScheduleDTO>> GetAvailableAsync(int? doctorId = null, int? specialtyId = null) => _repository.GetAvailableAsync(doctorId, specialtyId);
}
