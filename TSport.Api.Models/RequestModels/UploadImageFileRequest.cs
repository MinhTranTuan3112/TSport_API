using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TSport.Api.Models.RequestModels
{
    public class UploadImageFileRequest
    {
        [Required]
        public required IFormFile ImageFile { get; set; }
    }
}