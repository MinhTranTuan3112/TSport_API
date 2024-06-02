using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public OrdersController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        
        [HttpPost("{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> CreateOrder(int orderId)
        {
            
            return Created();
        }
    }
}