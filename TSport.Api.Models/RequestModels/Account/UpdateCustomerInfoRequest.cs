using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TSport.Api.Shared;
using TSport.Api.Shared.Enums;

namespace TSport.Api.Models.RequestModels.Account
{
    public class UpdateCustomerInfoRequest
    {
        [MaxLength(100, ErrorMessage = "First name must be less than 100 characters")]
        public string? FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "Last name must be less than 100 characters")]
        public string? LastName { get; set; }

        [EnumDataType(typeof(Gender))]
        public string? Gender { get; set; }

        [MaxLength(255, ErrorMessage = "Address must be less than 255 characters")]
        public string? Address { get; set; }

        public string? Phone { get; set; }

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly? Dob { get; set; }
    }
}