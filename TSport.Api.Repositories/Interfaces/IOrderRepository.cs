using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Order;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<PagedResultResponse<Order>> GetPagedOrders(QueryPagedOrderRequest request);

        Task<Order?> GetOrderDetailsInfoById(int orderId);
        Task<Order?> GetCustomerCartInfo(int id);

        void Update(Order order);
        Task<List<Order>?> GetCustomerInfo(int accountId);

    }
}
