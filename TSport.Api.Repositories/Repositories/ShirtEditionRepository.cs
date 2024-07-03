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
        public  async Task<PagedResultResponse<ShirtEdition>> GetPagedShirtsEdition(QueryPagedShirtEditionRequest request)
        {
            int pageNumber = request.PageNumber;
            int pageSize = request.PageSize;
            string sortColumn = request.SortColumn;
            bool sortByDesc = request.OrderByDesc;

            var query = _context.ShirtEditions
                            .AsQueryable();

            //Filter
            query = query.ApplyPagedShirtEditionFilterFilter(request);

            //Sort
            query = sortByDesc  ? query.OrderByDescending(GetSortProperty(sortColumn))
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
        public async Task<bool> CheckSeasonID(int seasonID)
        {
            var result = await _context.SeasonPlayers.AnyAsync(p=>p.SeasonId == seasonID);
            if (result)
            {
                return true;
            }
            else return false;
        }
      
            public  async Task<List<ShirtEdition>> GetAll()
        {
          return  await _context.ShirtEditions.ToListAsync();
        }

        public async Task<ShirtEdition> getShirtEditionbyId(int id)
        {
            return  await _context.ShirtEditions.FirstOrDefaultAsync(p=>p.Id == id);
        }

    }
}
