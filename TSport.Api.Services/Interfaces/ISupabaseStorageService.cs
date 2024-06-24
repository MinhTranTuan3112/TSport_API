using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TSport.Api.Services.Interfaces
{
    public interface ISupabaseStorageService
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string bucketName, string? imageName = default);

        string GetImageUrl(string bucketName, string imageUrl);

        Task<string> UpdateImageAsync(IFormFile imageFile, string bucketName, string imageName);

        Task DeleteImageAsync(string bucketName, string imageName);

        Task<string[]> UploadImagesAsync(IFormFileCollection files, string bucketName);
    }
}