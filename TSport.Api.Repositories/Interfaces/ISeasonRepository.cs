using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Repositories.Interfaces
{
    public interface ISeasonRepository : IGenericRepository<Season>
    {
        Task<PagedResultResponse<Season>> GetPagedSeasons(QueryPagedSeasonRequest request);

        Task<Season?> GetSeasonDetailsById(int id);

        void Update(Season season);
        Task<List<Season>> GetAll();
        Task<List<Season>> GetSeasonsByIdsAsync(List<int> seasonIds);


    }
}