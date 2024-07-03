using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.ResponseModels.Order;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels.Cart;
using TSport.Api.Services.Interfaces;
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
                throw new BadRequestException("Empty cart");
            }

            return order.Adapt<OrderCartResponse>();
            
        }

        public async Task<OrderResponse> GetOrderByIdAsync(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }

            return new OrderResponse
            {
                Id = order.Id,
                Code = order.Code,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Total = order.Total,
                CreatedAccountId = order.CreatedAccountId,
                CreatedDate = order.CreatedDate,
                ModifiedDate = order.ModifiedDate,
                ModifiedAccountId = order.ModifiedAccountId
            };
        }

        public async Task<bool> CreateOrderAsync(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null || order.Status != "InCart")
            {
                return false;
            }

            order.Status = "Created";
            _unitOfWork.OrderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /*public async Task<GetShirtDetailResponse> GetShirtDetailById(int id)
        {
            return (await _unitOfWork.ShirtRepository.GetShirtDetailById(id)).Adapt<GetShirtDetailResponse>();
        }*/
    }
}
