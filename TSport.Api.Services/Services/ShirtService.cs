using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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

        private readonly string _bucketName = "Shirts";

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
            var supabaseId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);

            if (account is null)
            {
                throw new UnauthorizedException("Unauthorized");
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
            shirt.CreatedAccountId = account.Id;
            shirt.CreatedDate = DateTime.Now;

            await _unitOfWork.ShirtRepository.AddAsync(shirt);
            await _unitOfWork.SaveChangesAsync();

            //            var imageConut = _unitOfWork.ImageRepository.Entities.Count() + 1; // init image Id
            var result = new CreateShirtResponse();
            List<string> imageList = [];

            foreach (var image in createShirtRequest.Images)
            {
                var imageUrl = await _serviceFactory.SupabaseStorageService.UploadImageAsync(image, _bucketName,
                    $"{shirt.Code}_{Guid.NewGuid()}.jpg");
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

            else if (shirt.Status is not null && shirt.Status == ShirtStatus.Deleted.ToString())
            {
                throw new BadRequestException("Shirt deleted");
            }

            shirt.Status = ShirtStatus.Deleted.ToString();

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateShirt(int id, UpdateShirtRequest request, ClaimsPrincipal claims)
        {
            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);

            if (account is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }

            var shirt = await _unitOfWork.ShirtRepository.FindOneAsync(s => s.Id == id);

            if (shirt is null)
            {
                throw new NotFoundException("Shirt not found!");
            }

            request.Adapt(shirt);
            shirt.ModifiedDate = DateTime.Now;
            shirt.ModifiedAccountId = account.Id;


            if (request.ShirtImages is not null or [])
            {
                List<Image> images = [];

                await _unitOfWork.ImageRepository.ExecuteDeleteAsync(i => i.ShirtId == shirt.Id);

                var imageUrls = await _serviceFactory.SupabaseStorageService.UploadImagesAsync(request.ShirtImages, _bucketName);

                foreach (var imageUrl in imageUrls)
                {
                    images.Add(new Image
                    {
                        Url = imageUrl,
                        ShirtId = shirt.Id
                    });
                }

                await _unitOfWork.ImageRepository.AddRangeAsync(images);
            }

            await _unitOfWork.SaveChangesAsync();
        }
        private readonly IConfiguration _configuration;

        //public TokenService(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        public string GenerateAccessToken(int accountId, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtAuth:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>(){
                new(ClaimTypes.Role, role),
                new("aid", accountId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtAuth:Issuer"],
                audience: _configuration["JwtAuth:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string expiredAccessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtAuth:Key"]!)),
                ValidateLifetime = false,
                ValidAudience = _configuration["JwtAuth:Audience"],
                ValidIssuer = _configuration["JwtAuth:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(expiredAccessToken, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
              StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid access token");
            }

            return principal;
        }
    }
}

    


