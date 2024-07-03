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
        public async Task<ShirtEditionRequest> CreateShirtEdition(ShirtEditionRequest request, ClaimsPrincipal claims)
        {
            if (await _unitOfWork.ShirtEditionRepository.AnyAsync(p => p.Code == request.Code)){
                throw new BadRequestException("Season with this code already exists");
            }
            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);
             if (account is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }

            /*  if (!await _unitOfWork.SeasonPlayer.AnyAsync(c => c.Id == request.SeasonId))
              {
                  throw new BadRequestException("Club does not exist");
          }*/
            if (await _unitOfWork.ShirtEditionRepository.CheckSeasonID(request.SeasonId) == false) { 
                throw new UnauthorizedException("Does not exist SeasonId");

            }
            var ShirtEdition = request.Adapt<ShirtEdition>();

            ShirtEdition.CreatedDate = DateTime.Now;
            ShirtEdition.CreatedAccountId = account.Id;
            ShirtEdition.Status = SeasonStatus.Active.ToString();

            await _unitOfWork.ShirtEditionRepository.AddAsync(ShirtEdition);
            await _unitOfWork.SaveChangesAsync();

            return ShirtEdition.Adapt<ShirtEditionRequest>();

        }


        public async Task<bool> DeleteShirtEditionAsync(int ShirtIdj)
        {
            var ShirtEdition = await _unitOfWork.ShirtEditionRepository.GetByIdAsync(ShirtIdj);
            if (ShirtEdition == null)
            {
                return false;
            }

            ShirtEdition.Status = SeasonStatus.Deleted.ToString();
            await _unitOfWork.ShirtEditionRepository.UpdateAsync(ShirtEdition);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

    

        public Task<List<ShirtEdition>> getAllShirtEdition()
        {
            return _unitOfWork.ShirtEditionRepository.GetAll() ;
        }

        public async Task<PagedResultResponse<GetShirtEdtionModel>> GetPagedSeasons(QueryPagedShirtEditionRequest request)
        {
            return (await _unitOfWork.ShirtEditionRepository.GetPagedShirtsEdition(request)).Adapt<PagedResultResponse<GetShirtEdtionModel>>();

        }

        public Task<ShirtEdition> GetShirteditionDetailsById(int id)
        {
            return _unitOfWork.ShirtEditionRepository.getShirtEditionbyId(id) ;
        }


        public async Task UpdateShirtEdition(int id, ShirtEditionRequest request, ClaimsPrincipal claims)
        {

            if (await _unitOfWork.ShirtEditionRepository.AnyAsync(p => p.Code == request.Code))
            {
                throw new BadRequestException("Season with this code already exists");
            }
            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);
            if (account is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }

            /*  if (!await _unitOfWork.SeasonPlayer.AnyAsync(c => c.Id == request.SeasonId))
              {
                  throw new BadRequestException("Club does not exist");
          }*/
            if (await _unitOfWork.ShirtEditionRepository.CheckSeasonID(request.SeasonId) == false)
            {
                throw new UnauthorizedException("Does not exist SeasonId");

            }
            var getShirtEdition = await _unitOfWork.ShirtEditionRepository.FindOneAsync(p => p.Id == id);

            request.Adapt(getShirtEdition);
            getShirtEdition.ModifiedDate = DateTime.Now;

            getShirtEdition.ModifiedAccountId = account.Id;
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
