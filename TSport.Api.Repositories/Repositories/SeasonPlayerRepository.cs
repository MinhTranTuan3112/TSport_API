using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class SeasonPlayerRepository : GenericRepository<SeasonPlayer>, ISeasonPlayerRepository
    {
        public SeasonPlayerRepository(TsportDbContext context) : base(context)
        {
        }
    }
}