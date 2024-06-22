using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Shared.Enums;

namespace TSport.Api.Models.RequestModels.Player
{
    public class UpdatePlayerRequest
    {
        public string? Code { get; set; }

        [MaxLength(100, ErrorMessage = "Name is too long")]
        public string? Name { get; set; }

        public int? ClubId { get; set; }
        
        [EnumDataType(typeof(PlayerStatus))]
        public string? Status { get; set; }
    }
}