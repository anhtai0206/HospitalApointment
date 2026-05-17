using System.Security.Claims;
using HospitalAppointmentSystem.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.API.Controllers;

public class DoctorController : Controller
{
    private readonly IDoctorService _doctorService;
    private readonly ISpecialtyService _specialtyService;
    private readonly IAppointmentService _appointmentService;

    public DoctorController(IDoctorService doctorService, ISpecialtyService specialtyService, IAppointmentService appointmentService)
    {
        _doctorService = doctorService;
        _specialtyService = specialtyService;
        _appointmentService = appointmentService;
    }

    public IActionResult Index(int? specialtyId)
    {
        return Redirect($"/#doctorSection");
    }

    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> MySchedule()
    {
        var doctorIdClaim = User.FindFirstValue("DoctorId");
        if (string.IsNullOrWhiteSpace(doctorIdClaim)) return RedirectToAction("AccessDenied", "Account");
        var appointments = await _appointmentService.GetByDoctorAsync(int.Parse(doctorIdClaim));
        return View(appointments);
    }


    [Authorize(Roles = "Doctor")]
    [HttpPost]
    public async Task<IActionResult> Confirm(int id)
    {
        var doctorIdClaim = User.FindFirstValue("DoctorId");
        if (string.IsNullOrWhiteSpace(doctorIdClaim)) return RedirectToAction("AccessDenied", "Account");

        var appointments = await _appointmentService.GetByDoctorAsync(int.Parse(doctorIdClaim));
        if (!appointments.Any(a => a.AppointmentId == id))
        {
            TempData["Error"] = "Bạn không có quyền xác nhận lịch hẹn này";
            return RedirectToAction(nameof(MySchedule));
        }

        var result = await _appointmentService.ConfirmAsync(id);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(MySchedule));
    }

    [Authorize(Roles = "Doctor")]
    [HttpPost]
    public async Task<IActionResult> Complete(int id)
    {
        var doctorIdClaim = User.FindFirstValue("DoctorId");
        if (string.IsNullOrWhiteSpace(doctorIdClaim)) return RedirectToAction("AccessDenied", "Account");

        var appointments = await _appointmentService.GetByDoctorAsync(int.Parse(doctorIdClaim));
        if (!appointments.Any(a => a.AppointmentId == id))
        {
            TempData["Error"] = "Bạn không có quyền cập nhật lịch hẹn này";
            return RedirectToAction(nameof(MySchedule));
        }

        var result = await _appointmentService.CompleteAsync(id);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(MySchedule));
    }

    [Authorize(Roles = "Doctor")]
    [HttpPost]
    public async Task<IActionResult> Cancel(int id, string? cancelReason)
    {
        var doctorIdClaim = User.FindFirstValue("DoctorId");
        if (string.IsNullOrWhiteSpace(doctorIdClaim)) return RedirectToAction("AccessDenied", "Account");

        var appointments = await _appointmentService.GetByDoctorAsync(int.Parse(doctorIdClaim));
        if (!appointments.Any(a => a.AppointmentId == id))
        {
            TempData["Error"] = "Bạn không có quyền hủy lịch hẹn này";
            return RedirectToAction(nameof(MySchedule));
        }

        var result = await _appointmentService.CancelAsync(id, cancelReason);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(MySchedule));
    }

}
