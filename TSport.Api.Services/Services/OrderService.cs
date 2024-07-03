﻿using Mapster;
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
                throw new BadRequestException("Empty cart");
            }

            return order.Adapt<OrderCartResponse>();
            
        }

        public async Task ConfirmOrder(ClaimsPrincipal claims, int orderId)
        {
            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
            await _unitOfWork.SaveChangesAsync();

        }

    }
}
