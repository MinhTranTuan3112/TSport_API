using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Shared.Enums;

namespace TSport.Api.Models.RequestModels.Shirt
{
    public class UpdateShirtRequest
    {
        [RegularExpression("^SRT\\d{3}$", ErrorMessage = "Invalid shirt code")]
        public string? Code { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [RegularExpression("^\\d+$", ErrorMessage = "Invalid number")]
        public int? Quantity { get; set; }

        [RegularExpression("^\\d+$", ErrorMessage = "Invalid id")]
        public int? ShirtEditionId { get; set; }

        [RegularExpression("^\\d+$", ErrorMessage = "Invalid id")]
        public int? SeasonPlayerId { get; set; }

        [EnumDataType(typeof(ShirtStatus))]
        public string? Status { get; set; }

        public IFormFileCollection? ShirtImages { get; set; }
    }
}
