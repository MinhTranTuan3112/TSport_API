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
    }
}
