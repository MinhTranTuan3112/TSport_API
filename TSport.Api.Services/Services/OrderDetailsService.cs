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


        public async Task cAddToCart(int Userid, OrderDetail OrderDetails)
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
            }
            int totalOrderDetails = await _unitOfWork.OrderDetailsRepository.TotalOrderDetails();

            // Tạo mã cho OrderDetail mới, ví dụ: "OD001", "OD002", ...
            string newOrderDetailCode = "OD" + (totalOrderDetails + 1).ToString("D3");


            decimal? pricePerProduct = await _unitOfWork.OrderDetailsRepository.getDiscountPrice(OrderDetails.ShirtId);


            decimal? subtotal = pricePerProduct * OrderDetails.Quantity;

            var AddShirt = new OrderDetail
            {
                Code = newOrderDetailCode,
                OrderId = Userid,
                ShirtId = OrderDetails.ShirtId,//FE tra ve
                Quantity = OrderDetails.Quantity, //Fe tra ve
                Subtotal = subtotal,
                Status = "Pending"
            };
            _unitOfWork.OrderDetailsRepository.AddAsync(AddShirt);





        }
    }
}
