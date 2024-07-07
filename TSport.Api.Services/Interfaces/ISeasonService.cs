using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Services.BusinessModels.Season;

namespace TSport.Api.Services.Interfaces
{
    public interface ISeasonService
    {
        Task<PagedResultResponse<GetSeasonModel>> GetPagedSeasons(QueryPagedSeasonRequest request);

        Task<GetSeasonDetailsModel> GetSeasonDetailsById(int id);

        Task<GetSeasonModel> CreateSeason(CreateSeasonRequest request, ClaimsPrincipal claims);

        Task UpdateSeason(int id, UpdateSeasonRequest request, ClaimsPrincipal claims);

        Task<bool> DeleteSeasonAsync(int seasonId);
        Task<List<ViewReponse>> GetAllSeasons();

    }
}