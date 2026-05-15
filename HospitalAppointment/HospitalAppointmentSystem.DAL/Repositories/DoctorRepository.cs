using HospitalAppointmentSystem.DAL.Data;
using HospitalAppointmentSystem.DTO;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.DAL.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly HospitalDbContext _db;
    public DoctorRepository(HospitalDbContext db) => _db = db;

    public async Task<List<DoctorDTO>> GetAllAsync(int? specialtyId = null)
    {
        var query = _db.Doctors
            .Include(d => d.User)
            .Include(d => d.Specialty)
            .AsQueryable();

        if (specialtyId.HasValue)
            query = query.Where(d => d.SpecialtyId == specialtyId.Value);

        return await query
            .OrderBy(d => d.DoctorId)
            .Select(d => new DoctorDTO
            {
                DoctorId = d.DoctorId,
                FullName = d.User!.FullName,
                Email = d.User.Email,
                Phone = d.User.Phone,
                SpecialtyId = d.SpecialtyId,
                SpecialtyName = d.Specialty!.SpecialtyName,
                Qualification = d.Qualification,
                ExperienceYears = d.ExperienceYears,
                Description = d.Description
            })
            .ToListAsync();
    }

    public async Task<DoctorDTO?> GetByIdAsync(int id)
    {
        return await _db.Doctors
            .Include(d => d.User)
            .Include(d => d.Specialty)
            .Where(d => d.DoctorId == id)
            .Select(d => new DoctorDTO
            {
                DoctorId = d.DoctorId,
                FullName = d.User!.FullName,
                Email = d.User.Email,
                Phone = d.User.Phone,
                SpecialtyId = d.SpecialtyId,
                SpecialtyName = d.Specialty!.SpecialtyName,
                Qualification = d.Qualification,
                ExperienceYears = d.ExperienceYears,
                Description = d.Description
            })
            .FirstOrDefaultAsync();
    }
}
