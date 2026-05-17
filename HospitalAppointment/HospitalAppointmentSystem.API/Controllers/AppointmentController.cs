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
    private readonly IAuthService _authService;

    public AppointmentController(IAppointmentService appointmentService, IDoctorService doctorService, ISpecialtyService specialtyService, IScheduleService scheduleService, ILookupService lookupService, IAuthService authService)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _specialtyService = specialtyService;
        _scheduleService = scheduleService;
        _lookupService = lookupService;
        _authService = authService;
    }

    [Authorize(Roles = "Patient")]
    [HttpGet]
    public IActionResult Create()
    {
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "Patient")]
    [HttpPost]
    public async Task<IActionResult> Create(AppointmentDTO dto)
    {
        dto.PatientId = GetPatientId();
        var result = await _appointmentService.BookAppointmentAsync(dto);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> MyAppointments()
    {
        var appointments = await _appointmentService.GetByPatientAsync(GetPatientId());
        return View(appointments);
    }

    [Authorize(Roles = "Patient")]
    [HttpPost]
    public async Task<IActionResult> CancelMine(int id, string? cancelReason)
    {
        var appointments = await _appointmentService.GetByPatientAsync(GetPatientId());
        if (!appointments.Any(a => a.AppointmentId == id))
        {
            TempData["Error"] = "Bạn không có quyền hủy lịch hẹn này";
            return RedirectToAction(nameof(MyAppointments));
        }

        var result = await _appointmentService.CancelAsync(id, cancelReason);
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
