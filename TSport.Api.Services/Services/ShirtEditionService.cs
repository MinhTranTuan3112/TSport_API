using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.RequestModels.ShirtEdition;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels;
using TSport.Api.Services.BusinessModels.Season;
using TSport.Api.Services.BusinessModels.ShirtEdition;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Enums;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Services.Services
{
    public class ShirtEditionService : IShirtEditionService
    {
        private readonly IUnitOfWork _unitOfWork;


        public ShirtEditionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ShirtEditionModel> CreateShirtEdition(CreateShirtEditionRequest request, ClaimsPrincipal claims)
        {

            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);
            if (account is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }


            if (!await _unitOfWork.SeasonRepository.AnyAsync(se => se.Id == request.SeasonId))
            {
                throw new BadRequestException("SeasonId does not exist");
            }

            var shirtEdition = request.Adapt<ShirtEdition>();

            shirtEdition.Code = $"SE{Guid.NewGuid().ToString()}";
            shirtEdition.CreatedDate = DateTime.Now;
            shirtEdition.CreatedAccountId = account.Id;
            shirtEdition.Status = SeasonStatus.Active.ToString();

            await _unitOfWork.ShirtEditionRepository.AddAsync(shirtEdition);
            await _unitOfWork.SaveChangesAsync();

            return shirtEdition.Adapt<ShirtEditionModel>();
        }


        public async Task DeleteShirtEdition(int shirtEditionId)
        {
            var shirtEdition = await _unitOfWork.ShirtEditionRepository.GetByIdAsync(shirtEditionId);
            if (shirtEdition is null)
            {
                throw new NotFoundException("ShirtEdition not found");
            }

            shirtEdition.Status = SeasonStatus.Deleted.ToString();
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<PagedResultResponse<GetShirtEdtionModel>> GetPagedShirtEditions(QueryPagedShirtEditionRequest request)
        {
            return (await _unitOfWork.ShirtEditionRepository.GetPagedShirtsEdition(request)).Adapt<PagedResultResponse<GetShirtEdtionModel>>();
        }

        public async Task<ShirtEditionDetailsModel> GetShirtEditionDetailsById(int id)
        {
            var shirtEdition = await _unitOfWork.ShirtEditionRepository.GetShirtEditionById(id);

            if (shirtEdition is null)
            {
                throw new NotFoundException("Shirt edition not found");
            }

            return shirtEdition.Adapt<ShirtEditionDetailsModel>();
        }

        public async Task<List<GetShirtEdtionModel>> GetShirtEditions()
        {
            return (await _unitOfWork.ShirtEditionRepository.GetAllAsync()).Adapt<List<GetShirtEdtionModel>>();
        }

        public async Task UpdateShirtEdition(int id, UpdateShirtEditionRequest request, ClaimsPrincipal claims)
        {

            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);

            if (account is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }

            if (request.SeasonId.HasValue)
            {
                if (!await _unitOfWork.SeasonRepository.AnyAsync(se => se.Id == request.SeasonId))
                {
                    throw new BadRequestException("Does not exist SeasonId");
                }
            }

            var shirtEdition = await _unitOfWork.ShirtEditionRepository.FindOneAsync(p => p.Id == id);

            if (shirtEdition is null)
            {
                throw new NotFoundException("Shirt edition not found");
            }

            request.Adapt(shirtEdition);

            shirtEdition.ModifiedDate = DateTime.Now;
            shirtEdition.ModifiedAccountId = account.Id;
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
