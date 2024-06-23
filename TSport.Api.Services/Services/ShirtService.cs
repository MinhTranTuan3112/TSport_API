using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Shirt;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels;
using TSport.Api.Services.BusinessModels.Shirt;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Enums;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Services.Services
{
    public class ShirtService : IShirtService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;
        private readonly IRedisCacheService<PagedResultResponse<GetShirtInPagingResultModel>> _pagedResultCacheService;

        public ShirtService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory, IRedisCacheService<PagedResultResponse<GetShirtInPagingResultModel>> pagedResultCacheService)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
            _pagedResultCacheService = pagedResultCacheService;
        }

        public async Task<PagedResultResponse<GetShirtInPagingResultModel>> GetPagedShirts(QueryPagedShirtsRequest request)
        {
            var pagedResult = await _unitOfWork.ShirtRepository.GetPagedShirts(request);

            return pagedResult.Adapt<PagedResultResponse<GetShirtInPagingResultModel>>();
        }

        public async Task<PagedResultResponse<GetShirtInPagingResultModel>> GetCachedPagedShirts(QueryPagedShirtsRequest request)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);

            return await _pagedResultCacheService.GetOrSetCacheAsync(
                $"pagedShirts_{serializedRequest}",
                () => GetPagedShirts(request)
            ) ?? new PagedResultResponse<GetShirtInPagingResultModel>();
        }

        public async Task<ShirtDetailModel> GetShirtDetailById(int id)
        {
            var shirt = await _unitOfWork.ShirtRepository.GetShirtDetailById(id);
            if (shirt is null)
            {
                throw new NotFoundException("Shirt not found");
            }
            else if (shirt.Status is not null && shirt.Status.Equals("Deleted"))
            {
                throw new BadRequestException("Shirt deleted");
            }

            return shirt.Adapt<ShirtDetailModel>();
        }
        public async Task<CreateShirtResponse> AddShirt(CreateShirtRequest createShirtRequest, ClaimsPrincipal user)
        {
            string? userId = user.FindFirst(c => c.Type == "aid")?.Value;

            if (userId is null)
            {
                throw new BadRequestException("User Unauthorized");
            }

            var existedShirt = await _unitOfWork.ShirtRepository.FindOneAsync(s => s.Code == createShirtRequest.Code);
            if (existedShirt is not null)
            {
                throw new BadRequestException("Shirt code existed!");
            }

            Shirt shirt = createShirtRequest.Adapt<Shirt>(); // when mapping, there are a image obj with id = 0, shirtId = 0 by default, don't know why
            if (shirt.Images.Any())
            {
                shirt.Images.Clear(); // remove all items from shirt's image list
            }

            shirt.Status = "Active";
            shirt.CreatedAccountId = Int32.Parse(userId);
            shirt.CreatedDate = DateTime.Now;

            await _unitOfWork.ShirtRepository.AddAsync(shirt);
            await _unitOfWork.SaveChangesAsync();

            //            var imageConut = _unitOfWork.ImageRepository.Entities.Count() + 1; // init image Id
            var result = new CreateShirtResponse();
            List<string> imageList = [];

            foreach (var image in createShirtRequest.Images)
            {
                var imageUrl = await _serviceFactory.FirebaseStorageService.UploadImageAsync(image);
                await _unitOfWork.ImageRepository.AddAsync(new Image
                {
                    //                    Id = imageConut,
                    Url = imageUrl,
                    ShirtId = shirt.Id
                });
                //                imageConut++; // image Id +1 for next image
                await _unitOfWork.SaveChangesAsync();
                imageList.Add(imageUrl);
            }

            result = shirt.Adapt<CreateShirtResponse>();
            result.ImagesUrl = imageList;
            return result;
        }


        public async Task UpdateShirt(UpdateShirtRequest request, ClaimsPrincipal user)
        {
            var supabaseId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);    

            if (account is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }

            var shirt = await _unitOfWork.ShirtRepository.FindOneAsync(s => s.Code == request.Code);

            //there are a image obj with id = 0, shirtId = 0 by default, don't know why
            if (shirt is null)
            {
                throw new NotFoundException("Shirt not existed!");
            }

            if (shirt.Status is not null && shirt.Status.Equals("Deleted"))
            {
                throw new BadRequestException("Shirt deleted!");
            }

            if (shirt.Images.Count != 0)
            {
                shirt.Images.Clear(); // remove all items from shirt's image list
            }

// //            shirt.ModifiedAccountId = Int32.Parse(userId);
// //            shirt.ModifiedDate = DateTime.Now;
//             shirt.Description = updateShirtRequest.Description is null ? shirt.Description : updateShirtRequest.Description;
//             shirt.Quantity = updateShirtRequest.Quantity is null ? shirt.Quantity : updateShirtRequest.Quantity;
//             shirt.ShirtEditionId = (int)(updateShirtRequest.ShirtEditionId is null ? shirt.ShirtEditionId : updateShirtRequest.ShirtEditionId);
//             shirt.SeasonPlayerId = (int)(updateShirtRequest.SeasonPlayerId is null ? shirt.SeasonPlayerId : updateShirtRequest.SeasonPlayerId);

            request.Adapt(shirt);
            shirt.ModifiedAccountId = account.Id;
            shirt.ModifiedDate = DateTime.Now;

            var images = await _unitOfWork.ImageRepository.FindAsync(i => i.ShirtId == shirt.Id);

            foreach(var image in images)
            {
                await _unitOfWork.ImageRepository.DeleteAsync(image);
                await _unitOfWork.SaveChangesAsync();
            }

            var result = new UpdateShirtResponse();
            List<string> imageList = [];

            foreach (var image in request.Images)
            {
                var imageUrl = await _serviceFactory.FirebaseStorageService.UploadImageAsync(image);

                await _unitOfWork.ImageRepository.AddAsync(new Image
                {
                    Url = imageUrl,
                    ShirtId = shirt.Id
                });
                await _unitOfWork.SaveChangesAsync();
                imageList.Add(imageUrl);
            }

        }
        
        public async Task DeleteShirt(int id)
        {
            var shirt = await _unitOfWork.ShirtRepository.FindOneAsync(s => s.Id == id);
            if (shirt is null)
            {
                throw new NotFoundException("Shirt not found");
            }

            else if (shirt.Status is not null && shirt.Status == ShirtStatus.Deleted.ToString())
            {
                throw new BadRequestException("Shirt deleted");
            }

            shirt.Status = ShirtStatus.Deleted.ToString();

            await _unitOfWork.SaveChangesAsync();
        }
        
    }
}

