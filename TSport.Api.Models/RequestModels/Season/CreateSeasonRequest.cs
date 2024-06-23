using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Models.RequestModels.Season
{
    public class CreateSeasonRequest
    {
        [Required]
        [RegularExpression(@"SES\d{3}", ErrorMessage = "Code must be in format SESxxx whete x is a digit.")]
        public required string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public required string Name { get; set; }

        public int? ClubId { get; set; }
    }
}