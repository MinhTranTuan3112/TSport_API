using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TSport.Api.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrder(int orderId, ClaimsPrincipal claims);
    }
}