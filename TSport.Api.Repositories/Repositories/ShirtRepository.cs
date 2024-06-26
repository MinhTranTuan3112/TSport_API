using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Extensions;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class ShirtRepository : GenericRepository<Shirt>, IShirtRepository
    {
        private readonly TsportDbContext _context;
        public ShirtRepository(TsportDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResultResponse<Shirt>> GetPagedShirts(QueryPagedShirtsRequest request)
        {

            int pageNumber = request.PageNumber;
            int pageSize = request.PageSize;
            string sortColumn = request.SortColumn;
            bool sortByDesc = request.OrderByDesc;

            //Query
            var query = _context.Shirts
                                   .AsNoTracking()
                                   .Include(s => s.Images)
                                   .Include(s => s.ShirtEdition)
                                    .AsQueryable();

            //Filter
            // if (request.QueryShirtRequest is not null)
            // {
            // }
            query = query.ApplyPagedShirtsFilter(request);

            //Sort
            query = sortByDesc ? query.OrderByDescending(GetSortProperty(sortColumn))
                              : query.OrderBy(GetSortProperty(sortColumn));


            //Paging
            return await query.ToPagedResultResponseAsync(pageNumber, pageSize);

        }

        private Expression<Func<Shirt, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "code" => shirt => (shirt.Code == null) ? shirt.Id : shirt.Code,
                "description" => shirt => (shirt.Description == null) ? shirt.Id : shirt.Description,
                "status" => shirt => (shirt.Status == null) ? shirt.Id : shirt.Status,
                "createddate" => shirt => shirt.CreatedDate,
                _ => shirt => shirt.Id,
            };
        }

        public async Task<Shirt?> GetShirtDetailById(int id)
        {
            var shirt = await _context.Shirts
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Include(s => s.ShirtEdition)
                .Include(s => s.SeasonPlayer)
                    .ThenInclude(sp => sp.Player)
                .Include(s => s.SeasonPlayer)
                    .ThenInclude(sp => sp.Season)
                        .ThenInclude(se => se.Club)
                .Include(s => s.CreatedAccount)
                .Include(s => s.OrderDetails)
                .Include(s => s.Images)
                .AsSplitQuery()
                .SingleOrDefaultAsync();

            return shirt;
        }
    }
}

