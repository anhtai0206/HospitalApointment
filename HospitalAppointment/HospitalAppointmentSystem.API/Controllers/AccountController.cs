using System.Security.Claims;
using HospitalAppointmentSystem.API.Services;
using HospitalAppointmentSystem.BLL.Services;
using HospitalAppointmentSystem.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.API.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly ICloudinaryPhotoService _photoService;

    public AccountController(IAuthService authService, ICloudinaryPhotoService photoService)
    {
        _authService = authService;
        _photoService = photoService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginDTO());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO dto, string? returnUrl = null)
    {
        var user = await _authService.LoginAsync(dto);
        if (user == null)
        {
            TempData["Error"] = "Email hoặc mật khẩu không đúng";
            return View(dto);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role),
            new("UserId", user.UserId.ToString())
        };
        if (user.PatientId.HasValue) claims.Add(new Claim("PatientId", user.PatientId.Value.ToString()));
        if (user.DoctorId.HasValue) claims.Add(new Claim("DoctorId", user.DoctorId.Value.ToString()));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = dto.RememberMe });

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return user.Role switch
        {
            "Admin" => RedirectToAction("Appointments", "Admin"),
            "Doctor" => RedirectToAction("MySchedule", "Doctor"),
            _ => RedirectToAction("Create", "Appointment")
        };
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterPatientDTO());

    [HttpPost]
    public async Task<IActionResult> Register(RegisterPatientDTO dto)
    {
        var result = await _authService.RegisterPatientAsync(dto);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        if (!result.Success) return View(dto);
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult ForgotPassword() => View(new ForgotPasswordDTO());

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO dto)
    {
        var result = await _authService.ResetPasswordAsync(dto);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        if (!result.Success) return View(dto);
        return RedirectToAction(nameof(Login));
    }

    [Authorize(Roles = "Patient")]
    [HttpGet]
    public async Task<IActionResult> PatientInfo()
    {
        var profile = await _authService.GetPatientProfileAsync(GetPatientId());
        if (profile == null) return RedirectToAction(nameof(AccessDenied));
        return View(profile);
    }

    [Authorize(Roles = "Patient")]
    [HttpPost]
    public async Task<IActionResult> PatientInfo(PatientProfileDTO dto, IFormFile? photoFile)
    {
        var patientId = GetPatientId();

        if (photoFile != null && photoFile.Length > 0)
        {
            var uploadResult = await _photoService.UploadPatientPhotoAsync(photoFile, patientId);
            if (!uploadResult.Success)
            {
                TempData["Error"] = uploadResult.Message;
                return View(dto);
            }

            dto.PhotoUrl = uploadResult.Data ?? string.Empty;
        }

        var result = await _authService.UpdatePatientProfileAsync(patientId, dto);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        if (!result.Success) return View(dto);
        return RedirectToAction(nameof(PatientInfo));
    }

    [Authorize]
    [HttpGet]
    public IActionResult ChangePassword() => View(new ChangePasswordDTO());

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);
        var result = await _authService.ChangePasswordAsync(userId, dto);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        if (!result.Success) return View(dto);
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied() => View();

    private int GetPatientId()
    {
        var value = User.FindFirstValue("PatientId");
        if (string.IsNullOrWhiteSpace(value)) throw new UnauthorizedAccessException("Tài khoản chưa có hồ sơ bệnh nhân");
        return int.Parse(value);
    }
}
