using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace joro.too.Services.Services;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary; 
    public CloudinaryService(IConfiguration config) 
    { 
        var account = new Account( 
            config["Cloudinary:CloudName"], 
            config["Cloudinary:ApiKey"], 
            config["Cloudinary:ApiSecret"]); 
        _cloudinary = new Cloudinary(account); 
    } 
    public async Task<string> UploadImageAsync(IFormFile file) 
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }
        using var stream = file.OpenReadStream(); 
        var uploadParams = new ImageUploadParams 
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Width(324).Height(480).Crop("fill").Gravity("face")
        }; 
        var uploadResult = await _cloudinary.UploadAsync(uploadParams); 
        if (uploadResult == null || uploadResult.SecureUrl == null) 
        {
            return null; 
        } 
        return uploadResult.SecureUrl.ToString();
    } 
    public async Task<string> UploadPfpAsync(IFormFile file) 
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }
            
        using var stream = file.OpenReadStream(); 
        var uploadParams = new ImageUploadParams 
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Width(100).Height(100).Crop("fill").Gravity("face")
        }; 
        var uploadResult = await _cloudinary.UploadAsync(uploadParams); 
        if (uploadResult == null || uploadResult.SecureUrl == null) 
        {
            return null; 
        } 
        return uploadResult.SecureUrl.ToString();
    } 
    public async Task<string> UploadVideoAsync(IFormFile file) 
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }
            
        using var stream = file.OpenReadStream(); 
        var uploadParams = new VideoUploadParams() 
        {
            File = new FileDescription(file.FileName, stream)
        }; 
        var uploadResult = await _cloudinary.UploadAsync(uploadParams); 
        if (uploadResult == null || uploadResult.SecureUrl == null) 
        {
            return null; 
        } 
        return uploadResult.SecureUrl.ToString();
    }

    public async Task<bool> DeleteVideoAsync(string vidsrc)
    {
        var deleteParams = new DelResParams(){
            PublicIds = new List<string>{vidsrc.Substring(62, 20)},
            Type = "upload",
            ResourceType = ResourceType.Video};
        var result = _cloudinary.DeleteResources(deleteParams);
        return result.DeletedCounts.Count>0;
    }
    public async Task<bool> DeleteImageAsync(string imgsrc)
    {
        var deleteParams = new DelResParams(){
            PublicIds = new List<string>{imgsrc.Substring(62, 20)},
            Type = "upload",
            ResourceType = ResourceType.Image};
        var result = _cloudinary.DeleteResources(deleteParams);
        return result.DeletedCounts.Count>0;
    }
}