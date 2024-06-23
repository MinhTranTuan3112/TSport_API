using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using TSport.Api.DataAccess.Interfaces;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Repositories.Repositories;

namespace TSport.Api.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TsportDbContext _context;
        private IDbContextTransaction? _currentTransaction;
        private readonly Lazy<IAccountRepository> _accountRepository;
        private readonly Lazy<IShirtRepository> _shirtRepository;
        private readonly Lazy<IClubRepository> _clubRepository;
        private readonly Lazy<IImageRepository> _imageRepository;
        private readonly Lazy<ISeasonRepository> _seasonRepository;
        private readonly Lazy<IPlayerRepository> _playerRepository;

        public UnitOfWork(TsportDbContext context)
        {
            _context = context;
            _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(context));
            _shirtRepository = new Lazy<IShirtRepository>(() => new ShirtRepository(context));
            _clubRepository = new Lazy<IClubRepository>(() => new ClubRepository(context));
            _imageRepository = new Lazy<IImageRepository>(() => new ImageRepository(context));
            _seasonRepository = new Lazy<ISeasonRepository>(() => new SeasonRepository(context));
            _playerRepository = new Lazy<IPlayerRepository>(() => new PlayerRepository(context));
        }
        
        public IClubRepository ClubRepository => _clubRepository.Value;
        public IAccountRepository AccountRepository => _accountRepository.Value;
        public IShirtRepository ShirtRepository => _shirtRepository.Value;
        public IImageRepository ImageRepository => _imageRepository.Value;

        public ISeasonRepository SeasonRepository => _seasonRepository.Value;

        public IPlayerRepository PlayerRepository => _playerRepository.Value;

        public async Task BeginTransactionAsync()
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.CommitAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
