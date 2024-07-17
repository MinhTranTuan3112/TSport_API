using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Models.ResponseModels.Account
{

    public class OrderDetailResponseModel
    {
        public int OrderId { get; set; }
        public int ShirtId { get; set; }
        public string? Code { get; set; }
        public string Size { get; set; } = null!;
        public decimal Subtotal { get; set; }
        public int Quantity { get; set; }
        public string? Status { get; set; }

        public ShirtResponseModel Shirt { get; set; } = null!;

    }
    public class ShirtResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
      //  public int? Quantity { get; set; }
        public ShirtEditionResponseModel ShirtEdition { get; set; } = null!;

    }
    public class ShirtEditionResponseModel
    {
    //    public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string? Color { get; set; }
        public string? Origin { get; set; }


        /*       public int ShirtEditionId { get; set; }
               public int SeasonPlayerId { get; set; }*/
    }

    public class OrderResponseModel
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal Total { get; set; }
        public List<OrderDetailResponseModel> OrderDetails { get; set; } = new List<OrderDetailResponseModel>();
    }

    public class CustomerResponseModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public DateOnly? Dob { get; set; }
        public string SupabaseId { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? Status { get; set; }
        public List<OrderResponseModel> Orders { get; set; } = new List<OrderResponseModel>();
    }
    public class GetAccountWithOrderReponse
    {
        public List<CustomerResponseModel> Customers { get; set; } = new List<CustomerResponseModel>();

    }
}
