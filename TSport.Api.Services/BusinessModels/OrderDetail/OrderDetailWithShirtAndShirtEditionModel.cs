using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Services.BusinessModels.Shirt;

namespace TSport.Api.Services.BusinessModels.OrderDetail
{
    public class OrderDetailWithShirtAndShirtEditionModel
    {
        public int OrderId { get; set; }

        public int ShirtId { get; set; }

        public string? Code { get; set; }

        public string Size { get; set; } = null!;

        public decimal Subtotal { get; set; }

        public int Quantity { get; set; }

        public string? Status { get; set; }

        public ShirtWithImagesAndShirtEditionModel Shirt { get; set; } = null!;
    }
}