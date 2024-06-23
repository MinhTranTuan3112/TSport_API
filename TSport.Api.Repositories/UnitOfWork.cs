using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.DataAccess.Interfaces;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Repositories.Repositories;

namespace TSport.Api.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TsportDbContext _context;
        private readonly Lazy<IAccountRepository> _accountRepository;
        private readonly Lazy<IShirtRepository> _shirtRepository;
        private readonly Lazy<IClubRepository> _clubRepository;
        private readonly Lazy<IImageRepository> _imageRepository;
        private readonly Lazy<ISeasonRepository> _seasonRepository;
        private readonly Lazy<IPlayerRepository> _playerRepository;
        private readonly Lazy<IOrderRepository> _OrderRepository;
        private readonly Lazy<IOrderDetailsRepository> _orderdetailsRepository;
        public UnitOfWork(TsportDbContext context)
        {
            _context = context;
            _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(context));
            _shirtRepository = new Lazy<IShirtRepository>(() => new ShirtRepository(context));
            _clubRepository = new Lazy<IClubRepository>(() => new ClubRepository(context));
            _imageRepository = new Lazy<IImageRepository>(() => new ImageRepository(context));
            _seasonRepository = new Lazy<ISeasonRepository>(() => new SeasonRepository(context));
            _playerRepository = new Lazy<IPlayerRepository>(() => new PlayerRepository(context));
            _OrderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(context));
            _orderdetailsRepository = new Lazy<IOrderDetailsRepository>(() => new OrderDetailsRepository(context));
        }



        public IClubRepository ClubRepository => _clubRepository.Value;
        public IAccountRepository AccountRepository => _accountRepository.Value;
        public IShirtRepository ShirtRepository => _shirtRepository.Value;
        public IImageRepository ImageRepository => _imageRepository.Value;
        public IOrderRepository OrderRepository => _OrderRepository.Value;
        public ISeasonRepository SeasonRepository => _seasonRepository.Value;

        public IPlayerRepository PlayerRepository => _playerRepository.Value;
        public IOrderDetailsRepository OrderDetailsRepository => _orderdetailsRepository.Value;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
