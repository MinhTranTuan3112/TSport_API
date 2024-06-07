using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Services.BusinessModels;
using TSport.Api.Services.BusinessModels.Shirt;

namespace TSport.Api.Services.Interfaces
{
    public interface IShirtService
    {
        Task<PagedResultResponse<GetShirtModel>> GetPagedShirts(QueryPagedShirtsRequest request);   
        Task<ShirtDetailModel> GetShirtDetailById(int id);
    }
}
