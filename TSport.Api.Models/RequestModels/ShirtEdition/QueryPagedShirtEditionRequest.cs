using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Models.RequestModels.ShirtEdition
{
    public class QueryPagedShirtEditionRequest
    {
        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "sortColumn")]
        public string SortColumn { get; set; } = "id";

        [FromQuery(Name = "orderByDesc")]
        public bool OrderByDesc { get; set; } = true;

        [FromQuery(Name = "code")]
        public string? Code { get; set; }
        
        [FromQuery(Name = "size")]
        public string? Size { get; set; }

        [FromQuery(Name = "hasSignature")]
        public bool? HasSignature { get; set; }

        [FromQuery(Name = "stockPrice")]
        public decimal StockPrice { get; set; }

        [FromQuery(Name = "startPrice")]
        public decimal StartPrice { get; set; }

        [FromQuery(Name = "endPrice")]
        public decimal EndPrice { get; set; }

        [FromQuery(Name = "discountPrice")]
        public decimal? DiscountPrice { get; set; }

        [FromQuery(Name = "color")]
        public string? Color { get; set; }

        [FromQuery(Name = "origin")]
        public string? Origin { get; set; }

        [FromQuery(Name = "quantity")]
        public int Quantity { get; set; }

        [FromQuery(Name = "material")]
        public string? Material { get; set; }

        [FromQuery(Name = "seasonid")]
        public int? SeasonId { get; set; }


    }
}
