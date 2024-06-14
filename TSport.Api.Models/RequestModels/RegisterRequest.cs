using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Shared.Enums;

namespace TSport.Api.Models.RequestModels
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public required string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Password must have minimum 8 characters (>= 1 uppercase, >=1 lowercase, >= 1 digit, >= 1 special character)")]
        public required string Password { get; set; }

        [Required]
        [EnumDataType(typeof(Role), ErrorMessage = "Invalid role")]
        public required string Role { get; set; }

        [Required]
        public required string SupabaseId { get; set; }
    }
}