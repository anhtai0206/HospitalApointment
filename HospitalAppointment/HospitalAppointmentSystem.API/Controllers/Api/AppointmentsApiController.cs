using HospitalAppointmentSystem.BLL.Services;
using HospitalAppointmentSystem.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.API.Controllers.Api;

[ApiController]
[Route("api/appointments")]
public class AppointmentsApiController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    public AppointmentsApiController(IAppointmentService appointmentService) => _appointmentService = appointmentService;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _appointmentService.GetAllAsync());

    [HttpGet("patient/{patientId:int}")]
    public async Task<IActionResult> GetByPatient(int patientId) => Ok(await _appointmentService.GetByPatientAsync(patientId));

    [HttpPost]
    public async Task<IActionResult> Book([FromBody] AppointmentDTO dto)
    {
        var result = await _appointmentService.BookAppointmentAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}/confirm")]
    public async Task<IActionResult> Confirm(int id)
    {
        var result = await _appointmentService.ConfirmAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _appointmentService.CancelAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
