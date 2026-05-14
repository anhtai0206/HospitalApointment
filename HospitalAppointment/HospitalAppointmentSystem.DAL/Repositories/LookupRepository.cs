using HospitalAppointmentSystem.DAL.Data;
using HospitalAppointmentSystem.DTO;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.DAL.Repositories;

public class LookupRepository : ILookupRepository
{
    private readonly HospitalDbContext _db;
    public LookupRepository(HospitalDbContext db) => _db = db;

    public async Task<List<MedicalServiceDTO>> GetMedicalServicesAsync()
    {
        return await _db.MedicalServices
            .OrderBy(s => s.MedicalServiceId)
            .Select(s => new MedicalServiceDTO
            {
                MedicalServiceId = s.MedicalServiceId,
                ServiceName = s.ServiceName,
                Price = s.Price,
                Description = s.Description
            })
            .ToListAsync();
    }

    public async Task<List<ClinicRoomDTO>> GetClinicRoomsAsync(int? specialtyId = null)
    {
        var query = _db.ClinicRooms
            .Include(r => r.Specialty)
            .Where(r => r.Status == "Active")
            .AsQueryable();

        if (specialtyId.HasValue)
            query = query.Where(r => r.SpecialtyId == specialtyId.Value);

        return await query
            .OrderBy(r => r.SpecialtyId)
            .ThenBy(r => r.RoomName)
            .Select(r => new ClinicRoomDTO
            {
                ClinicRoomId = r.ClinicRoomId,
                SpecialtyId = r.SpecialtyId,
                SpecialtyName = r.Specialty!.SpecialtyName,
                RoomName = r.RoomName,
                Status = r.Status
            })
            .ToListAsync();
    }
}
