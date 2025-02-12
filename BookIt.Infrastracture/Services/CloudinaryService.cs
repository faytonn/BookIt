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
        string fileName = string.Concat(Guid.NewGuid(), file.FileName.Substring(file.FileName.LastIndexOf('.')));

        var uploadResult = new ImageUploadResult();
        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                AssetFolder = "bookit."
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        string url = uploadResult.SecureUrl.ToString();

        return url;
    }

    public async Task<bool> FileDeleteAsync(string filePath)
    {
        try
        {
            string publicIdWithExtension = filePath.Substring(filePath.LastIndexOf("limakaz"));
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
