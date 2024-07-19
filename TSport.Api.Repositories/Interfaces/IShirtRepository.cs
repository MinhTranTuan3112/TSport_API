using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Repositories.Interfaces
{
    public interface IShirtRepository : IGenericRepository<Shirt>
    {
        Task<PagedResultResponse<Shirt>> GetPagedShirts(QueryPagedShirtsRequest request);   
        Task<Shirt?> GetShirtDetailById(int id);

        Task<Shirt?> GetShirtWithShirtEditionById(int id);

        Task<List<Shirt>> getListShirtbaseOrderId(int id);
        Task<List<Shirt>> GetShirtDetailsByShirtIdsAsync(List<int> shirtIds);

    }
}
  
