using HospitalAppointmentSystem.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.API.Controllers;

public class DoctorController : Controller
{
    private readonly IDoctorService _doctorService;
    private readonly ISpecialtyService _specialtyService;

    public DoctorController(IDoctorService doctorService, ISpecialtyService specialtyService)
    {
        _doctorService = doctorService;
        _specialtyService = specialtyService;
    }

    public async Task<IActionResult> Index(int? specialtyId)
    {
        ViewBag.Specialties = await _specialtyService.GetAllAsync();
        ViewBag.SelectedSpecialtyId = specialtyId;
        var doctors = await _doctorService.GetAllAsync(specialtyId);
        return View(doctors);
    }
}
