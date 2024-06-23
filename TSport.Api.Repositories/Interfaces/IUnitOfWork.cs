using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.DataAccess.Interfaces;

namespace TSport.Api.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        IAccountRepository AccountRepository { get; }

        IClubRepository ClubRepository { get; }

        IShirtRepository ShirtRepository { get; }

        IImageRepository ImageRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailsRepository OrderDetailsRepository { get; }

        ISeasonRepository SeasonRepository { get; }

        IPlayerRepository PlayerRepository { get; }

    }
}