using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.ResponseModels.Shirt;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Services.Services
{
    public class ShirtService : IShirtService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShirtService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetShirtDetailResponse> GetShirtDetailById(int id)
        {
            var shirt = await _unitOfWork.ShirtRepository.GetShirtDetailById(id);
            if (shirt is null)
            {
                throw new NotFoundException("Shirt not found");
            }
            return shirt.Adapt<GetShirtDetailResponse>();
        }
    }
}
