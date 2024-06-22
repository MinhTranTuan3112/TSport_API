using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Models.RequestModels.Player
{
    public class CreatePlayerRequest
    {
        [Required]
        public required string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name is too long")]
        public required string Name { get; set; }
        
        public int? ClubId { get; set; }
    }
}