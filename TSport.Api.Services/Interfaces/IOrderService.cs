using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Models.ResponseModels.Order;
using TSport.Api.Services.BusinessModels.Cart;

namespace TSport.Api.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderCartResponse> GetCartInfo(ClaimsPrincipal claims);

        Task<OrderResponse> GetOrderByIdAsync(int orderId);

        Task<bool> CreateOrderAsync(int orderId);
    }
}
