using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Models.RequestModels.Shirt;

namespace TSport.Api.Models.RequestModels.Club
{
    public class QueryClubRequest
    {
        [FromQuery(Name = "page")]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "size")]
        public int PageSize { get; set; } = 9;

        [FromQuery(Name = "sortColumn")]
        public string SortColumn { get; set; } = "id";

        [FromQuery(Name = "orderByDesc")]
        public bool OrderByDesc { get; set; } = true;

        public ClubRequest? ClubRequest { get; set; }
    }
}
