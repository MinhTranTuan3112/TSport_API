using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels;
using TSport.Api.Services.BusinessModels.Cart;
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
        [SupabaseAuthorize(Roles = ["Customer"])]
        public async Task<ActionResult<OrderCartResponse>> GetCartInfo()
        {
            return await _serviceFactory.OrderService.GetCartInfo(HttpContext.User);
        }

        [HttpPost("add-to-cart")]
        [SupabaseAuthorize(Roles = ["Customer"])]
        public async Task<ActionResult> AddtoCart([FromBody] AddToCartRequest request)
        {
            await _serviceFactory.OrderDetailsService.AddToCart(HttpContext.User, request.ShirtId, request.Quantity);
            return Ok();
        }

        [HttpPost("{orderId}")]
        [SupabaseAuthorize(Roles = ["Customer"])]
        public async Task<IActionResult> CreateOrder(int orderId)
        {
            await _serviceFactory.OrderService.ConfirmOrder(HttpContext.User, orderId);
            return Ok();
        }
    }
}

