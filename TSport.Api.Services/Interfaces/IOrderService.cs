using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Order;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Models.ResponseModels.Order;
using TSport.Api.Services.BusinessModels.Cart;
using TSport.Api.Services.BusinessModels.Order;

namespace TSport.Api.Services.Interfaces
{
    public interface IOrderService
    {
        Task<PagedResultResponse<OrderModel>> GetPagedOrders(QueryPagedOrderRequest request);
        
        Task<OrderDetailsInfoModel> GetOrderDetailsInfoById(int orderId); 

        Task<OrderCartResponse> GetCartInfo(ClaimsPrincipal claims);
        
        Task ConfirmOrder(ClaimsPrincipal claims, int orderId, List<int>shirtIds);
    }
}
