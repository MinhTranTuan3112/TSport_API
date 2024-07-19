using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Shared.Enums;

namespace TSport.Api.Models.RequestModels.Club
{
    public class UpdateClubRequest
    {
        [Required]      
        [RegularExpression("^CLB\\d{3}$", ErrorMessage = "Invalid shirt code")]
        public string? Code { get; set; }
        
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }


        public IFormFile? Image { get; set; }
        
        [EnumDataType(typeof(ShirtStatus))]
        public string? Status { get; set; }
    }
}
