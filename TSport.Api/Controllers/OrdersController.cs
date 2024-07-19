using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels;
using TSport.Api.Models.RequestModels.Order;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Order;
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

        [HttpGet]
      //  [SupabaseAuthorize]
        [Route("get-total-order")]

        public async Task<ActionResult<int>> GetTotalOrder()
        {
            return await _serviceFactory.OrderService.GetTotalOrder();
        }

        [HttpGet]
      //  [SupabaseAuthorize]
        [Route("get-monthly-revenue-by-year-month")]

        public async Task<ActionResult<decimal>> GetMonthlyRevenue([FromQuery] int year, int month)
        {
            return await _serviceFactory.OrderService.GetMonthlyRevenue(year, month);
        }

        [HttpGet]
       // [SupabaseAuthorize]
        [Route("get-monthly-revenue-now")]

        public async Task<ActionResult<decimal>> GetMonthlyRevenueNow()
        {
            return await _serviceFactory.OrderService.GetMonthlyRevenueNow();
        }

        [HttpGet]
      //  [SupabaseAuthorize]
        [Route("get-all-orderdetails")]

        public async Task<ActionResult<List<OrderDetailsReponse>>> GetList()
        {
            return await _serviceFactory.OrderDetailsService.GetList();
        }

        [HttpGet("{id}")]
        //[SupabaseAuthorize]
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
        public async Task<ActionResult> AddtoCart([FromBody] AddToCartRequest request)
        {

            await _serviceFactory.OrderDetailsService.AddToCart(HttpContext.User, request.ShirtId, request.Quantity, request.Size);
            return Ok();
        }

        [HttpPost("{orderId}")]
        [SupabaseAuthorize(Roles = ["Customer"])]
        public async Task<IActionResult> CreateOrder(int orderId, [FromBody] List<AddToCartRequest> shirts)
        {
            await _serviceFactory.OrderService.ConfirmOrder(HttpContext.User, orderId, shirts);
            return Ok();
        }

        [HttpPatch("{orderId}")]
        [SupabaseAuthorize(Roles = ["Staff", "Customer"])]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            await _serviceFactory.OrderService.CancelOrder(HttpContext.User, orderId);
            return Ok();
        }

        [HttpPut("{orderId}")]
        [SupabaseAuthorize(Roles = ["Customer"])]
        public async Task<IActionResult> UpdateOrderDetail(int orderId, [FromBody] AddToCartRequest request)
        {
            await _serviceFactory.OrderService.UpdateOrderDetail(HttpContext.User, orderId, request);
            return Ok();
        }

        [HttpDelete("{orderId}/shirts/{shirtId}")]
        [SupabaseAuthorize(Roles = ["Customer"])]
        public async Task<IActionResult> DeleteShirtFromCart(int orderId, int shirtId)
        {
            await _serviceFactory.OrderService.DeleteShirtFromCart(HttpContext.User, orderId, shirtId);
            return Ok();
        }

        [HttpDelete("{orderId}")]
        [SupabaseAuthorize(Roles = ["Staff", "Customer"])]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            await _serviceFactory.OrderService.DeleteOrder(HttpContext.User, orderId);
            return NoContent();
        }
        [HttpPatch("change-status/{orderId}")]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, string status)
        {
            await _serviceFactory.OrderService.ChangeOrderStatus(orderId, status);
            return NoContent();
        }

        [HttpGet]
        [Route("club-order-report")]
        [SupabaseAuthorize]
        public async Task<ActionResult<ClubOrderReportResponse>> GetClubOrderReport([FromQuery] List<int> clubIds)
        {
            return await _serviceFactory.OrderService.GetClubOrderReport(clubIds);
        }


    }
}

