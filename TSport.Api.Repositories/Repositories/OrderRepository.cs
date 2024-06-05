using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.Entities;
using TSport.Api.Models.ResponseModels.Cart;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(TsportDbContext context) : base(context)
        {
        }



        public async Task<Order?> GetCartByID(int AccountId)
        {

            var items =  await _context.Orders
                .AsNoTracking()
                .Include(o =>o.OrderDetails)
                    .ThenInclude(b => b.Shirt)
                        .ThenInclude(b => b.ShirtEdition)
                            .ThenInclude(b => b.Season)
                .ThenInclude(b => b.SeasonPlayers)
                .Include(b => b.OrderDetails)
                .ThenInclude(b => b.Shirt)
                .ThenInclude(b=> b.SeasonPlayer)
                .ThenInclude(b=>b.Player)
                .ThenInclude( b=> b.Club)
                .Where(o => o.CreatedAccountId == AccountId && o.Status == "Pending").FirstOrDefaultAsync();

            return items;
        }
      
    }
}
