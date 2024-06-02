using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.Entities;

namespace TSport.Api.Repositories.Interfaces
{
    public interface IShirtRepository : IGenericRepository<Shirt>
    {
        Task<Shirt?> GetShirtDetailById(int id);
    }
}
