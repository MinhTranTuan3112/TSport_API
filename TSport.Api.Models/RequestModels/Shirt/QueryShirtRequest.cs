using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TSport.Api.Models.RequestModels.Shirt
{
    public class QueryShirtRequest
    {
        [FromQuery(Name = "code")]
        public string? Code { get; set; }

        [FromQuery(Name = "name")]
        public string? Name { get; set; }

        [FromQuery(Name = "description")]
        public string? Description { get; set; }

        [FromQuery(Name = "quantity")]
        public string? Status { get; set; }

        [FromQuery(Name = "shirtEditionId")]
        public int? ShirtEditionId { get; set; }

        [FromQuery(Name = "seasonPlayerId")]
        public int? SeasonPlayerId { get; set; }
    }
}