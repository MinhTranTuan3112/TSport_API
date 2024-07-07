using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.DataAccess.Interfaces;

using TSport.Api.Models.RequestModels.Club;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Extensions;
using TSport.Api.Shared.Enums;

namespace TSport.Api.Repositories.Repositories
{
    public class ClubRepository : GenericRepository<Club>, IClubRepository
    {
        private readonly TsportDbContext _context;

        public ClubRepository(TsportDbContext context) : base(context)
        {
            _context = context;
        }


        private Expression<Func<Club, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "code" => shirt => shirt.Code == null ? shirt.Id : shirt.Code,
                "status" => shirt => shirt.Status == null ? shirt.Id : shirt.Status,
                "createddate" => shirt => shirt.CreatedDate,
                _ => shirt => shirt.Id,
            };
        }

        public async Task<PagedResultResponse<Club>> GetPagedClub(QueryClubRequest queryPagedClubDto)
        {
            int pageNumber = queryPagedClubDto.PageNumber;
            int pageSize = queryPagedClubDto.PageSize;
            string sortColumn = queryPagedClubDto.SortColumn;
            bool sortByDesc = queryPagedClubDto.OrderByDesc;

            var query = _context.Clubs
                    .AsNoTracking()
                    .Where(c => c.Status != ShirtStatus.Deleted.ToString())
                    .Include(s => s.Seasons)
                    .AsQueryable();

            if (queryPagedClubDto.ClubRequest is not null)
            {
                query = query.ApplyPagedClubFilter(queryPagedClubDto);
            }

            //Sort
            query = sortByDesc ? query.OrderByDescending(GetSortProperty(sortColumn))
                                : query.OrderBy(GetSortProperty(sortColumn));


            //Paging
            return await query.ToPagedResultResponseAsync(pageNumber, pageSize);
        }

        public async Task<Club?> GetClubDetailById(int id)
        {
            var club = await _context.Clubs.AsNoTracking()
                                            .Include(s => s.Seasons)
                                            .Include(s => s.Players)
                                            .SingleOrDefaultAsync(s => s.Id == id);


            return club;
        }

        public async Task<List<Club>> GetAll()
        {
            /*return _context.Players.ToList().Ad;*/
            return await _context.Clubs.ToListAsync();
        
        }
    }
}
