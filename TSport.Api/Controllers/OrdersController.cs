using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels;
using TSport.Api.Models.ResponseModels.Auth;
using TSport.Api.Repositories.Interfaces;
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
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder(int orderId)
        {
            // Check if the order exists
            var order = await _serviceFactory.OrderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            // Check if the order belongs to the customer
            var userId = int.Parse(User.FindFirst("id").Value);
            if (order.CreatedAccountId != userId)
            {
                return BadRequest("The order does not belong to the customer.");
            }

            // Check if the order status is InCart
            if (order.Status != "InCart")
            {
                return BadRequest("The order status must be 'InCart'.");
            }

            // Create the order
            var result = await _serviceFactory.OrderService.CreateOrderAsync(orderId);
            if (result)
            {
                return Ok(new { Message = "Order created successfully." });
            }

            return BadRequest("Unable to create order.");
        }
    }
}

