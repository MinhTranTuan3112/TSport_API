using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Extensions;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class SeasonRepository : GenericRepository<Season>, ISeasonRepository
    {
        private readonly TsportDbContext _context;

        public SeasonRepository(TsportDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResultResponse<Season>> GetPagedSeasons(QueryPagedSeasonRequest request)
        {
            //Query
            var query = _context.Seasons.AsQueryable();

            //Filter
            query = query.ApplyPagedSeasonsFilter(request);


            //Sort
            query = request.OrderByDesc ? query.OrderByDescending(GetSortProperty(request.SortColumn))
                                        : query.OrderBy(GetSortProperty(request.SortColumn));


            //Paging
            return await query.ToPagedResultResponseAsync(request.PageNumber, request.PageSize);
        }

        private Expression<Func<Season, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "code" => season => (season.Code != null) ? season.Code : season.Id,
                "name" => season => (season.Name != null) ? season.Name : season.Id,
                _ => season => season.Id
            };
        }


    }
}