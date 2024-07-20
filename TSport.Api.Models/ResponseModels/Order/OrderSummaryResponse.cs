using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Models.ResponseModels.Order
{
    public class OrderSummaryResponse
    {
        public int TotalOrders { get; set; }
        public List<OrderStatusSummary> OrdersByStatus { get; set; }
    }
}
