using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TSport.Api.Models.RequestModels.Club;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.DataAccess.Interfaces
{
    public interface IClubRepository :IGenericRepository<Club>
    {
        Task<PagedResultResponse<Club>> GetPagedClub(QueryClubRequest queryPagedClubDto);
        Task<Club?> GetClubDetailById(int id);
        Task<List<Club>> GetAll();

    }
}
