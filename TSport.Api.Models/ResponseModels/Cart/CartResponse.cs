using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Models.ResponseModels.Cart
{
    public class CartResponse
    {
        public int OrderId { get; set; }
        public int CreatedAccountId { get; set; }


        public int ShirtId { get; set; }

        public string? Code { get; set; }

        public decimal? Subtotal { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }

        /* public int OrderId { get; set; }

            public int ShirtId { get; set; }

            public string? Code { get; set; }

            public decimal? Subtotal { get; set; }

            public int? Quantity { get; set; }

            public string? Status { get; set; }*/
        // public List<CartItemResponse> OrderDetail { get; set; }
        public virtual ICollection<CartItemResponse> OrderDetail { get; set;}

    }
    public class CartItemResponse
    {

        //TbOrderDetails
        public int OrderId { get; set; }
        public int ShirtId { get; set; }
        public int? Quantity { get; set; }
        public string? Status { get; set; }
        public decimal? Subtotal { get; set; }



        // Table Shirt  cart -> price ,color ,status -> de canh bao cart còn hàng không ? , 

        public string? Size { get; set; }

        public bool? HasSignature { get; set; }

        public decimal? Price { get; set; }

        public string? Color { get; set; }
        public int SeasonPlayerId { get; set; }
        //DuaVao SeasonPlayerID -> co Name trong table Player
        public string? PlayerName { get; set; }

        public int? ClubId { get; set; }

        //tb club
        public string? ClubName { get; set; }   
        public int SeasonId { get; set; }
        //Tb Season
        public string? SeasonName { get; set; }

    }
}
