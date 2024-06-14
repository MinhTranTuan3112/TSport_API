using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.BusinessLogic.Interfaces;

using TSport.Api.DataAccess.Interfaces;
using TSport.Api.Models.RequestModels.Club;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Club;
using TSport.Api.Models.ResponseModels.Shirt;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels;
using TSport.Api.Services.BusinessModels.Club;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.BusinessLogic.Services
{
    public class ClubService : IClubService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClubService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResultResponse<GetClubModel>> GetPagedClub(QueryCLubRequest queryPagedClubDto)
        {
            return (await _unitOfWork.ClubRepository.GetPagedClub(queryPagedClubDto)).Adapt<PagedResultResponse<GetClubModel>>();
        }



        public Task<List<GetClubModel>> GetShirts()
        {
            throw new NotImplementedException();
        }
        public async Task<GetClubModel> GetClubByClubId(int id)
        {
            var shirt = await _unitOfWork.ClubRepository.GetClubDetailById(id);
            if (shirt is null)
            {
                throw new NotFoundException("Shirt not found");
            }

            return shirt.Adapt<GetClubModel>();
        }

        public async Task<CreateClubResponse> AddClub(CreateClubRequest createClubRequest, ClaimsPrincipal user)
        {
            string? userId = user.FindFirst(c => c.Type == "aid")?.Value;

            if (userId is null)
            {
                throw new BadRequestException("User Unauthorized");
            }

            var existedShirt = await _unitOfWork.ClubRepository.FindOneAsync(s => s.Code == createClubRequest.Code);
            if (existedShirt is not null)
            {
                throw new BadRequestException("Club code existed!");
            }
            Club club = createClubRequest.Adapt<Club>();

            club.Status = "Active";
            club.CreatedAccountId = Int32.Parse(userId);
            club.CreatedDate = DateTime.Now;

            await _unitOfWork.ClubRepository.AddAsync(club);
            await _unitOfWork.SaveChangesAsync();

            return club.Adapt<CreateClubResponse>();

        }


    }
}
