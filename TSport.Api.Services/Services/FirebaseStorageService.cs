using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Services.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly IConfiguration _configuration;
        private readonly string _bucketName;

        public FirebaseStorageService(StorageClient storageClient, IConfiguration configuration)
        {
            _storageClient = storageClient;
            _configuration = configuration;
            _bucketName = _configuration["Firebase:Bucket"]!;
        }

        public async Task DeleteImageAsync(string imageName)
        {
            await _storageClient.DeleteObjectAsync(_bucketName, imageName, cancellationToken: CancellationToken.None);
        }

        public string GetImageUrl(string imageName)
        {

            string imageUrl = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(imageName)}?alt=media";
            return imageUrl;
        }

        public async Task<string> UpdateImageAsync(IFormFile imageFile, string imageName)
        {

            using var stream = new MemoryStream();

            await imageFile.CopyToAsync(stream);

            stream.Position = 0;

            // Re-upload the image with the same name to update it
            var blob = await _storageClient.UploadObjectAsync(_bucketName, imageName, imageFile.ContentType, stream, cancellationToken: CancellationToken.None);

            return GetImageUrl(imageName);
        }


        public async Task<string> UploadImageAsync(IFormFile imageFile, string? imageName = default)
        {
            imageName ??= imageFile.FileName;

            using var stream = new MemoryStream();

            await imageFile.CopyToAsync(stream);

            var blob = await _storageClient.UploadObjectAsync(_bucketName, imageName, imageFile.ContentType, stream, cancellationToken: CancellationToken.None);

            if (blob is null)
            {
                throw new BadRequestException("Upload failed");
            }

            return GetImageUrl(imageFile.FileName);

        }

        public async Task<string[]> UploadImagesAsync(IFormFileCollection files)
        {
            var uploadTasks = new List<Task<string>>();

            foreach (var file in files)
            {
                uploadTasks.Add(UploadImageAsync(file));
            }

            var imageUrls = await Task.WhenAll(uploadTasks);

            return imageUrls;
        }
    }
}