using HospitalAppointmentSystem.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.API.Controllers.Api;

[ApiController]
[Route("api/schedules")]
public class SchedulesApiController : ControllerBase
{
    private readonly IScheduleService _scheduleService;
    public SchedulesApiController(IScheduleService scheduleService) => _scheduleService = scheduleService;

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable([FromQuery] int? doctorId, [FromQuery] int? specialtyId)
        => Ok(await _scheduleService.GetAvailableAsync(doctorId, specialtyId));

    [HttpGet("doctor/{doctorId:int}")]
    public async Task<IActionResult> GetByDoctor(int doctorId)
        => Ok(await _scheduleService.GetAvailableAsync(doctorId));
}
