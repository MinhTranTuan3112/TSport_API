using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {

        private readonly TsportDbContext _context;

        public AccountRepository(TsportDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<List<Account>> GetAllAcountCustomer()
        {
            var getAll = await _context.Accounts.AsNoTracking().Where(p => p.Role == "Customer").ToListAsync(); 
            return getAll;
        }

        public async Task<Account?> GetCustomerDetailsInfo(int id)
        {
            return await _context.Accounts
                            .AsNoTracking()
                            .Include(a => a.OrderCreatedAccounts)
                            .ThenInclude(o => o.OrderDetails)
                            .ThenInclude(od => od.Shirt)
                            .ThenInclude(s => s.ShirtEdition)
                            .AsSplitQuery()
                            .SingleOrDefaultAsync(a => a.Id == id);
        }
    }
}