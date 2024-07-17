using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;
using TSport.Api.Repositories.Entities;
using System.Security.Claims;
using TSport.Api.Shared.Exceptions;
using TSport.Api.Shared.Enums;
using TSport.Api.Models.ResponseModels.Order;
using Mapster;

namespace TSport.Api.Services.Services
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task AddToCart(ClaimsPrincipal claims, int shirtId, int quantity, string size)
        {
            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var supabaseId = "580b1b9e-c395-467c-a4e8-ce48c0ec09d1";
            if (supabaseId is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }

            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId.Equals(supabaseId));
            if (account is null)
            {
                throw new NotFoundException("Account not found");
            }

            var order = await _unitOfWork.OrderRepository.FindOneAsync(o => o.CreatedAccountId == account.Id && o.Status == OrderStatus.InCart.ToString());

            if (order is null)
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
            }

            var shirt = await _unitOfWork.ShirtRepository.GetShirtWithShirtEditionById(shirtId);

            if (shirt is null)
            {
                throw new NotFoundException("Shirt not found");
            }

            await UpsertCart(shirtId, quantity, size, order, shirt);

        }

        public async Task<List<OrderDetailsReponse>> GetList()
        {
            //return await _unitOfWork.OrderDetailsRepository.GetAllAsync().Adapt<List<OrderDetailsReponse>>();

            var orderDetails = await _unitOfWork.OrderDetailsRepository.GetAllAsync();
            var orderDetailResponses = orderDetails.Adapt<List<OrderDetailsReponse>>();
            return orderDetailResponses;
        }

        private async Task UpsertCart(int shirtId, int quantity, string size, Order order, Shirt shirt)
        {
            var orderDetail = await _unitOfWork.OrderDetailsRepository.FindOneAsync(od => od.OrderId == order.Id && od.ShirtId == shirtId);

            if (orderDetail is null)
            {

                orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ShirtId = shirtId,
                    Code = shirt.Code,
                    Quantity = quantity,
                    Size = size,
                    Subtotal = (shirt.ShirtEdition is not null &&
                    shirt.ShirtEdition.DiscountPrice.HasValue) ?
                    shirt.ShirtEdition.DiscountPrice.Value * quantity :
                    shirt.ShirtEdition!.StockPrice * quantity,
                    Status = OrderStatus.InCart.ToString()
                };

                await _unitOfWork.OrderDetailsRepository.AddAsync(orderDetail);
            }
            else
            {
                orderDetail.Quantity += quantity;
                orderDetail.Subtotal += (shirt.ShirtEdition is not null &&
                    shirt.ShirtEdition.DiscountPrice.HasValue) ? shirt.ShirtEdition.DiscountPrice.Value * quantity : shirt.ShirtEdition!.StockPrice * quantity;

            }

            order.Total += orderDetail.Subtotal;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
