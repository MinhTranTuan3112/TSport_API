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

        Task<Decimal> GetMonthlyRevenueNow();

        Task<decimal> GetMonthlyRevenue(int year, int month);

        Task<int> GetTotalOrder();

        Task<Order?> GetFullOrderInfo(int orderId);

        Task<List<int>> GetValidOrderIdsAsync();


    }
}
