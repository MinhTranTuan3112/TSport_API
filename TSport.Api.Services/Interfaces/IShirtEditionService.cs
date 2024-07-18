using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.RequestModels.ShirtEdition;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Services.BusinessModels;
using TSport.Api.Services.BusinessModels.Season;
using TSport.Api.Services.BusinessModels.ShirtEdition;

namespace TSport.Api.Services.Interfaces
{
    public interface IShirtEditionService
    {
        Task<PagedResultResponse<GetShirtEdtionModel>> GetPagedShirtEditions(QueryPagedShirtEditionRequest request);

        Task<ShirtEditionDetailsModel> GetShirtEditionDetailsById(int id);

        Task<ShirtEditionModel> CreateShirtEdition(CreateShirtEditionRequest request, ClaimsPrincipal claims);

        Task UpdateShirtEdition(int id, UpdateShirtEditionRequest request, ClaimsPrincipal claims);

        Task DeleteShirtEdition(int shirtEditionId);
        Task<List<GetShirtEdtionModel>> GetShirtEditions();
    }
}
