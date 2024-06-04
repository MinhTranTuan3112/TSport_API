using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //Iqueryable


           /* var response = items.Select(order => new CartResponse
            {
                OrderId = order.Id,
                CreatedAccountId = order.CreatedAccountId,
                // ShirtId = order.OrderDetails.FirstOrDefault()?.ShirtId ?? 0, // Assuming each order has at least one detail
                ShirtId = order.OrderDetails.FirstOrDefault().ShirtId,
                Code = order.Code,
                Subtotal = order.Total,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Items = order.OrderDetails.Select(od => new CartItemResponse
                {
                    OrderId = od.OrderId,
                    ShirtId = od.ShirtId,
                    Quantity = od.Quantity,
                    Status = od.Status,
                    Subtotal = od.Subtotal,
                    Size = od.Shirt.ShirtEdition.Size,
                    HasSignature = od.Shirt.ShirtEdition.HasSignature,
                    Price = od.Shirt.ShirtEdition.StockPrice,
                    Color = od.Shirt.ShirtEdition.Color,
                    //SeasonPlayerId = od.Shirt.ShirtEdition.Season.SeasonPlayers.FirstOrDefault()?.Player.Id ?? 0, // Assuming each shirt edition has at least one season player
                    SeasonPlayerId = od.Shirt.ShirtEdition.Season.SeasonPlayers.FirstOrDefault().Player.Id, 
                    PlayerName = od.Shirt.ShirtEdition.Season.SeasonPlayers.FirstOrDefault()?.Player.Name,
                    ClubId = od.Shirt.ShirtEdition.Season.Club.Id,
                    ClubName = od.Shirt.ShirtEdition.Season.Club.Name,
                    SeasonId = od.Shirt.ShirtEdition.Season.Id,
                    SeasonName = od.Shirt.ShirtEdition.Season.Name
                }).ToList()
            }).ToList();

            return response;*/
        }

    }
}
