using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Models.RequestModels.Order
{
    public class AddToCartRequest
    {
        public int ShirtId { get; set; }

        public int Quantity { get; set; }
    }
}