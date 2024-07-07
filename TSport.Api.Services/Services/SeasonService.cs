using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels.Season;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Enums;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Services.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeasonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetSeasonModel> CreateSeason(CreateSeasonRequest request, ClaimsPrincipal claims)
        {
            if (await _unitOfWork.SeasonRepository.AnyAsync(s => s.Code == request.Code))
            {
                throw new BadRequestException("Season with this code already exists");
            }

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

            var season = request.Adapt<Season>();

            season.CreatedDate = DateTime.Now;
            season.CreatedAccountId = account.Id;
            season.Status = SeasonStatus.Active.ToString();

            await _unitOfWork.SeasonRepository.AddAsync(season);
            await _unitOfWork.SaveChangesAsync();

            return season.Adapt<GetSeasonModel>();
        }

        public async Task<PagedResultResponse<GetSeasonModel>> GetPagedSeasons(QueryPagedSeasonRequest request)
        {
            return (await _unitOfWork.SeasonRepository.GetPagedSeasons(request)).Adapt<PagedResultResponse<GetSeasonModel>>();
        }

        public async Task<GetSeasonDetailsModel> GetSeasonDetailsById(int id)
        {
            var season = await _unitOfWork.SeasonRepository.GetSeasonDetailsById(id);

            if (season is null)
            {
                throw new NotFoundException("Season not found");
            }


            return season.Adapt<GetSeasonDetailsModel>();
        }

        public async Task UpdateSeason(int id, UpdateSeasonRequest request, ClaimsPrincipal claims)
        {
            var season = await _unitOfWork.SeasonRepository.FindOneAsync(s => s.Id == id);

            if (season is null)
            {
                throw new NotFoundException("Season not found");
            }

            if (request.ClubId.HasValue)
            {
                if (!await _unitOfWork.ClubRepository.AnyAsync(c => c.Id == request.ClubId))
                {
                    throw new BadRequestException("Club does not exist");
                }
            }

            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);

            if (account is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }

            request.Adapt(season);
            season.ModifiedAccountId = account.Id;
            season.ModifiedDate = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteSeasonAsync(int seasonId)
        {
            var season = await _unitOfWork.SeasonRepository.GetByIdAsync(seasonId);
            if (season == null)
            {
                return false;
            }

            season.Status = SeasonStatus.Deleted.ToString();
            _unitOfWork.SeasonRepository.Update(season);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<ViewReponse>> GetAllSeasons()
        {
            var Seasons = (await _unitOfWork.SeasonRepository.GetAll());
            if (Seasons == null)
            {
                throw new NotFoundException("No Season was found");


            }
            return Seasons.Adapt<List<ViewReponse>>();
        }
    }
}