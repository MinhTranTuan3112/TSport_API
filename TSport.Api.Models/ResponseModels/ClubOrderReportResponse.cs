using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Order;
using TSport.Api.Models.RequestModels.Shirt;

namespace TSport.Api.Models.ResponseModels
{
    public class ClubOrderReportResponse
    {
        public int TotalOrderCount { get; set; } // Tổng số lượng đơn hàng
        public int TotalShirtQuantity { get; set; } // Tổng số lượng áo trong các đơn hàng
        public decimal TotalRevenue { get; set; }
        public List<ShirtQuantityBySize> ShirtQuantitiesBySize { get; set; } // Số lượng áo phân loại theo size
        public List<OrderModel> Orders { get; set; }
    }

}
