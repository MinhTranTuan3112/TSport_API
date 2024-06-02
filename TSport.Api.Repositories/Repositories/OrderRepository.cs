using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TSport.Api.Models.Entities;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRespository
    {
        private readonly TsportDbContext _context;
        public OrderRepository(TsportDbContext context) : base(context)
        {
            _context = context;
        }
        
    }
}