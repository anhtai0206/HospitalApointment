using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HospitalAppointmentSystem.DTO;
using Microsoft.Extensions.Options;

namespace HospitalAppointmentSystem.API.Services;

public class CloudinarySettings
{
    public string CloudName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
    public string Folder { get; set; } = "hospital-patient-photos";
}

public interface ICloudinaryPhotoService
{
    Task<ApiResponse<string>> UploadPatientPhotoAsync(IFormFile file, int patientId);
}

public class CloudinaryPhotoService : ICloudinaryPhotoService
{
    private readonly CloudinarySettings _settings;

    public CloudinaryPhotoService(IOptions<CloudinarySettings> options)
    {
        _settings = options.Value;
    }

    public async Task<ApiResponse<string>> UploadPatientPhotoAsync(IFormFile file, int patientId)
    {
        if (file == null || file.Length == 0)
            return ApiResponse<string>.Fail("Vui lòng chọn ảnh bệnh nhân");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType.ToLower()))
            return ApiResponse<string>.Fail("Chỉ cho phép tải ảnh JPG, PNG hoặc WEBP");

        if (file.Length > 2 * 1024 * 1024)
            return ApiResponse<string>.Fail("Ảnh không được vượt quá 2MB");

        if (string.IsNullOrWhiteSpace(_settings.CloudName) ||
            _settings.CloudName == "YOUR_CLOUD_NAME" ||
            string.IsNullOrWhiteSpace(_settings.ApiKey) ||
            string.IsNullOrWhiteSpace(_settings.ApiSecret))
        {
            return ApiResponse<string>.Fail("Chưa cấu hình Cloudinary trong appsettings.json");
        }

        var account = new Account(_settings.CloudName, _settings.ApiKey, _settings.ApiSecret);
        var cloudinary = new Cloudinary(account);

        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = _settings.Folder,
            PublicId = $"patient_{patientId}_{DateTime.Now:yyyyMMddHHmmss}",
            Overwrite = true,
            Transformation = new Transformation()
                .Width(360)
                .Height(480)
                .Crop("fill")
                .Gravity("face")
                .Quality("auto")
                .FetchFormat("auto")
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);
        if (uploadResult.Error != null)
            return ApiResponse<string>.Fail(uploadResult.Error.Message);

        return ApiResponse<string>.Ok(uploadResult.SecureUrl?.ToString() ?? uploadResult.Url.ToString(), "Tải ảnh bệnh nhân thành công");
    }
}
