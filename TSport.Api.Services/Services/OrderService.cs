using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.Entities;
using TSport.Api.Models.ResponseModels.Cart;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;

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


     

        public async Task<CartResponse> GetCartInfo(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetCartByID(id);



            var response = new CartResponse
            {
                OrderId = order.Id,
                CreatedAccountId = order.CreatedAccountId,
                ShirtId = order.OrderDetails.FirstOrDefault().ShirtId,
                Code = order.Code,
                Subtotal = order.Total,
                OrderDate = order.OrderDate,
                Status = order.Status,
                OrderDetail = order.OrderDetails.Select(od =>
                {

                    return new CartItemResponse
                    {
                        OrderId = od.OrderId,
                        ShirtId = od.ShirtId,
                        Quantity = od.Quantity,
                        Status = od.Status,
                        Subtotal = od.Subtotal,
                        Size = od.Shirt.ShirtEdition.Size,
                        HasSignature = od.Shirt.ShirtEdition.HasSignature,
                        Price = od.Shirt.ShirtEdition.StockPrice,
                        Color = od.Shirt.ShirtEdition.Color,
                        SeasonPlayerId = od.Shirt.SeasonPlayer.PlayerId,
                        PlayerName = od.Shirt.SeasonPlayer.Player.Name,
                        ClubId = od.Shirt.SeasonPlayer.Player.ClubId,
                        ClubName = od.Shirt.SeasonPlayer.Player.Club.Name,
                        SeasonId = od.Shirt.ShirtEdition.Season.Id,
                        SeasonName = od.Shirt.ShirtEdition.Season.Name


                    };
                }).ToList()
            };


            return response;

            // return cart.Adapt<CartResponse>();
        }
        /*public async Task<GetShirtDetailResponse> GetShirtDetailById(int id)
        {
            return (await _unitOfWork.ShirtRepository.GetShirtDetailById(id)).Adapt<GetShirtDetailResponse>();
        }*/
    }
}
