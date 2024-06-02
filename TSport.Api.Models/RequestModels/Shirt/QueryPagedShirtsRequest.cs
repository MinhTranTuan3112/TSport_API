using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Models.RequestModels.Shirt
{
    public class QueryPagedShirtsRequest
    {
        public int PageNumber { get; set; } = 1;
        
        public int PageSize { get; set; } = 9;

        public string SortColumn { get; set; } = "id";

        public bool OrderByDesc { get; set; } = true;

        public QueryShirtRequest? QueryShirtRequest { get; set; }

        public decimal StartPrice { get; set; }

        public decimal EndPrice { get; set; }
    }
}