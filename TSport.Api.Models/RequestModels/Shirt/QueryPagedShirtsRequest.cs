using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace TSport.Api.Models.RequestModels.Shirt
{
    public class QueryPagedShirtsRequest
    {
        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 9;

        [FromQuery(Name = "sortColumn")]
        public string SortColumn { get; set; } = "id";

        [FromQuery(Name = "orderByDesc")]
        public bool OrderByDesc { get; set; } = true;

        // public QueryShirtRequest? QueryShirtRequest { get; set; }

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

        [FromQuery(Name = "seasonId")]
        public List<int> SeasonIds { get; set; } = [];

        [FromQuery(Name = "playerId")]
        public List<int> PlayerIds { get; set; } = [];

        [FromQuery(Name = "startPrice")]
        public decimal StartPrice { get; set; }

        [FromQuery(Name = "endPrice")]
        public decimal EndPrice { get; set; }

        [FromQuery(Name = "sizes")]
        public List<string> Sizes { get; set; } = [];

        [FromQuery(Name = "clubId")]
        public List<int> ClubIds { get; set; } = [];
    }
}