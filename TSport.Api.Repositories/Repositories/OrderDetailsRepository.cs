using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class OrderDetailsRepository : GenericRepository<OrderDetail>, IOrderDetailsRepository
    {
        private readonly TsportDbContext _context;

        public OrderDetailsRepository(TsportDbContext context) : base(context)
        {
            _context = context;

        }
        public async Task<bool> ExistingCart(int userID)
        {
            var ExistingCart = await _context.Orders
                .AsNoTracking()
                .Where(p => p.CreatedAccountId == userID && p.Status == "InCart").FirstOrDefaultAsync();


            if (ExistingCart == null)
            {
                return false;

            }
            else
            {

                return true;
            }

        }
        public async Task<int> GetCartId(int userId)
        {
            var GetCartId = await _context.Orders
                           .AsNoTracking()
                           .Include(od => od.OrderDetails)
                           .Where(p => p.CreatedAccountId == userId && p.Status == "InCart").FirstOrDefaultAsync();
            return GetCartId.Id;

        }

        public async Task<int> TotalOrderDetails()
        {
            var totalAmount = await _context.Orders
        .AsNoTracking()
        .Include(od => od.OrderDetails)
        .SelectMany(od => od.OrderDetails) // Flatten the collection
        .CountAsync();
            return (int)totalAmount;
        }

        public async Task<decimal> GetDiscountPrice(int shirtId)
        {
            var Product = await _context.Orders
                .AsNoTracking()
                .Include(od => od.OrderDetails)
                .ThenInclude(od => od.Shirt)
                .ThenInclude(od => od.ShirtEdition)
                .FirstOrDefaultAsync(od => od.Id == shirtId);

            var ProductPrice = Product?.OrderDetails.FirstOrDefault();


            return ProductPrice.Shirt.ShirtEdition.DiscountPrice ?? 0;
        }
        /*
        public async Task<OrderDetail?> FindOneAsync(Expression<Func<OrderDetail, bool>> predicate)
        {
            return await _context.OrderDetails.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        
        public async Task DeleteAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
        }
        */

        public async Task<List<OrderDetail?>> Getall()
        {
            return await _context.OrderDetails.ToListAsync();
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdsAsync(List<int> orderIds)
        {
            return await _context.OrderDetails
          .Where(od => orderIds.Contains(od.OrderId))
          .ToListAsync();
        }
    }
}
