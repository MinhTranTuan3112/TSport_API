using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public ImagesController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpPost("test-upload")]
        public async Task<IActionResult> TestUpload(UploadImageFileRequest request)
        {
            return Created(nameof(TestUpload), await _serviceFactory.SupabaseStorageService.UploadImageAsync(request.ImageFile, "TSport"));
        }

        

    }
}