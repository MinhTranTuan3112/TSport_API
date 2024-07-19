using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Repositories.Interfaces
{
    public interface IOrderDetailsRepository : IGenericRepository<OrderDetail>
    {
        Task<bool> ExistingCart(int userId);
        Task<int> GetCartId(int id);

        Task<int> TotalOrderDetails();

        Task<decimal> GetDiscountPrice(int shirtId);

//        Task<OrderDetail?> FindOneAsync(Expression<Func<OrderDetail, bool>> predicate);

//        Task DeleteAsync(OrderDetail orderDetail);
   
        Task<List<OrderDetail?>> Getall();
        Task<List<OrderDetail>> GetOrderDetailsByOrderIdsAsync(List<int> orderIds);


    }
}
