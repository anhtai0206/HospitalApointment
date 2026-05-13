using HospitalAppointmentSystem.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.API.Controllers.Api;

[ApiController]
[Route("api/doctors")]
public class DoctorsApiController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    public DoctorsApiController(IDoctorService doctorService) => _doctorService = doctorService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? specialtyId)
        => Ok(await _doctorService.GetAllAsync(specialtyId));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        return doctor == null ? NotFound() : Ok(doctor);
    }
}
