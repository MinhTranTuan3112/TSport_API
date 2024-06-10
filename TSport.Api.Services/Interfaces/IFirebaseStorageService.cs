using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TSport.Api.Services.Interfaces
{
    public interface IFirebaseStorageService
    {
        Task UploadImageAsync(IFormFile imageFile);

        Task<string> GetImageUrlAsync(string imageName);
    }
}