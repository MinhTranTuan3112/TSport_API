using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels.Season;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Services.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeasonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResultResponse<GetSeasonModel>> GetPagedSeasons(QueryPagedSeasonRequest request)
        {
            return (await _unitOfWork.SeasonRepository.GetPagedSeasons(request)).Adapt<PagedResultResponse<GetSeasonModel>>();
        }

        
    }
}