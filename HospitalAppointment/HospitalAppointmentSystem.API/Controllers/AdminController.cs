using HospitalAppointmentSystem.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.API.Controllers;

public class AdminController : Controller
{
    private readonly IAppointmentService _appointmentService;

    public AdminController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    public async Task<IActionResult> Appointments()
    {
        var appointments = await _appointmentService.GetAllAsync();
        return View(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> Confirm(int id)
    {
        var result = await _appointmentService.ConfirmAsync(id);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Appointments));
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _appointmentService.CancelAsync(id);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Appointments));
    }
}
