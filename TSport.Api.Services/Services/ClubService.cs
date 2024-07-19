using Mapster;
using Newtonsoft.Json;
using System.Security.Claims;
using TSport.Api.BusinessLogic.Interfaces;
using TSport.Api.Models.RequestModels.Club;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Club;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels.Club;
using TSport.Api.Services.BusinessModels.Shirt;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.BusinessLogic.Services
{
    public class ClubService : IClubService
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IRedisCacheService<PagedResultResponse<GetClubModel>> _pagedResultCacheService;

        private readonly string _bucketName = "Clubs";

        public ClubService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory, IRedisCacheService<PagedResultResponse<GetClubModel>> pagedResultCacheService)
        {
            _serviceFactory = serviceFactory;
            _unitOfWork = unitOfWork;
            _pagedResultCacheService = pagedResultCacheService;
        }

        public async Task<PagedResultResponse<GetClubModel>> GetPagedClubs(QueryClubRequest request)
        {
            return (await _unitOfWork.ClubRepository.GetPagedClub(request)).Adapt<PagedResultResponse<GetClubModel>>();
        }

        public async Task<PagedResultResponse<GetClubModel>> GetCachedPagedClubs(QueryClubRequest request)
        {
            return await _pagedResultCacheService.GetOrSetCacheAsync(
                $"pagedClubs_{JsonConvert.SerializeObject(request)}",
                () => GetPagedClubs(request)
            ) ?? new PagedResultResponse<GetClubModel>();
        }

        public async Task<GetClubDetailsModel> GetClubDetailsById(int id)
        {
            var shirt = await _unitOfWork.ClubRepository.GetClubDetailById(id);
            if (shirt is null)
            {
                throw new NotFoundException("Club not found");
            }

            return shirt.Adapt<GetClubDetailsModel>();
        }

        public async Task<GetClubResponse> AddClub(CreateClubRequest createClubRequest, ClaimsPrincipal user)
        {
            var supabaseId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (supabaseId is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }

            var account = await _serviceFactory.AccountService.GetAccountBySupabaseId(supabaseId);

            var existedShirt = await _unitOfWork.ClubRepository.FindOneAsync(s => s.Code == createClubRequest.Code);
            if (existedShirt is not null)
            {
                throw new BadRequestException("Club code existed!");
            }

            var club = createClubRequest.Adapt<Club>();

            club.Status = "Active";
            club.CreatedAccountId = account.Id;
            club.CreatedDate = DateTime.Now;
            // club.ModifiedDate = DateTime.Now;
            // club.ModifiedAccountId = account.Id;

            if (createClubRequest.Image is not null)
            {
                var imageUrl = await _serviceFactory.SupabaseStorageService.UploadImageAsync(createClubRequest.Image, _bucketName);
                club.LogoUrl = imageUrl;
            }


            await _unitOfWork.ClubRepository.AddAsync(club);
            await _unitOfWork.SaveChangesAsync();

            return club.Adapt<GetClubResponse>();

        }

        public async Task DeleteClub(int id)
        {
            var club = await _unitOfWork.ClubRepository.FindOneAsync(c => c.Id == id);

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

        public async Task UpdateClub(int id, UpdateClubRequest updateClubRequest, ClaimsPrincipal user)
        {
            var supabaseId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var account = await _serviceFactory.AccountService.GetAccountBySupabaseId(supabaseId!);

            var club = await _unitOfWork.ClubRepository.FindOneAsync(s => s.Id == id);

            if (club is null)
            {
                throw new NotFoundException("Club not found");
            }

            // club.Name = updateClubRequest.Name is null ? club.Name : updateClubRequest.Name;
            // club.Status = updateClubRequest.Status is null ? club.Status : updateClubRequest.Status;

            updateClubRequest.Adapt(club);

            club.ModifiedDate = DateTime.Now;
            club.ModifiedAccountId = account.Id;

            if (updateClubRequest.Image is not null)
            {
                var imageUrl = await _serviceFactory.SupabaseStorageService.UploadImageAsync(updateClubRequest.Image, _bucketName);
                club.LogoUrl = imageUrl;
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ViewReponse>> GetAllClubs()
        {
            var Clubs = (await _unitOfWork.ClubRepository.GetAll());
            if (Clubs == null)
            {
                throw new NotFoundException("No player was found");


            }
            return Clubs.Adapt<List<ViewReponse>>();
        }
    }
}
