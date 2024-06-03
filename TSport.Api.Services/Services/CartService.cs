using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.ResponseModels.Cart;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Services.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;

        public CartService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
        }

   

        public async Task<CartResponse> GetCartInfo(int id)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByID(id);
            return cart.Adapt<CartResponse>();
        }
        /*public async Task<GetShirtDetailResponse> GetShirtDetailById(int id)
        {
            return (await _unitOfWork.ShirtRepository.GetShirtDetailById(id)).Adapt<GetShirtDetailResponse>();
        }*/
    }
}
