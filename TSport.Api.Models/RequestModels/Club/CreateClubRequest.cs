﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Models.RequestModels.Club
{
    public class CreateClubRequest
    {

        [Required]
        [RegularExpression("^\\d{3}$", ErrorMessage = "Invalid shirt code")]
        public string? Code { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Required]
        public IFormFile Images { get; set; } = null!;

        public string? Status { get; set; } = "Active";
    }
}
