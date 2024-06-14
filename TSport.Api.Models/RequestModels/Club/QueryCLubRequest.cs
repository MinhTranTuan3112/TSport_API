using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Shirt;

namespace TSport.Api.Models.RequestModels.Club
{
    public class QueryCLubRequest
    {

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 9;

        public string SortColumn { get; set; } = "id";

        public bool OrderByDesc { get; set; } = true;

        public ClubRequest? ClubRequest { get; set; }


        public List<string>? Sizes { get; set; } = [];
    }
}
