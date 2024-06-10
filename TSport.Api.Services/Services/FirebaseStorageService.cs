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


        public FirebaseStorageService(StorageClient storageClient, IConfiguration configuration)
        {
            _storageClient = storageClient;
            _configuration = configuration;
        }

        public async Task<string> GetImageUrlAsync(string imageName)
        {
            string bucketName = _configuration["Firebase:Bucket"]!;

            return string.Empty;
           
        }

        public async Task UploadImageAsync(IFormFile imageFile)
        {
            var randomGuid = Guid.NewGuid();
            string bucketName = _configuration["Firebase:Bucket"]!;

            using var stream = new MemoryStream();

            await imageFile.CopyToAsync(stream);

            var blob = await _storageClient.UploadObjectAsync(bucketName, imageFile.FileName, imageFile.ContentType, stream, cancellationToken: CancellationToken.None);

        }

    }
}