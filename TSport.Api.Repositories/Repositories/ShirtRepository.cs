using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.Entities;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class ShirtRepository : GenericRepository<Shirt>, IShirtRepository
    {
        private readonly TsportDbContext _context;
        public ShirtRepository(TsportDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Shirt?> GetShirtDetailById(int id)
        {
            var shirt = await _context.Shirts
                .AsNoTracking()
                .Include(s => s.ShirtEdition)
                .Include(s => s.SeasonPlayer)
                    .ThenInclude(sp => sp.Player)
                .Include(s => s.SeasonPlayer)
                    .ThenInclude(sp => sp.Season)
                        .ThenInclude(se => se.Club)
                .SingleOrDefaultAsync(s => s.Id == id);

            return shirt;
        }
    }
}
