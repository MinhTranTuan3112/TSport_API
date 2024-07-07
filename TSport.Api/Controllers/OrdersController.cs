using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels;
using TSport.Api.Models.RequestModels.Order;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Services.BusinessModels.Cart;
using TSport.Api.Services.BusinessModels.Order;
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

        [HttpGet]
        [SupabaseAuthorize]
        public async Task<ActionResult<PagedResultResponse<OrderModel>>> GetOrders([FromQuery] QueryPagedOrderRequest request)
        {
            return await _serviceFactory.OrderService.GetPagedOrders(request);
        }

        [HttpGet("{id}")]
        [SupabaseAuthorize]
        public async Task<ActionResult<OrderDetailsInfoModel>> GetOrderDetailsInfoById(int id)
        {
            return await _serviceFactory.OrderService.GetOrderDetailsInfoById(id);
        }

        [HttpGet("get-cart")]
        [SupabaseAuthorize(Roles = ["Customer"])]
        public async Task<ActionResult<OrderCartResponse>> GetCartInfo()
        {
            return await _serviceFactory.OrderService.GetCartInfo(HttpContext.User);
        }

        [HttpPost("add-to-cart")]
        [SupabaseAuthorize(Roles = ["Customer"])]
        public async Task<ActionResult> AddtoCart([FromBody] List<AddToCartRequest> request)
        {
            foreach(var item in request)
            {
                await _serviceFactory.OrderDetailsService.AddToCart(HttpContext.User, item.ShirtId, item.Quantity, item.Size);
            }
            return Ok();
        }

        [HttpPost("{orderId}")]
        [SupabaseAuthorize(Roles = ["Customer"])]
        public async Task<IActionResult> CreateOrder(int orderId,[FromQuery] List<int> shirtIds)
        {
            await _serviceFactory.OrderService.ConfirmOrder(HttpContext.User, orderId, shirtIds);
            return Ok();
        }
    }
}

