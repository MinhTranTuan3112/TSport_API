using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Season?> GetSeasonDetailsById(int id)
        {
            return await _context.Seasons.AsNoTracking()
                                           .Include(s => s.Club)
                                           .Include(s => s.CreatedAccount)
                                           .Include(s => s.ModifiedAccount)
                                           .AsSplitQuery()
                                           .SingleOrDefaultAsync(s => s.Id == id);
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

        public void Update(Season season)
        {
            _context.Seasons.Update(season);
        }

        public async Task<List<Season>> GetAll()
        {
            return await _context.Seasons.ToListAsync();
        }

        public async Task<List<Season>> GetSeasonsByIdsAsync(List<int> seasonIds)
        {
            return await _context.Seasons
            .Where(sn => seasonIds.Contains(sn.Id))
            .ToListAsync();
        }
    }
}