using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels.Shirt;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Enums;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Services.Services
{
    public class ShirtService : IShirtService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShirtService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteShirt(int id)
        {
            var shirt = await _unitOfWork.ShirtRepository.GetByIdAsync(id);
            if (shirt is null)
            {
                throw new NotFoundException("Shirt not found!");
            }
            shirt.Status = ShirtStatus.Deleted.ToString();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedResultResponse<GetShirtModel>> GetPagedShirts(QueryPagedShirtsRequest request)
        {
            return (await _unitOfWork.ShirtRepository.GetPagedShirts(request)).Adapt<PagedResultResponse<GetShirtModel>>();
        }
        
    }
}