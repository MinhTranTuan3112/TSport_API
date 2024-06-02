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

namespace TSport.Api.Services.Services
{
    public class ShirtService : IShirtService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShirtService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResultResponse<GetShirtModel>> GetPagedShirts(QueryPagedShirtsRequest request)
        {
            return (await _unitOfWork.ShirtRepository.GetPagedShirts(request)).Adapt<PagedResultResponse<GetShirtModel>>();
        }
    }
}