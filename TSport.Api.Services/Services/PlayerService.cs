using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Newtonsoft.Json;
using TSport.Api.Models.RequestModels.Player;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels.Player;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Enums;
using TSport.Api.Shared.Exceptions;

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

        public async Task<GetPlayerModel> CreatePlayer(CreatePlayerRequest request, ClaimsPrincipal claims)
        {
            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);

            if (account is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }

            if (request.ClubId.HasValue)
            {
                if (!await _unitOfWork.ClubRepository.AnyAsync(c => c.Id == request.ClubId))
                {
                    throw new BadRequestException("Club does not exist");
                }
            }

            var player = request.Adapt<Player>();
            player.CreatedDate = DateTime.Now;
            player.CreatedAccountId = account.Id;
            player.Status = PlayerStatus.Active.ToString();

            await _unitOfWork.PlayerRepository.AddAsync(player);
            await _unitOfWork.SaveChangesAsync();

            return player.Adapt<GetPlayerModel>();
        }

        public async Task<List<ViewReponse>> GetAllPlayers()
        {
            var players = (await _unitOfWork.PlayerRepository.GetAll());
            if (players == null) {
                throw new NotFoundException("No player was found");


            }
            return players.Adapt<List<ViewReponse>>();
        }
        public async Task<PagedResultResponse<GetPlayerModel>> GetPagedPlayers(QueryPagedPlayersRequest request)
        {
            return (await _unitOfWork.PlayerRepository.GetPagedPlayers(request)).Adapt<PagedResultResponse<GetPlayerModel>>();
        }
        public async Task<PagedResultResponse<GetPlayerModel>> GetCachedPagedPlayers(QueryPagedPlayersRequest request)
        {
            return await _pagedResultCacheService.GetOrSetCacheAsync(
                $"pagedPlayers_{JsonConvert.SerializeObject(request)}",
                () => GetPagedPlayers(request)
            ) ?? new PagedResultResponse<GetPlayerModel>();
        }

      

        public async Task<GetPlayerDetailsModel> GetPlayerDetailsById(int id)
        {
            var player = await _unitOfWork.PlayerRepository.GetPlayerDetailsById(id);

            if (player is null)
            {
                throw new NotFoundException("Player does not exist");
            }

            return player.Adapt<GetPlayerDetailsModel>();
        }

        public async Task UpdatePlayer(int id, UpdatePlayerRequest request, ClaimsPrincipal user)
        {
            var supabaseId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);

            if (account is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }

            if (request.ClubId.HasValue)
            {
                if (!await _unitOfWork.ClubRepository.AnyAsync(c => c.Id == request.ClubId))
                {
                    throw new BadRequestException("Club does not exist");
                }
            }

            var player = await _unitOfWork.PlayerRepository.FindOneAsync(p => p.Id == id);

            if (player is null)
            {
                throw new NotFoundException("Player not found");
            }

          
            request.Adapt(player);
            
            await _unitOfWork.SaveChangesAsync();
        }
    }
}