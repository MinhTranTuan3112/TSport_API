using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Newtonsoft.Json;
using TSport.Api.Models.RequestModels.Player;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels.Player;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Services.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisCacheService<PagedResultResponse<GetPlayerModel>> _pagedResultCacheService;
        public PlayerService(IUnitOfWork unitOfWork, IRedisCacheService<PagedResultResponse<GetPlayerModel>> pagedResultCacheService)
        {
            _unitOfWork = unitOfWork;
            _pagedResultCacheService = pagedResultCacheService;
        }

        public async Task<PagedResultResponse<GetPlayerModel>> GetCachedPagedPlayers(QueryPagedPlayersRequest request)
        {
            return await _pagedResultCacheService.GetOrSetCacheAsync(
                $"pagedPlayers_{JsonConvert.SerializeObject(request)}",
                () => GetPagedPlayers(request)
            ) ?? new PagedResultResponse<GetPlayerModel>();
        }

        public async Task<PagedResultResponse<GetPlayerModel>> GetPagedPlayers(QueryPagedPlayersRequest request)
        {
            return (await _unitOfWork.PlayerRepository.GetPagedPlayers(request)).Adapt<PagedResultResponse<GetPlayerModel>>();
        }
    }
}