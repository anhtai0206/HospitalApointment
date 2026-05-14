using System.Security.Claims;
using HospitalAppointmentSystem.BLL.Services;
using HospitalAppointmentSystem.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.API.Controllers;

[Authorize]
public class AppointmentController : Controller
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly ISpecialtyService _specialtyService;
    private readonly IScheduleService _scheduleService;
    private readonly ILookupService _lookupService;

    public AppointmentController(IAppointmentService appointmentService, IDoctorService doctorService, ISpecialtyService specialtyService, IScheduleService scheduleService, ILookupService lookupService)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _specialtyService = specialtyService;
        _scheduleService = scheduleService;
        _lookupService = lookupService;
    }

    [Authorize(Roles = "Patient")]
    [HttpGet]
    public async Task<IActionResult> Create(int? specialtyId, int? doctorId)
    {
        ViewBag.SelectedSpecialtyId = specialtyId;
        ViewBag.SelectedDoctorId = doctorId;
        ViewBag.Specialties = await _specialtyService.GetAllAsync();
        ViewBag.Doctors = await _doctorService.GetAllAsync(specialtyId);
        ViewBag.Schedules = await _scheduleService.GetAvailableAsync(doctorId, specialtyId);
        ViewBag.Services = await _lookupService.GetMedicalServicesAsync();
        ViewBag.Rooms = await _lookupService.GetClinicRoomsAsync();
        return View(new AppointmentDTO { PatientId = GetPatientId() });
    }

    [Authorize(Roles = "Patient")]
    [HttpPost]
    public async Task<IActionResult> Create(AppointmentDTO dto)
    {
        dto.PatientId = GetPatientId();
        var result = await _appointmentService.BookAppointmentAsync(dto);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Create));
    }

    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> MyAppointments()
    {
        var appointments = await _appointmentService.GetByPatientAsync(GetPatientId());
        return View(appointments);
    }

    [Authorize(Roles = "Patient")]
    [HttpPost]
    public async Task<IActionResult> CancelMine(int id)
    {
        var appointments = await _appointmentService.GetByPatientAsync(GetPatientId());
        if (!appointments.Any(a => a.AppointmentId == id))
        {
            TempData["Error"] = "Bạn không có quyền hủy lịch hẹn này";
            return RedirectToAction(nameof(MyAppointments));
        }

        var result = await _appointmentService.CancelAsync(id);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(MyAppointments));
    }

    private int GetPatientId()
    {
        var value = User.FindFirstValue("PatientId");
        if (string.IsNullOrWhiteSpace(value)) throw new UnauthorizedAccessException("Tài khoản chưa có hồ sơ bệnh nhân");
        return int.Parse(value);
    }
}
