using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Shirt;
using TSport.Api.Services.BusinessModels;
using TSport.Api.Services.BusinessModels.Shirt;

namespace TSport.Api.Services.Interfaces
{
    public interface IShirtService
    {
        Task<PagedResultResponse<GetShirtInPagingResultModel>> GetPagedShirts(QueryPagedShirtsRequest request);   
        Task<PagedResultResponse<GetShirtInPagingResultModel>> GetCachedPagedShirts(QueryPagedShirtsRequest request);
        Task<ShirtDetailModel> GetShirtDetailById(int id);
        Task<CreateShirtResponse> AddShirt(CreateShirtRequest shirt, ClaimsPrincipal user);
        Task DeleteShirt(int id);
        Task UpdateShirt(int id, UpdateShirtRequest request, ClaimsPrincipal claims);

    }
}
