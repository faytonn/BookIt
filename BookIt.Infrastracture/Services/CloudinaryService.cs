using BookIt.Application.Interfaces.Services.External;
using BookIt.Domain.AppSettingModels;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace BookIt.Infrastracture.External;

public class CloudinaryService : ICloudinaryService
{
    private readonly IConfiguration _configuration;
    private readonly CloudinaryOptions _cloudinaryOptions;
    private readonly Cloudinary _cloudinary = null!;

    public CloudinaryService(IConfiguration configuration)
    {
        _configuration = configuration;
        _cloudinaryOptions = _configuration.GetSection("CloudinarySettings").Get<CloudinaryOptions>() ?? new();

        var myAccount = new Account
        {
            ApiKey = _cloudinaryOptions.APIKey,
            Cloud = _cloudinaryOptions.CloudName,
            ApiSecret = _cloudinaryOptions.APISecret
        };

        _cloudinary = new Cloudinary(myAccount);

        _cloudinary.Api.Secure = true;
    }

    public async Task<string> FileCreateAsync(IFormFile file)
    {
        if (file.Length == 0)
            throw new ArgumentException("The uploaded file is empty.", nameof(file));

        string fileName = string.Concat(Guid.NewGuid(), Path.GetExtension(file.FileName));

        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            AssetFolder = "bookit."
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null || uploadResult.SecureUrl == null)
            throw new InvalidOperationException($"Image upload failed: {uploadResult.Error?.Message ?? "no URL returned"}");

        return uploadResult.SecureUrl.ToString();
    }

    public async Task<bool> FileDeleteAsync(string filePath)
    {
        try
        {
            string publicIdWithExtension = filePath.Substring(filePath.LastIndexOf("bookit."));
            string publicId = publicIdWithExtension.Substring(0, publicIdWithExtension.LastIndexOf('.'));

            var deleteParams = new DelResParams()
            {
                PublicIds = new List<string> { publicId },
                Type = "upload",
                ResourceType = CloudinaryDotNet.Actions.ResourceType.Image
            };
            var result = await _cloudinary.DeleteResourcesAsync(deleteParams);

            return result.StatusCode == HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

    }
}
