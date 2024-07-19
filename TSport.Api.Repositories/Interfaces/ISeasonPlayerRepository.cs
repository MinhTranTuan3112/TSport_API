using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Repositories.Interfaces
{
    public interface ISeasonPlayerRepository : IGenericRepository<SeasonPlayer>
    {
        //Task<SeasonPlayer> Get
        Task<List<Player>> getPlayerName();
    }
}