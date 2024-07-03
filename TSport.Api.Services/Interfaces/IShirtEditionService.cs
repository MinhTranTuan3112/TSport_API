using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.RequestModels.ShirtEdition;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Services.BusinessModels.Season;
using TSport.Api.Services.BusinessModels.ShirtEdition;

namespace TSport.Api.Services.Interfaces
{
    public interface IShirtEditionService
    {
        Task<PagedResultResponse<GetShirtEdtionModel>> GetPagedSeasons(QueryPagedShirtEditionRequest request);

        Task<List<ShirtEdition>> getAllShirtEdition();

        Task<ShirtEdition> GetShirteditionDetailsById(int id);

        Task<ShirtEditionRequest> CreateShirtEdition(ShirtEditionRequest request, ClaimsPrincipal claims);

        Task UpdateShirtEdition(int id, ShirtEditionRequest request, ClaimsPrincipal claims);

        Task<bool> DeleteShirtEditionAsync(int seasonId);
        /*Task<PagedResultResponse<GetSeasonModel>> GetPagedSeasons(QueryPagedSeasonRequest request);

                Task<GetSeasonDetailsModel> GetSeasonDetailsById(int id);

                Task<GetSeasonModel> CreateSeason(CreateSeasonRequest request, ClaimsPrincipal claims);

                Task UpdateSeason(int id, UpdateSeasonRequest request, ClaimsPrincipal claims);

                Task<bool> DeleteSeasonAsync(int seasonId);*/
    }
}
