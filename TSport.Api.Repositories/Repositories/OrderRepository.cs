using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Shared.Enums;

namespace TSport.Api.Repositories.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly TsportDbContext _context;

        public OrderRepository(TsportDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Order?> GetCustomerCartInfo(int accountId)
        {

            var order = await _context.Orders
                .AsNoTracking()
                .Where(o => o.CreatedAccountId == accountId && o.Status == OrderStatus.InCart.ToString())
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Shirt)
                        .ThenInclude(s => s.ShirtEdition)
                .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Shirt)
                            .ThenInclude(s => s.Images) // Include Shirt Images
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            return order;
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

    }
}
