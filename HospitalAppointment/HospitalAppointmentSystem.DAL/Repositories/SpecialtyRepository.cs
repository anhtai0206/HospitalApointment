using HospitalAppointmentSystem.DAL.Data;
using HospitalAppointmentSystem.DTO;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.DAL.Repositories;

public class SpecialtyRepository : ISpecialtyRepository
{
    private readonly HospitalDbContext _db;
    public SpecialtyRepository(HospitalDbContext db) => _db = db;

    public async Task<List<SpecialtyDTO>> GetAllAsync()
    {
        return await _db.Specialties
            .OrderBy(s => s.SpecialtyName)
            .Select(s => new SpecialtyDTO
            {
                SpecialtyId = s.SpecialtyId,
                SpecialtyName = s.SpecialtyName,
                Description = s.Description
            })
            .ToListAsync();
    }
}
