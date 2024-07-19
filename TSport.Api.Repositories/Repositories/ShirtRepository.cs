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
using TSport.Api.Shared.Enums;

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
                                   .Where(s => s.Status != ShirtStatus.Deleted.ToString())
                                   .Include(s => s.Images)
                                   .Include(s => s.ShirtEdition)
                                   .Include(s => s.SeasonPlayer)
                                       .ThenInclude(se => se.Season)
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
                "price" => shirt => (shirt.ShirtEdition != null && shirt.ShirtEdition.DiscountPrice != null)
                ? shirt.ShirtEdition.DiscountPrice : shirt.Id,
                "createddate" => shirt => shirt.CreatedDate,
                _ => shirt => shirt.Id,
            };
        }

        public async Task<Shirt?> GetShirtDetailById(int id)
        {
            var shirt = await _context.Shirts
                .Where(s => s.Status != ShirtStatus.Deleted.ToString())
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

        public async Task<Shirt?> GetShirtWithShirtEditionById(int id)
        {
            return await _context.Shirts.Where(s => s.Id == id && s.Status != ShirtStatus.Deleted.ToString())
                 .Include(s => s.ShirtEdition)
                 .SingleOrDefaultAsync();
        }

        public async Task<List<Shirt>> getListShirtbaseOrderId(int id)
        {
            var shirtEditions = await _context.Shirts
                                         .Where(se => se.Id == id)
                                         .ToListAsync();

            return shirtEditions;

        }

        public async Task<List<Shirt>> GetShirtDetailsByShirtIdsAsync(List<int> shirtIds)
        {
            return await _context.Shirts
              .Where(s => shirtIds.Contains(s.Id))
              .ToListAsync();
        }
    }
}

