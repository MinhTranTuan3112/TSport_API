using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Services.Services
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;

        public OrderDetailsService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
        }




    
        public async Task AddToCart(int Userid, int ShirtId, int quantity)
        {
            var ExsitingCart = await _unitOfWork.OrderDetailsRepository.ExistingCart(Userid);

            if (ExsitingCart == false)
            {
                //Cart not yet having -> create 
                var newCart = new Order
                {
                    Code = "ORD000",
                    OrderDate = DateTime.Now,
                    Status = "InCart",
                    CreatedAccountId = Userid,
                    CreatedDate = DateTime.Now,
                    Total = 0,

                };
                _unitOfWork.OrderRepository.AddAsync(newCart);
                _unitOfWork.SaveChangesAsync();
            }
            int totalOrderDetails = await _unitOfWork.OrderDetailsRepository.TotalOrderDetails();

            // Tạo mã cho OrderDetail mới, ví dụ: "OD001", "OD002", ...
            string newOrderDetailCode = "OD" + (totalOrderDetails + 1).ToString("D3");


            decimal? pricePerProduct = await _unitOfWork.OrderDetailsRepository.getDiscountPrice(ShirtId);


            decimal? subtotal = pricePerProduct * quantity;

            var AddShirt = new OrderDetail
            {
                Code = newOrderDetailCode,
                OrderId = Userid,
                ShirtId = ShirtId,//FE tra ve
                Quantity = quantity, //Fe tra ve
                Subtotal = subtotal,
                Status = "Pending"
            };
            _unitOfWork.OrderDetailsRepository.AddAsync(AddShirt);

            var currentOrder = await _unitOfWork.OrderRepository.GetCartByID(Userid);
            if (currentOrder == null) {
                throw new Exception("Can  not found Account");
            
            }
            currentOrder.Total += AddShirt.Subtotal;

            _unitOfWork.OrderRepository.UpdateAsync(currentOrder);

            _unitOfWork.SaveChangesAsync();





        }
    }
}
