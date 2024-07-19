using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class SeasonPlayerRepository : GenericRepository<SeasonPlayer>, ISeasonPlayerRepository
    {
        private readonly TsportDbContext _context;
        public SeasonPlayerRepository(TsportDbContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<List<Player>> getPlayerName()
        {
            return await _context.Players.ToListAsync();

        }
    }
}