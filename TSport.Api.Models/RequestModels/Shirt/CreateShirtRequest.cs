using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Models.RequestModels.Shirt
{
    public class CreateShirtRequest
    {
        [Required]
        [RegularExpression("^SRT\\d{3}$", ErrorMessage = "Invalid shirt code")]
        public string Code { get; set; } = null!;

        [Required]
        [MaxLength(255, ErrorMessage = "Name is too long")]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        [RegularExpression("^\\d+$", ErrorMessage = "Invalid number")]
        public int? Quantity { get; set; }

        public string? Status { get; set; } = "Active";

        [Required]
        [RegularExpression("^\\d+$", ErrorMessage = "Invalid id")]
        public int ShirtEditionId { get; set; }

        [Required]
        [RegularExpression("^\\d+$", ErrorMessage = "Invalid id")]
        public int SeasonPlayerId { get; set; }

        [Required]
        public IFormFileCollection Images { get; set; } = null!;
    }
}
