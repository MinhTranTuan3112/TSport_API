using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Order;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Extensions;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Shared.Enums;
using Mapster;

namespace TSport.Api.Repositories.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly TsportDbContext _context;

        public OrderRepository(TsportDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Order?> GetCustomerCartInfo(int accountId)
        {

            var order = await _context.Orders
                .AsNoTracking()
                .Where(o => o.CreatedAccountId == accountId && o.Status == OrderStatus.InCart.ToString())
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Shirt)
                        .ThenInclude(s => s.ShirtEdition)
                .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Shirt)
                            .ThenInclude(s => s.Images) // Include Shirt Images
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<PagedResultResponse<Order>> GetPagedOrders(QueryPagedOrderRequest request)
        {
            var query = _context.Orders.AsQueryable();


            query = query.ApplyPagedOrdersFilter(request);


            query = request.OrderByDesc ? query.OrderByDescending(GetSortProperty(request.SortColumn))
                                        : query.OrderBy(GetSortProperty(request.SortColumn));


            return await query.ToPagedResultResponseAsync(request.PageNumber, request.PageSize);
        }

        private Expression<Func<Order, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "id" => o => o.Id,
                "createddate" => o => o.CreatedDate,
                "orderdate" => o => o.OrderDate,
                _ => o => o.Id
            };
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public async Task<Order?> GetOrderDetailsInfoById(int orderId)
        {
            return await _context.Orders.AsNoTracking()
                                        .Where(o => o.Id == orderId)
                                        .Include(o => o.CreatedAccount)
                                        .Include(o => o.ModifiedAccount)
                                        .Include(o => o.Payments)
                                        .Include(o => o.OrderDetails)
                                        .ThenInclude(od => od.Shirt)
                                        .ThenInclude(s => s.ShirtEdition)
                                        .AsSplitQuery()
                                        .SingleOrDefaultAsync();
        }

        public async Task<decimal> GetMonthlyRevenue(int year, int month)
        {
            var monthlyRevenue = await _context.Orders
                .Where(o => o.OrderDate.Year == year && o.OrderDate.Month == month && o.Status != "InCart")
            .SumAsync(o => o.Total);

            return monthlyRevenue;
        }

        public async Task<int> GetTotalOrder()
        {
            var totalOrder = await _context.Orders
                .Where(o => o.Status != "InCart")
                .CountAsync();
            return totalOrder;


        }

        public async Task<decimal> GetMonthlyRevenueNow()
        {
            var now = DateTime.Now;
            var year = now.Year;
            var month = now.Month;

            var monthlyRevenue = await _context.Orders
                .Where(o => o.OrderDate.Year == year && o.OrderDate.Month == month && o.Status != "InCart")
                .SumAsync(o => o.Total);

            return monthlyRevenue;
        }

        public async Task<List<Order>?> GetCustomerInfo(int accountId)
        {
            var order = await _context.Orders
                        .AsNoTracking()
                        .Where(o => o.CreatedAccountId == accountId && !(o.Status == OrderStatus.InCart.ToString()))
                        .Include(o => o.OrderDetails)
                            .ThenInclude(or => or.Shirt)
                                .ThenInclude(s => s.ShirtEdition)
                        .ToListAsync();
            return order;
        }

        public async Task<Order?> GetFullOrderInfo(int orderId)
        {
            return await _context.Orders.AsNoTracking()
                                        .Where(o => o.Id == orderId)
                                        .Include(o => o.CreatedAccount)
                                        .Include(o => o.OrderDetails)
                                        .ThenInclude(od => od.Shirt)
                                        .ThenInclude(s => s.ShirtEdition)
                                        .ThenInclude(se => se.Season)
                                        .ThenInclude(s => s.Club)
                                        .AsSplitQuery()
                                        .SingleOrDefaultAsync();
        }

        public async Task<ClubOrderReportResponse> GetClubOrderReport(List<int> clubIds)
        {
            // Step 1: Lấy tất cả các OrderId từ bảng Order với trạng thái khác "InCart"
            var validOrders = await _context.Orders
                                            .AsNoTracking()
                                            .Where(o => o.Status != OrderStatus.InCart.ToString())
                                            .Select(o => o.Id)
                                            .ToListAsync();

            // Step 2: Lấy tất cả các chi tiết đơn hàng từ bảng OrderDetail với OrderId từ ValidOrders
            var orderDetails = await _context.OrderDetails
                                             .AsNoTracking()
                                             .Where(od => validOrders.Contains(od.OrderId))
                                             .ToListAsync();

            // Step 3-5: Kết hợp các truy vấn để tìm ClubId từ OrderDetails
            var seasonsQuery = from od in _context.OrderDetails.AsNoTracking()
                               join s in _context.Shirts.AsNoTracking() on od.ShirtId equals s.Id
                               join se in _context.ShirtEditions.AsNoTracking() on s.ShirtEditionId equals se.Id
                               join sn in _context.Seasons.AsNoTracking() on se.SeasonId equals sn.Id
                               where validOrders.Contains(od.OrderId)
                               select new
                               {
                                   od.OrderId,
                                   od.Quantity,
                                   od.Size,
                                   ClubId = (int?)sn.ClubId // Ensure ClubId is nullable
                               };

            var seasons = await seasonsQuery.ToListAsync();

            // Step 6: Kiểm tra ClubId và lọc ra các đơn hàng có ClubId nằm trong danh sách clubIds
            var ordersWithClubIds = seasons
                                    .Where(se => se.ClubId.HasValue && clubIds.Contains(se.ClubId.Value))
                                    .Select(se => se.OrderId)
                                    .Distinct()
                                    .ToList();

            var orders = await _context.Orders
                                       .AsNoTracking()
                                       .Where(o => ordersWithClubIds.Contains(o.Id))
                                       .ToListAsync();

            // Tính tổng doanh thu
            var totalRevenue = orders.Sum(o => o.Total);

            return new ClubOrderReportResponse
            {
                TotalRevenue = totalRevenue,
                Orders = orders.Adapt<List<OrderModel>>() // Ensure you have Mapster installed and imported
            };
        }


    }
}
