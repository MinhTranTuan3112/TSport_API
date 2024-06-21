using Mapster;
using Microsoft.VisualStudio.CodeCoverage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.BusinessLogic.Interfaces;

using TSport.Api.Models.RequestModels.Club;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Club;

using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels.Club;
using TSport.Api.Services.BusinessModels.Shirt;
using TSport.Api.Services.Interfaces;
using TSport.Api.Services.Services;
using TSport.Api.Shared.Enums;
using TSport.Api.Shared.Exceptions;
using static System.Net.Mime.MediaTypeNames;

namespace TSport.Api.BusinessLogic.Services
{
    public class ClubService : IClubService
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisCacheService<PagedResultResponse<GetClubModel>> _pagedResultCacheService;
        public ClubService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory, IRedisCacheService<PagedResultResponse<GetClubModel>> pagedResultCacheService)
        {
            _serviceFactory = serviceFactory;
            _unitOfWork = unitOfWork;
            _pagedResultCacheService = pagedResultCacheService;
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
                throw new NotFoundException("Club not found");
            }

            return shirt.Adapt<GetClubModel>();
        }

        public async Task<CreateClubResponse> AddClub(CreateClubRequest createClubRequest,ClaimsPrincipal user)
        {
            string? userId = "1";
                //user.FindFirst(c => c.Type == "aid")?.Value;

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
            club.ModifiedDate = DateTime.Now;
            club.ModifiedAccountId = Int32.Parse(userId);
            var imageUrl = await _serviceFactory.FirebaseStorageService.UploadImageAsync(createClubRequest.Images);
             club.LogoUrl = imageUrl;
            
            await _unitOfWork.ClubRepository.AddAsync(club);
            await _unitOfWork.SaveChangesAsync();

            return club.Adapt<CreateClubResponse>();

        }

        public async Task DeleteClub(int id)
        {
            var club = await _unitOfWork.ClubRepository.FindOneAsync(s => s.Id == id);
            if (club is null)
            {
                throw new NotFoundException("Club not found");
            }

            else if (club.Status is not null && club.Status == "Deleted")
            {
                throw new BadRequestException("Club deleted");
            }

            club.Status = "Deleted";

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<UpdateClubResponse> UpdateClub(UpdateClubRequest updateClubRequest, ClaimsPrincipal user)
        {
            string? userId = "1";
            var club = await _unitOfWork.ClubRepository.FindOneAsync(s=> s.Code == updateClubRequest.Code);
            if (club is null)
            {
                throw new NotFoundException("Club not found");
            }
            club.Name = updateClubRequest.Name is null ? club.Name : updateClubRequest.Name;
            club.Status = updateClubRequest.Status is null ? club.Status : updateClubRequest.Status;
            
            club.ModifiedDate = DateTime.Now;
            club.ModifiedAccountId = Int32.Parse(userId);
            if (updateClubRequest.Image != null)
            {
                var imageUrl = await _serviceFactory.FirebaseStorageService.UploadImageAsync(updateClubRequest.Image);

                club.LogoUrl = imageUrl;
            }

           var    result = club.Adapt<UpdateClubResponse>();
            await _unitOfWork.SaveChangesAsync();

            return result;
        }
    }
}
