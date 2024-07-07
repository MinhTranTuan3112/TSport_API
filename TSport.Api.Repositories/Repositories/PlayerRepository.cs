using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TSport.Api.Models.RequestModels.Player;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Extensions;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        private readonly TsportDbContext _context;
        public PlayerRepository(TsportDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Player>> GetAll()
        {
            /*return _context.Players.ToList().Ad;*/
            return await _context.Players.ToListAsync();
        
        }

        public async Task<PagedResultResponse<Player>> GetPagedPlayers(QueryPagedPlayersRequest request)
        {
            var query = _context.Players
                            .AsQueryable();

            //Filter
            query = query.ApplyPagedPlayersFilter(request);

            //Sort
            query = request.OrderByDesc ? query.OrderByDescending(GetSortProperty(request.SortColumn))
                                        : query.OrderBy(GetSortProperty(request.SortColumn));


            //Paging
            return await query.ToPagedResultResponseAsync(request.PageNumber, request.PageSize);
        }

        public async Task<Player?> GetPlayerDetailsById(int id)
        {
            return await _context.Players.AsNoTracking()
                                        .Include(p => p.Club)
                                        .Include(p => p.CreatedAccount)
                                        .Include(p => p.ModifiedAccount)
                                        .Include(p => p.SeasonPlayers)
                                        .AsSplitQuery()
                                        .SingleOrDefaultAsync(p => p.Id == id);
        }

        private Expression<Func<Player, object>> GetSortProperty(string sortColumn)

        => sortColumn.ToLower() switch
        {
            "code" => player => (player.Code != null) ? player.Code : player.Id,
            "name" => player => (player.Name != null) ? player.Name : player.Id,
            "status" => player => (player.Status != null) ? player.Status : player.Id,
            "clubid" => player => (player.ClubId != null) ? player.ClubId : player.Id,
            _ => player => player.Id
        };
    }
}