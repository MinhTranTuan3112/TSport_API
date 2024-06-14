using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Shirt;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels;
using TSport.Api.Services.BusinessModels.Shirt;
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

        public async Task<PagedResultResponse<GetShirtModel>> GetPagedShirts(QueryPagedShirtsRequest request)
        {
            return (await _unitOfWork.ShirtRepository.GetPagedShirts(request)).Adapt<PagedResultResponse<GetShirtModel>>();
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
            if ( existedShirt is not null )
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
        public async Task DeleteShirt(int id)
        {
            var shirt = await _unitOfWork.ShirtRepository.FindOneAsync(s => s.Id == id);
            if (shirt is null)
            {
                throw new NotFoundException("Shirt not found");
            }

            else if (shirt.Status is not null && shirt.Status.Equals("Deleted"))
            {
                throw new BadRequestException("Shirt deleted");
            }

            shirt.Status = "Deleted";

            await _unitOfWork.ShirtRepository.UpdateAsync(shirt);
            await _unitOfWork.SaveChangesAsync();
            return;
        }
    }
}
       
