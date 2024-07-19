using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.RequestModels.ShirtEdition;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Repositories.Interfaces
{
    public interface IShirtEditionRepository : IGenericRepository<ShirtEdition> 
    {
        
        Task<PagedResultResponse<ShirtEdition>> GetPagedShirtsEdition(QueryPagedShirtEditionRequest request);
        
        Task<ShirtEdition?> GetShirtEditionById(int id);

        Task<List<ShirtEdition>> GetShirtEditionsByIdsAsync(List<int> shirtEditionIds);

    }
}
