using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Shared.Enums;

namespace TSport.Api.Models.RequestModels.Season
{
    public class UpdateSeasonRequest
    {
        [RegularExpression(@"SES\d{3}", ErrorMessage = "Code must be in format SESxxx whete x is a digit.")]
        public string? Code { get; set; }

        [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string? Name { get; set; }

        public int? ClubId { get; set; }

        [EnumDataType(typeof(SeasonStatus))]
        public string? Status { get; set; }
    }
}