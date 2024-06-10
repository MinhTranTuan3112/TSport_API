using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> TestUpload(IFormFile file)
        {
            await _serviceFactory.FirebaseStorageService.UploadImageAsync(file);
            return Ok();
        }
    }
}