using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels;
using TSport.Api.Models.ResponseModels.Shirt;

namespace TSport.Api.Services.Interfaces
{
    public interface IShirtService
    {
        Task<CreateShirtResponse> AddShirt(CreateShirtRequest createShirtRequest, ClaimsPrincipal user);
    }
}
