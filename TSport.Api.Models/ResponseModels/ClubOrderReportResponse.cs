using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Order;

namespace TSport.Api.Models.ResponseModels
{
    public class ClubOrderReportResponse
    {
        public decimal TotalRevenue { get; set; }
        public List<OrderModel> Orders { get; set; }
    }
}
