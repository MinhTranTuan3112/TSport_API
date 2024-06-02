using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.Entities;
using TSport.Api.Models.RequestModels;
using TSport.Api.Models.ResponseModels.Shirt;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Services.Services
{
    public class ShirtService : IShirtService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;


        public ShirtService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
        }

        public async Task<CreateShirtResponse> AddShirt(CreateShirtRequest createShirtRequest, ClaimsPrincipal user)
        {
            string? userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                throw new BadRequestException("User Unauthorized");
            }

            Shirt shirt = CreateShirtRequest.Adapt<Shirt>();
            shirt.Status = "Active";
            shirt.CreatedAccountId = int.Parse(userId);
            shirt.CreatedDate = DateTime.Now;
            shirt.Id = CountShirt() + 1;  // tui thu bo cai nay r nhung ma chay api len no tra ve loi Id = null khong add vao DB duoc

            await _unitOfWork.GetShirtRepository().AddAsync(shirt);
            await _unitOfWork.SaveChangesAsync();

            return (await _unitOfWork.GetShirtRepository().GetByIdAsync(CountShirt())).Adapt<CreateShirtResponse>();
        }
        private int CountShirt()
        {
            return _unitOfWork.GetShirtRepository().Entities.Count();
        }
    }
}
