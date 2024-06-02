using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Models.RequestModels.Shirt
{
    public class QueryShirtRequest
    {
        public string? Code { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }

        public int? ShirtEditionId { get; set; }

        public int? SeasonPlayerId { get; set; }
    }
}