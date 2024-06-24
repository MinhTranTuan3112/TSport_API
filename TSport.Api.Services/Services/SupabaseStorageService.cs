using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Supabase;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Services.Services
{
    public class SupabaseStorageService : ISupabaseStorageService
    {
        private readonly Client _client;
        private readonly IConfiguration _configuration;

        public SupabaseStorageService(Client client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async Task DeleteImageAsync(string bucketName, string imageUrl)
        {
            await _client.Storage.From(bucketName).Remove(new List<string> { imageUrl });
        }

        public string GetImageUrl(string bucketName, string imageName)
        {
            string url = _configuration["Supabase:Url"]!;
            return $"{url}/storage/v1/object/public/{bucketName}/{imageName}";
        }

        public async Task<string> UpdateImageAsync(IFormFile imageFile, string bucketName, string imageName)
        {
            using var memoryStream = new MemoryStream();

            await imageFile.CopyToAsync(memoryStream);

            await _client.Storage.From(bucketName).Update(memoryStream.ToArray(), imageName ?? imageFile.FileName);

            return GetImageUrl(bucketName, imageName ?? imageFile.FileName);
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string bucketName, string? imageName = default)
        {
            using var memoryStream = new MemoryStream();

            await imageFile.CopyToAsync(memoryStream);

            await _client.Storage.From(bucketName).Upload(memoryStream.ToArray(), imageName ?? imageFile.FileName);

            return GetImageUrl(bucketName, imageName ?? imageFile.FileName);
        }

        public async Task<string[]> UploadImagesAsync(IFormFileCollection files, string bucketName)
        {
            var uploadTasks = new List<Task<string>>();

            foreach (var file in files)
            {
                uploadTasks.Add(UploadImageAsync(file, bucketName));
            }

            var imageUrls = await Task.WhenAll(uploadTasks);

            return imageUrls;
        }
    }
}