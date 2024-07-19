using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TSport.Api.Models.RequestModels.Player;
using TSport.Api.Models.RequestModels.ShirtEdition;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Extensions;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class ShirtEditionRepository : GenericRepository<ShirtEdition>, IShirtEditionRepository
    {
        private readonly TsportDbContext _context;
        public ShirtEditionRepository(TsportDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<PagedResultResponse<ShirtEdition>> GetPagedShirtsEdition(QueryPagedShirtEditionRequest request)
        {
            int pageNumber = request.PageNumber;
            int pageSize = request.PageSize;
            string sortColumn = request.SortColumn;
            bool sortByDesc = request.OrderByDesc;

            var query = _context.ShirtEditions.AsQueryable();

            //Filter
            query = query.ApplyPagedShirtEditionFilterFilter(request);

            //Sort
            query = sortByDesc ? query.OrderByDescending(GetSortProperty(sortColumn))
                                        : query.OrderBy(GetSortProperty(sortColumn));


            //Paging
            return await query.ToPagedResultResponseAsync(pageNumber, pageSize);
        }
        private Expression<Func<ShirtEdition, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "code" => shirtEdition => shirtEdition.Code != null ? (object)shirtEdition.Code : shirtEdition.Id,
                "size" => shirtEdition => shirtEdition.Size != null ? (object)shirtEdition.Size : shirtEdition.Id,
                "price" => shirtEdition => shirtEdition.StockPrice != null ? (object)shirtEdition.StockPrice : (object)shirtEdition.Id,
                _ => shirtEdition => shirtEdition.Id
            };
        }


        public async Task<ShirtEdition?> GetShirtEditionById(int id)
        {
            return await _context.ShirtEditions
                                   .AsNoTracking()
                                   .Include(se => se.CreatedAccount)
                                   .Include(se => se.ModifiedAccount)
                                   .Include(se => se.Season)
                                   .Include(se => se.Shirts)
                                   .AsSplitQuery()
                                    .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ShirtEdition?>> GetShirtBaseOrder(int id)
        {
            var shirtEditions = await _context.ShirtEditions
                                         .Where(se => se.Id == id)
                                         .ToListAsync();

            return shirtEditions;

        }

        public async Task<List<ShirtEdition>> GetShirtEditionsByIdsAsync(List<int> shirtEditionIds)
        {
            return await _context.ShirtEditions
           .Where(se => shirtEditionIds.Contains(se.Id))
                    .ToListAsync();
        }
    }
}
