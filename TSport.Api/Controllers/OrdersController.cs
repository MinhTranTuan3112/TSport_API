using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels;
using TSport.Api.Models.ResponseModels.Auth;
using TSport.Api.Models.ResponseModels.Cart;
using TSport.Api.Repositories.Interfaces;
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
        
        [HttpGet("get-cart")]
        public async Task<ActionResult<CartResponse>> GetCartbyId(int userid)
        {
            return await _serviceFactory.OrderService.GetCartInfo(userid);
        }

        [HttpPost("add-to-cart")]
        public async Task<ActionResult> AddtoCart([FromBody] AddToCartRequest request)
        {
            await _serviceFactory.OrderDetailsService.AddToCart(request.UserId, request.ShirtId, request.Quantity.Value);
            return Ok();
        }
    }
}

