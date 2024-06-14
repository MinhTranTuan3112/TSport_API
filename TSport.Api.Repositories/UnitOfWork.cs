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

        public UnitOfWork(TsportDbContext context)
        {
            _context = context;
            _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(context));
            _shirtRepository = new Lazy<IShirtRepository>(() => new ShirtRepository(context));

            _clubRepository = new Lazy<IClubRepository>(() => new ClubRepository(context));

            _imageRepository = new Lazy<IImageRepository>(() => new ImageRepository(context));
        }


        }
        public IClubRepository ClubRepository => _clubRepository.Value;
        public IAccountRepository AccountRepository => _accountRepository.Value;
        public IShirtRepository ShirtRepository => _shirtRepository.Value;
        public IImageRepository ImageRepository => _imageRepository.Value;


      

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}