using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Player;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Services.BusinessModels.Player;

namespace TSport.Api.Services.Interfaces
{
    public interface IPlayerService
    {
        Task<PagedResultResponse<GetPlayerModel>> GetPagedPlayers(QueryPagedPlayersRequest request);  

        Task<PagedResultResponse<GetPlayerModel>> GetCachedPagedPlayers(QueryPagedPlayersRequest request); 

        Task<GetPlayerModel> CreatePlayer(CreatePlayerRequest request, ClaimsPrincipal claims);

        Task<GetPlayerDetailsModel> GetPlayerDetailsById(int id);

        Task UpdatePlayer(int id, UpdatePlayerRequest request, ClaimsPrincipal user);

        Task<List<ViewReponse>> GetAllPlayers();
    }
}