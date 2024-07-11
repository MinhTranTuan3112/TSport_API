using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Models.ResponseModels.Order
{
    public class OrderDetailsReponse
    {
        public int OrderId { get; set; }

        //public int ShirtId { get; set; }

        public string? Code { get; set; }

        public string Size { get; set; } = null!;

        public decimal Subtotal { get; set; }

        public int Quantity { get; set; }

        public string? Status { get; set; }
    }
}
