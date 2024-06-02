using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.ResponseModels.Shirt;

namespace TSport.Api.Services.Interfaces
{
    public interface IShirtService
    {
        Task<GetShirtDetailResponse> GetShirtDetailById(int id);
    }
}
