﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.ResponseModels.Cart;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly TsportDbContext _context;

        public OrderRepository(TsportDbContext context) : base(context)
        {
            _context = context;
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
                .Where(o => o.CreatedAccountId == AccountId && o.Status == "InCart").FirstOrDefaultAsync();

            return items;
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

    }
}
