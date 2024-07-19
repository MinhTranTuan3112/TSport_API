using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using TSport.Api.Models.RequestModels.Club;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Club;
using TSport.Api.Models.ResponseModels.Shirt;
using TSport.Api.Repositories.Entities;
using TSport.Api.Services.BusinessModels.Club;

namespace TSport.Api.BusinessLogic.Interfaces
{
    public interface IClubService
    {       
        Task<PagedResultResponse<GetClubModel>> GetPagedClubs(QueryClubRequest request);
        Task<PagedResultResponse<GetClubModel>> GetCachedPagedClubs(QueryClubRequest request);
        Task<GetClubDetailsModel> GetClubDetailsById(int clubId);
        Task<GetClubResponse> AddClub(CreateClubRequest shirt, ClaimsPrincipal user);
        Task DeleteClub(int id);
        Task UpdateClub(int id, UpdateClubRequest updateClubRequest, ClaimsPrincipal user);
        Task<List<ViewReponse>> GetAllClubs();

    }
}
