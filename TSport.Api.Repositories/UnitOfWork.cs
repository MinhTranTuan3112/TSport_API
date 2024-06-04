using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Models.ResponseModels.Cart;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Repositories.Repositories;

namespace TSport.Api.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TsportDbContext _context;
        private readonly Lazy<IAccountRepository> _accountRepository;
        private readonly Lazy<IOrderRepository> _CartRepository;

        public UnitOfWork(TsportDbContext context)
        {
            _context = context;
            _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(context));
            _CartRepository = new Lazy<IOrderRepository>(() => new OrderRepository(context));
        }

        public IAccountRepository AccountRepository => _accountRepository.Value;

        public IOrderRepository CartRepository => _CartRepository.Value;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}