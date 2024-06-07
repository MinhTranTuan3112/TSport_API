using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Services.BusinessModels;

namespace TSport.Api.Services.Interfaces
{
    public interface IShirtService
    {
        Task<ShirtDetailModel> GetShirtDetailById(int id);
    }
}
