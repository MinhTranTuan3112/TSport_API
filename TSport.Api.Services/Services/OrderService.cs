using Mapster;
using Microsoft.EntityFrameworkCore;
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


        public async Task ConfirmOrder(ClaimsPrincipal claims, int orderId, List<AddToCartRequest> shirts)
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
            //reset order total price value to 0
            order.Total = 0;

            if (order.CreatedAccountId != account.Id)
            {
                throw new BadRequestException("The order does not belong to the customer.");
            }

            // Check if the order status is InCart
            if (order.Status != "InCart")
            {
                throw new BadRequestException("The order status must be 'InCart'.");
            }

            var orderDetails = await _unitOfWork.OrderDetailsRepository.FindAsync(o => o.Status != null && o.Status.Equals(OrderStatus.InCart.ToString()));
            if (orderDetails is [])
            {
                throw new NotFoundException("There are no items in cart.");
            }

            if (shirts is null || shirts is [])
            {
                throw new BadRequestException("There are no product confirmed for order.");
            }

            // Create the order
            order.Status = OrderStatus.Pending.ToString();

            foreach (var shirt in shirts)
            {
                var orderDetail = await _unitOfWork.OrderDetailsRepository.FindOneAsync(o => o.OrderId == orderId && o.ShirtId == shirt.ShirtId);
                var shirtDetail = await _unitOfWork.ShirtRepository.GetShirtDetailById(shirt.ShirtId);

                if (orderDetail is null || (orderDetail.Status != null && !orderDetail.Status.Equals(OrderStatus.InCart.ToString())))
                {
                    throw new BadRequestException("The shirt is not in cart.");
                }
                if (shirtDetail is null)
                {
                    throw new NotFoundException("Shirt not found.");
                }
                if (shirtDetail.Quantity < shirt.Quantity)
                {
                    throw new BadRequestException("There are not enough shirt in stock.");
                }
                // update order detail
                orderDetail.Quantity = shirt.Quantity;
                orderDetail.Size = shirt.Size;
                if (shirtDetail.ShirtEdition.DiscountPrice != null)
                {
                    orderDetail.Subtotal = (decimal)(shirt.Quantity * shirtDetail.ShirtEdition.DiscountPrice);
                }
                else
                {
                    orderDetail.Subtotal = shirt.Quantity * shirtDetail.ShirtEdition.StockPrice;
                }
                orderDetail.Status = OrderStatus.Pending.ToString();
                orderDetails.Remove(orderDetails.First(o => o.OrderId == orderDetail.OrderId && o.ShirtId == orderDetail.ShirtId));
                //reduce quantity from stock
                shirtDetail.Quantity -= shirt.Quantity;
                //recaculate total price
                order.Total += orderDetail.Subtotal;
            }
            await _unitOfWork.SaveChangesAsync();

            if (orderDetails is not [])
            {
                order = await _unitOfWork.OrderRepository.AddAsync(new Order
                {
                    Code = $"OD{Guid.NewGuid().ToString()}",
                    CreatedAccountId = account.Id,
                    Status = OrderStatus.InCart.ToString(),
                    Total = 0,
                    CreatedDate = DateTime.Now,
                    OrderDate = DateTime.Now
                });
                await _unitOfWork.SaveChangesAsync();

                foreach (var item in orderDetails)
                {
                    await _unitOfWork.OrderDetailsRepository.DeleteAsync(item);
                    await _unitOfWork.OrderDetailsRepository.AddAsync(new OrderDetail
                    {
                        OrderId = order.Id,
                        ShirtId = item.ShirtId,
                        Code = item.Code,
                        Subtotal = item.Subtotal,
                        Quantity = item.Quantity,
                        Size = item.Size,
                        Status = OrderStatus.InCart.ToString()
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();

            await QueryAndSendOrderConfirmationEmail(order.Id);
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

        public async Task CancelOrder(ClaimsPrincipal claims, int orderId)
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
            
            if (order.CreatedAccountId != account.Id && account.Role != "Staff")
            {
                throw new BadRequestException("The order does not belong to this account.");
            }

            // Check if the order status is Pending
            if (order.Status != OrderStatus.Pending.ToString())
            {
                throw new BadRequestException("The order status must be 'Pending' to cancel.");
            }

            // Cancel the order
            order.Status = OrderStatus.Cancelled.ToString();

            var orderDetails = await _unitOfWork.OrderDetailsRepository.FindAsync(o => o.OrderId == order.Id);
            if (orderDetails is not [])
            {
                foreach (var item in orderDetails)
                {
                    if (item.Status == OrderStatus.Pending.ToString())
                    {
                        item.Status = OrderStatus.Cancelled.ToString();
                    }
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateOrderDetail(ClaimsPrincipal claims, int orderId, AddToCartRequest request)
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

            // Check if the order status is InCart
            if (order.Status != OrderStatus.InCart.ToString())
            {
                throw new BadRequestException("The order status must be 'InCart'.");
            }

            // Check if the ShirtId exists in the order
            var orderDetail = await _unitOfWork.OrderDetailsRepository.FindOneAsync(od => od.OrderId == orderId && od.ShirtId == request.ShirtId);
            if (orderDetail is null)
            {
                throw new NotFoundException("Shirt not found in the order.");
            }

            // Update Quantity and Size
            orderDetail.Quantity = request.Quantity;
            orderDetail.Size = request.Size;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteShirtFromCart(ClaimsPrincipal claims, int orderId, int shirtId)
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

            // Check if the order status is InCart
            if (order.Status != OrderStatus.InCart.ToString())
            {
                throw new BadRequestException("The order status must be 'InCart'.");
            }

            // Check if the ShirtId exists in the order
            var orderDetail = await _unitOfWork.OrderDetailsRepository.FindOneAsync(od => od.OrderId == orderId && od.ShirtId == shirtId);
            if (orderDetail is null)
            {
                throw new NotFoundException("Shirt not found in the order.");
            }

            // Delete the Shirt from the Order
            await _unitOfWork.OrderDetailsRepository.DeleteAsync(orderDetail);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteOrder(ClaimsPrincipal claims, int orderId)
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

            if (account.Role.Equals("Customer"))
            {
                if (order.CreatedAccountId != account.Id)
                {
                    throw new BadRequestException("The order does not belong to this account.");
                }
            }

            var orderDetails = await _unitOfWork.OrderDetailsRepository.FindAsync(o => o.OrderId == order.Id);
            if (orderDetails is not [])
            {
                foreach (var item in orderDetails)
                {
                    // Delete order details
                    await _unitOfWork.OrderDetailsRepository.DeleteAsync(item);
                }
            }
            // Delete order
            await _unitOfWork.OrderRepository.DeleteAsync(order);

            await _unitOfWork.SaveChangesAsync();
        }
        //dua vao nam, thang

        public async Task<decimal> GetMonthlyRevenue(int year, int month)
        {
            return await _unitOfWork.OrderRepository.GetMonthlyRevenue(year, month);
        }

        public async Task<int> GetTotalOrder()
        {
            return await _unitOfWork.OrderRepository.GetTotalOrder();
        }

        public async Task<decimal> GetMonthlyRevenueNow()
        {
            return await _unitOfWork.OrderRepository.GetMonthlyRevenueNow();
        }

        public async Task QueryAndSendOrderConfirmationEmail(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetFullOrderInfo(orderId);

            if (order is null)
            {
                throw new NotFoundException("Order not found");
            }

            // Send email
            await _serviceFactory.EmailService.SendOrderConfirmationEmail(order);
        }

        public async Task ChangeOrderStatus(int orderId, string status)
        {
            var order = await _unitOfWork.OrderRepository.GetFullOrderInfo(orderId);

            if (order is null)
            {
                throw new NotFoundException("Order not found");
            }

            order.Status = status;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<decimal> GetRevenueBasedOnClub(int clubId)
        {
            /* var validOrderIds = await _unitOfWork.OrderRepository.GetValidOrderIdsAsync();

             // Step 2: Lấy tất cả các chi tiết đơn hàng từ bảng OrderDetail với OrderId từ validOrderIds
             var orderDetails = await _unitOfWork.OrderDetailsRepository.GetOrderDetailsByOrderIdsAsync(validOrderIds);

             // Step 3: Từ ShirtId trong bảng OrderDetail, tìm ra ShirtEditionId từ bảng Shirt
             var shirtIds = orderDetails.Select(od => od.ShirtId).Distinct().ToList();
             var shirts = await _unitOfWork.ShirtRepository.GetShirtDetailsByShirtIdsAsync(shirtIds);
             var shirtEditionIds = shirts.Select(s => s.ShirtEditionId).Distinct().ToList();

             // Step 4: Từ ShirtEditionId, tìm ra SeasonId từ bảng ShirtEdition
             var shirtEditions = await _unitOfWork.ShirtEditionRepository.GetShirtEditionsByIdsAsync(shirtEditionIds);
             var seasonIds = shirtEditions.Select(se => se.SeasonId).Distinct().ToList();

             // Step 5: Từ SeasonId, tìm ra ClubId từ bảng Season
             var seasons = await _unitOfWork.SeasonRepository.GetSeasonsByIdsAsync(seasonIds);
             var filteredOrders = seasons.Where(sn => sn.ClubId == clubId).ToList();

             // Step 6: Tính toán doanh thu
             var revenue = orderDetails
                 .Where(od => shirts.Any(s => clubShirtEditionIds.Contains(s.ShirtEditionId)))
         .Sum(od =>
         {
             var shirt = shirts.FirstOrDefault(s => s.Id == od.ShirtId);
             var shirtEdition = shirtEditions.FirstOrDefault(se => se.Id == shirt.ShirtEditionId);
             var price = shirtEdition?.DiscountPrice ?? 0; // Nếu không có giá, mặc định là 0
             return od.Quantity * price;
         });

             return revenue;*/
            var validOrderIds = await _unitOfWork.OrderRepository.GetValidOrderIdsAsync();

            // Bước 2: Lấy tất cả các chi tiết đơn hàng từ bảng OrderDetail với OrderId từ validOrderIds
            var orderDetails = await _unitOfWork.OrderDetailsRepository.GetOrderDetailsByOrderIdsAsync(validOrderIds);

            // Bước 3: Từ ShirtId trong bảng OrderDetail, tìm ra ShirtEditionId từ bảng Shirt
            var shirtIds = orderDetails.Select(od => od.ShirtId).Distinct().ToList();
            var shirts = await _unitOfWork.ShirtRepository.GetShirtDetailsByShirtIdsAsync(shirtIds);
            var shirtEditionIds = shirts.Select(s => s.ShirtEditionId).Distinct().ToList();

            // Bước 4: Từ ShirtEditionId, tìm ra SeasonId từ bảng ShirtEdition
            var shirtEditions = await _unitOfWork.ShirtEditionRepository.GetShirtEditionsByIdsAsync(shirtEditionIds);
            var seasonIds = shirtEditions.Select(se => se.SeasonId).Distinct().ToList();

            // Bước 5: Từ SeasonId, tìm ra ClubId từ bảng Season
            var seasons = await _unitOfWork.SeasonRepository.GetSeasonsByIdsAsync(seasonIds);
            var filteredSeasons = seasons.Where(se => se.ClubId == clubId).ToList();

            // Bước 6: Tính toán doanh thu
            var revenue = 0m;

            // Lấy các ShirtEditionId thuộc về ClubId cụ thể
            var clubShirtEditionIds = shirtEditions
                .Where(se => filteredSeasons.Any(fs => fs.Id == se.SeasonId))
                .Select(se => se.Id)
                .ToList();

            // Tính doanh thu dựa vào giá của các ShirtEdition
            revenue = orderDetails
                  .Where(od => shirts.Any(s => s.Id == od.ShirtId && clubShirtEditionIds.Contains(s.ShirtEditionId)))
                .Sum(od =>
                {
                    var shirt = shirts.FirstOrDefault(s => s.Id == od.ShirtId);
                    var shirtEdition = shirtEditions.FirstOrDefault(se => se.Id == shirt.ShirtEditionId);
                    var price = shirtEdition?.DiscountPrice ?? 0; // Nếu không có giá, mặc định là 0
                    return od.Quantity * price;
                });

            return revenue;
        }

        public async Task<int> CountOrdersByClubIdAsync(int clubId)
        {
            // Bước 1: Lấy tất cả OrderId hợp lệ (không ở trạng thái "incart")
            var validOrderIds = await _unitOfWork.OrderRepository.GetValidOrderIdsAsync();

            // Bước 2: Lấy tất cả các chi tiết đơn hàng từ bảng OrderDetail với OrderId từ validOrderIds
            var orderDetails = await _unitOfWork.OrderDetailsRepository.GetOrderDetailsByOrderIdsAsync(validOrderIds);

            // Bước 3: Từ ShirtId trong bảng OrderDetail, tìm ra ShirtEditionId từ bảng Shirt
            var shirtIds = orderDetails.Select(od => od.ShirtId).Distinct().ToList();
            var shirts = await _unitOfWork.ShirtRepository.GetShirtDetailsByShirtIdsAsync(shirtIds);
            var shirtEditionIds = shirts.Select(s => s.ShirtEditionId).Distinct().ToList();

            // Bước 4: Từ ShirtEditionId, tìm ra SeasonId từ bảng ShirtEdition
            var shirtEditions = await _unitOfWork.ShirtEditionRepository.GetShirtEditionsByIdsAsync(shirtEditionIds);
            var seasonIds = shirtEditions.Select(se => se.SeasonId).Distinct().ToList();

            // Bước 5: Từ SeasonId, tìm ra ClubId từ bảng Season và lọc theo clubId
            var seasons = await _unitOfWork.SeasonRepository.GetSeasonsByIdsAsync(seasonIds);
            var filteredSeasons = seasons.Where(se => se.ClubId == clubId).ToList();

            // Lấy các ShirtEditionId thuộc về ClubId cụ thể
            var clubShirtEditionIds = shirtEditions
                .Where(se => filteredSeasons.Any(fs => fs.Id == se.SeasonId))
                .Select(se => se.Id)
                .ToList();

            // Bước 6: Tính tổng số lượng đơn hàng
            var orderCount = orderDetails
                .Where(od => shirts.Any(s => s.Id == od.ShirtId && clubShirtEditionIds.Contains(s.ShirtEditionId)))
                .Select(od => od.OrderId)
                .Distinct()
                .Count();

            return orderCount;
        }
    }
}
