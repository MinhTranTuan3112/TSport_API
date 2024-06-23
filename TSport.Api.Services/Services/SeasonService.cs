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

            var season = request.Adapt<Season>();
            
            season.CreatedDate = DateTime.Now;
            season.CreatedAccountId = account.Id;

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
    }
}