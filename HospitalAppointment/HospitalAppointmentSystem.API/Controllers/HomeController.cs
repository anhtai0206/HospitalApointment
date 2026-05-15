using System.Security.Claims;
using HospitalAppointmentSystem.BLL.Services;
using HospitalAppointmentSystem.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.API.Controllers;

public class HomeController : Controller
{
    private readonly IDoctorService _doctorService;
    private readonly ISpecialtyService _specialtyService;
    private readonly IScheduleService _scheduleService;
    private readonly ILookupService _lookupService;
    private readonly IAuthService _authService;
    private readonly IAppointmentService _appointmentService;

    public HomeController(
        IDoctorService doctorService,
        ISpecialtyService specialtyService,
        IScheduleService scheduleService,
        ILookupService lookupService,
        IAuthService authService,
        IAppointmentService appointmentService)
    {
        _doctorService = doctorService;
        _specialtyService = specialtyService;
        _scheduleService = scheduleService;
        _lookupService = lookupService;
        _authService = authService;
        _appointmentService = appointmentService;
    }

    public async Task<IActionResult> Index(string? doctorName, int? specialtyId)
    {
        var doctors = await _doctorService.GetAllAsync(specialtyId);
        if (!string.IsNullOrWhiteSpace(doctorName))
        {
            doctors = doctors
                .Where(d => d.FullName.Contains(doctorName.Trim(), StringComparison.OrdinalIgnoreCase))
                .OrderBy(d => d.DoctorId)
                .ToList();
        }

        ViewBag.DoctorName = doctorName;
        ViewBag.SelectedSpecialtyId = specialtyId;
        ViewBag.Specialties = await _specialtyService.GetAllAsync();
        ViewBag.AllDoctors = await _doctorService.GetAllAsync();
        ViewBag.Doctors = doctors;
        ViewBag.Schedules = await _scheduleService.GetAvailableAsync();
        ViewBag.Services = await _lookupService.GetMedicalServicesAsync();
        ViewBag.Rooms = await _lookupService.GetClinicRoomsAsync();
        ViewBag.Today = DateTime.Today.ToString("yyyy-MM-dd");

        if (User.Identity?.IsAuthenticated == true && User.IsInRole("Patient"))
        {
            ViewBag.PatientProfile = await _authService.GetPatientProfileAsync(GetPatientId());
        }

        return View(new AppointmentDTO());
    }

    [Authorize(Roles = "Patient")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(AppointmentDTO dto)
    {
        dto.PatientId = GetPatientId();
        var result = await _appointmentService.BookAppointmentAsync(dto);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error() => View();

    private int GetPatientId()
    {
        var value = User.FindFirstValue("PatientId");
        if (string.IsNullOrWhiteSpace(value)) throw new UnauthorizedAccessException("Tài khoản chưa có hồ sơ bệnh nhân");
        return int.Parse(value);
    }
}
