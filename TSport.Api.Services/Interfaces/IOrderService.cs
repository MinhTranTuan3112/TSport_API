﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Models.ResponseModels.Cart;
using TSport.Api.Models.ResponseModels.Order;

namespace TSport.Api.Services.Interfaces
{
    public interface IOrderService
    {
        Task<CartResponse> GetCartInfo(int id );

        Task<OrderResponse> GetOrderByIdAsync(int orderId);

        Task<bool> CreateOrderAsync(int orderId);
    }
}
