using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Models.Entities;

namespace TSport.Api.Repositories.Interfaces
{
    public interface IOrderRespository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderInfoWithCustomerInfo(int orderId);
    }
}