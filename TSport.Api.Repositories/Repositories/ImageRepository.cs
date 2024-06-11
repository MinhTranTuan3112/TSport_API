using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        private readonly TsportDbContext _context;
        public ImageRepository(TsportDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
