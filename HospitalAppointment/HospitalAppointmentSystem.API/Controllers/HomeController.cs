using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.API.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Error() => View();
}
