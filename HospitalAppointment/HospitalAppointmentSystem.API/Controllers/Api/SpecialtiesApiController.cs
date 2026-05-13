using HospitalAppointmentSystem.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.API.Controllers.Api;

[ApiController]
[Route("api/specialties")]
public class SpecialtiesApiController : ControllerBase
{
    private readonly ISpecialtyService _specialtyService;
    public SpecialtiesApiController(ISpecialtyService specialtyService) => _specialtyService = specialtyService;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _specialtyService.GetAllAsync());
}
