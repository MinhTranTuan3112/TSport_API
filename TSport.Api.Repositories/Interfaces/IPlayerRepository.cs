using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Player;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Repositories.Interfaces
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task<PagedResultResponse<Player>> GetPagedPlayers(QueryPagedPlayersRequest request);
        Task<Player?> GetPlayerDetailsById(int id);
        Task<List<Player>> GetAll();
    }
}