using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TSport.Api.Models.RequestModels.Order
{
    public class QueryPagedOrderRequest
    {

        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 9;

        [FromQuery(Name = "sortColumn")]
        public string SortColumn { get; set; } = "id";

        [FromQuery(Name = "orderByDesc")]
        public bool OrderByDesc { get; set; } = true;

        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        [FromQuery(Name = "createdAccountId")]
        public int? CreatedAccountId { get; set; }

        [FromQuery(Name = "status")]
        public string? Status { get; set; }
    }
}