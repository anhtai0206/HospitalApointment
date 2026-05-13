using HospitalAppointmentSystem.DAL.Data;
using HospitalAppointmentSystem.DAL.Entities;
using HospitalAppointmentSystem.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HospitalAppointmentSystem.DAL.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly HospitalDbContext _db;
    private readonly string _connectionString;

    public AppointmentRepository(HospitalDbContext db, IConfiguration configuration)
    {
        _db = db;
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public async Task<List<AppointmentDetailDTO>> GetAllAsync()
    {
        return await GetAppointmentQuery().OrderByDescending(a => a.CreatedAt).ToListAsync();
    }

    public async Task<List<AppointmentDetailDTO>> GetByPatientAsync(int patientId)
    {
        return await GetAppointmentQuery(patientId).OrderByDescending(a => a.CreatedAt).ToListAsync();
    }

    private IQueryable<AppointmentDetailDTO> GetAppointmentQuery(int? patientId = null)
    {
        var query = _db.Appointments
            .Include(a => a.Patient)!.ThenInclude(p => p.User)
            .Include(a => a.Doctor)!.ThenInclude(d => d.User)
            .Include(a => a.Doctor)!.ThenInclude(d => d.Specialty)
            .Include(a => a.DoctorSchedule)
            .AsQueryable();

        if (patientId.HasValue)
            query = query.Where(a => a.PatientId == patientId.Value);

        return query.Select(a => new AppointmentDetailDTO
        {
            AppointmentId = a.AppointmentId,
            PatientName = a.Patient!.User!.FullName,
            DoctorName = a.Doctor!.User!.FullName,
            SpecialtyName = a.Doctor.Specialty!.SpecialtyName,
            WorkDate = a.DoctorSchedule!.WorkDate,
            StartTime = a.DoctorSchedule.StartTime,
            EndTime = a.DoctorSchedule.EndTime,
            Reason = a.Reason,
            Status = a.Status,
            CreatedAt = a.CreatedAt
        });
    }

    // ADO.NET gọi Stored Procedure có Transaction ở SQL Server
    public async Task<ApiResponse> BookAppointmentAsync(AppointmentDTO dto)
    {
        try
        {
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand("sp_BookAppointment", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PatientId", dto.PatientId);
            cmd.Parameters.AddWithValue("@DoctorId", dto.DoctorId);
            cmd.Parameters.AddWithValue("@ScheduleId", dto.ScheduleId);
            cmd.Parameters.AddWithValue("@Reason", dto.Reason);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return ApiResponse.Ok("Đăng ký lịch khám thành công");
        }
        catch (SqlException ex)
        {
            return ApiResponse.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            return ApiResponse.Fail("Lỗi hệ thống: " + ex.Message);
        }
    }

    public async Task<ApiResponse> CancelAsync(int appointmentId)
    {
        var appointment = await _db.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        if (appointment == null) return ApiResponse.Fail("Không tìm thấy lịch hẹn");
        if (appointment.Status == "Cancelled") return ApiResponse.Fail("Lịch hẹn đã bị hủy trước đó");

        appointment.Status = "Cancelled";
        var schedule = await _db.DoctorSchedules.FirstOrDefaultAsync(s => s.ScheduleId == appointment.ScheduleId);
        if (schedule != null && schedule.CurrentPatients > 0)
        {
            schedule.CurrentPatients--;
            if (schedule.CurrentPatients < schedule.MaxPatients) schedule.Status = "Available";
        }

        _db.AppointmentLogs.Add(new AppointmentLog
        {
            AppointmentId = appointmentId,
            ActionName = "Cancelled",
            ActionDate = DateTime.Now
        });

        await _db.SaveChangesAsync();
        return ApiResponse.Ok("Hủy lịch khám thành công");
    }

    public async Task<ApiResponse> ConfirmAsync(int appointmentId)
    {
        var appointment = await _db.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        if (appointment == null) return ApiResponse.Fail("Không tìm thấy lịch hẹn");

        appointment.Status = "Confirmed";
        _db.AppointmentLogs.Add(new AppointmentLog
        {
            AppointmentId = appointmentId,
            ActionName = "Confirmed",
            ActionDate = DateTime.Now
        });

        await _db.SaveChangesAsync();
        return ApiResponse.Ok("Xác nhận lịch khám thành công");
    }

    // ADO.NET phi kết nối: DataSet + DataAdapter
    public Task<DataSet> GetAppointmentsDisconnectedAsync()
    {
        using var conn = new SqlConnection(_connectionString);
        using var adapter = new SqlDataAdapter("SELECT * FROM vw_AppointmentDetails", conn);
        var ds = new DataSet();
        adapter.Fill(ds, "AppointmentDetails");
        return Task.FromResult(ds);
    }
}
