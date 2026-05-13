using HospitalAppointmentSystem.DAL.Data;
using HospitalAppointmentSystem.DTO;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.DAL.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly HospitalDbContext _db;
    public ScheduleRepository(HospitalDbContext db) => _db = db;

    public async Task<List<ScheduleDTO>> GetAvailableAsync(int? doctorId = null)
    {
        var today = DateTime.Today;
        var query = _db.DoctorSchedules
            .Include(s => s.Doctor)!.ThenInclude(d => d.User)
            .Include(s => s.Doctor)!.ThenInclude(d => d.Specialty)
            .Where(s => s.Status == "Available" && s.WorkDate >= today && s.CurrentPatients < s.MaxPatients);

        if (doctorId.HasValue)
            query = query.Where(s => s.DoctorId == doctorId.Value);

        return await query
            .OrderBy(s => s.WorkDate)
            .ThenBy(s => s.StartTime)
            .Select(s => new ScheduleDTO
            {
                ScheduleId = s.ScheduleId,
                DoctorId = s.DoctorId,
                DoctorName = s.Doctor!.User!.FullName,
                SpecialtyName = s.Doctor.Specialty!.SpecialtyName,
                WorkDate = s.WorkDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                MaxPatients = s.MaxPatients,
                CurrentPatients = s.CurrentPatients,
                AvailableSlots = s.MaxPatients - s.CurrentPatients,
                Status = s.Status
            })
            .ToListAsync();
    }

    public async Task<ScheduleDTO?> GetByIdAsync(int id)
    {
        return await _db.DoctorSchedules
            .Include(s => s.Doctor)!.ThenInclude(d => d.User)
            .Include(s => s.Doctor)!.ThenInclude(d => d.Specialty)
            .Where(s => s.ScheduleId == id)
            .Select(s => new ScheduleDTO
            {
                ScheduleId = s.ScheduleId,
                DoctorId = s.DoctorId,
                DoctorName = s.Doctor!.User!.FullName,
                SpecialtyName = s.Doctor.Specialty!.SpecialtyName,
                WorkDate = s.WorkDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                MaxPatients = s.MaxPatients,
                CurrentPatients = s.CurrentPatients,
                AvailableSlots = s.MaxPatients - s.CurrentPatients,
                Status = s.Status
            })
            .FirstOrDefaultAsync();
    }
}
