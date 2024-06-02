using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TSport.Api.Repositories.Interfaces;
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

        public async Task CreateOrder(int orderId, ClaimsPrincipal claims)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderInfoWithCustomerInfo(orderId);
            if (order is null)
            {
                throw new NotFoundException($"Can't find any orders with id {orderId}");
            }

            if (order.Status != OrderStatus.InCart.ToString())
            {
                throw new BadRequestException("Can't create order with this status");
            }

            var account = await _serviceFactory.AuthService.GetAuthAccountInfo(claims);
            if (account is null || account.Role != Role.Customer.ToString())
            {
                throw new UnauthorizedException("Unauthorized");    
            }

            if (order.CreatedAccountId != account.Id)
            {
                throw new BadRequestException($"This customer didn't have any order with id {orderId}");
            }

            









        }
    }
}