using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Order;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Order;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels.Cart;
using TSport.Api.Services.BusinessModels.Order;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Enums;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;

        public OrderService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
        }

        public async Task<OrderCartResponse> GetCartInfo(ClaimsPrincipal claims)
        {
            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var account = await _serviceFactory.AccountService.GetAccountBySupabaseId(supabaseId!);
            if (account is null)
            {
                throw new UnauthorizedException("Account not found");
            }

            var order = await _unitOfWork.OrderRepository.GetCustomerCartInfo(account.Id);
            if (order is null)
            {
                throw new NotFoundException("Empty cart");
            }

            return order.Adapt<OrderCartResponse>();

        }

        public async Task ConfirmOrder(ClaimsPrincipal claims, int orderId, List<int> shirtIds)
        {
            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var supabaseId = "580b1b9e-c395-467c-a4e8-ce48c0ec09d1"; // data for testing
            if (supabaseId is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }

            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);

            if (account is null)
            {
                throw new UnauthorizedException("Account not found");
            }

            // Check if the order exists
            var order = await _unitOfWork.OrderRepository.FindOneAsync(o => o.Id == orderId);
            if (order is null)
            {
                throw new NotFoundException("Order not found");
            }

            if (order.CreatedAccountId != account.Id)
            {
                throw new BadRequestException("The order does not belong to the customer.");
            }

            // Check if the order status is InCart
            if (order.Status != "InCart")
            {
                throw new BadRequestException("The order status must be 'InCart'.");
            }

            // Create the order
            order.Status = OrderStatus.Pending.ToString();

            foreach (var shirtId in shirtIds)
            {
                var orderDetail = await _unitOfWork.OrderDetailsRepository.FindOneAsync(o => o.OrderId == orderId && o.ShirtId == shirtId);
                if(orderDetail is null || (orderDetail.Status != null && orderDetail.Status.Equals(OrderStatus.InCart.ToString())))
                {
                    continue;
                }
                orderDetail.Status = OrderStatus.Pending.ToString();
            }
            await _unitOfWork.SaveChangesAsync();

            var remainOrderDetails = await _unitOfWork.OrderDetailsRepository.FindAsync(o => o.Status != null && o.Status.Equals(OrderStatus.InCart.ToString()));
            if (remainOrderDetails is not null)
            {
                order = new Order
                {
                    Code = $"OD{Guid.NewGuid().ToString()}",
                    CreatedAccountId = account.Id,
                    Status = OrderStatus.InCart.ToString(),
                    Total = 0,
                    CreatedDate = DateTime.Now,
                    OrderDate = DateTime.Now
                };

                await _unitOfWork.OrderRepository.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

                foreach (var item in remainOrderDetails)
                {
                    await _unitOfWork.OrderDetailsRepository.DeleteAsync(item);
                }
            }

            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<PagedResultResponse<OrderModel>> GetPagedOrders(QueryPagedOrderRequest request)
        {
            return (await _unitOfWork.OrderRepository.GetPagedOrders(request)).Adapt<PagedResultResponse<OrderModel>>();
        }

        public async Task<OrderDetailsInfoModel> GetOrderDetailsInfoById(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderDetailsInfoById(orderId);

            if (order is null)
            {
                throw new NotFoundException("Order not found");
            }

            return order.Adapt<OrderDetailsInfoModel>();
        }
    }
}
