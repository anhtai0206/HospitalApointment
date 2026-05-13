using HospitalAppointmentSystem.BLL.Services;
using HospitalAppointmentSystem.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.API.Controllers;

public class AppointmentController : Controller
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly ISpecialtyService _specialtyService;
    private readonly IScheduleService _scheduleService;

    public AppointmentController(IAppointmentService appointmentService, IDoctorService doctorService, ISpecialtyService specialtyService, IScheduleService scheduleService)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _specialtyService = specialtyService;
        _scheduleService = scheduleService;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? doctorId)
    {
        ViewBag.Doctors = await _doctorService.GetAllAsync();
        ViewBag.Specialties = await _specialtyService.GetAllAsync();
        ViewBag.Schedules = await _scheduleService.GetAvailableAsync(doctorId);
        return View(new AppointmentDTO { PatientId = 1 });
    }

    [HttpPost]
    public async Task<IActionResult> Create(AppointmentDTO dto)
    {
        var result = await _appointmentService.BookAppointmentAsync(dto);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Create));
    }

    public async Task<IActionResult> MyAppointments()
    {
        var appointments = await _appointmentService.GetByPatientAsync(1);
        return View(appointments);
    }
}
