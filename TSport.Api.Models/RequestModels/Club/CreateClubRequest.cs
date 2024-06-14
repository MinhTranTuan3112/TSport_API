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

        public string? LogoUrl { get; set; }

        public string? Status { get; set; } = "Active";
    }
}
