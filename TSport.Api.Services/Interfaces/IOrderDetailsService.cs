using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TSport.Api.Models.ResponseModels.Order;

namespace TSport.Api.Services.Interfaces
{
    public interface IOrderDetailsService
    {
         Task AddToCart(ClaimsPrincipal claims, int shirtId, int quantity, string size);

        Task<List<OrderDetailsReponse>> GetList(); 
    }
}
