using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TSport.Api.Models.RequestModels.Club;
using TSport.Api.Models.RequestModels.Player;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Repositories.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedResultResponse<T>> ToPagedResultResponseAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        where T : class
        {
            return new PagedResultResponse<T>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }

        public static IQueryable<Shirt> ApplyPagedShirtsFilter(this IQueryable<Shirt> query, QueryPagedShirtsRequest request)
        {
            if (request.StartPrice < request.EndPrice)
            {
                query = query.Where(s => s.ShirtEdition != null && s.ShirtEdition.DiscountPrice >= request.StartPrice &&
                                        s.ShirtEdition.DiscountPrice <= request.EndPrice);
            }

            if (request.Sizes is not [])
            {
                query = query.Where(s => s.ShirtEdition != null && request.Sizes.Contains(s.ShirtEdition.Size.ToUpper()));
            }


            var filterProperties = typeof(QueryShirtRequest).GetProperties();
            var requestProperties = typeof(QueryPagedShirtsRequest).GetProperties();

            foreach (var requestProperty in requestProperties)
            {
                if (!filterProperties.Any(p => p.Name == requestProperty.Name))
                {
                    continue;
                }

                var propertyValue = requestProperty.GetValue(request, null);

                if (propertyValue is null)
                {
                    continue;
                }


                if (requestProperty.PropertyType == typeof(string))
                {
                    var filterValueLower = ((string)propertyValue).ToLower();

                    query = query.Where(s => EF.Property<string>(s, requestProperty.Name).ToLower().Contains(filterValueLower));
                }
                else
                {
                    query = query.Where(s => EF.Property<object>(s, requestProperty.Name) == propertyValue);
                }
            }


            return query;
        }
        public static IQueryable<Club> ApplyPagedClubFilter(this IQueryable<Club> query, QueryClubRequest request)
        {
            var filterProperties = typeof(ClubRequest).GetProperties();
            foreach (var property in filterProperties)
            {
                var propertyValue = property.GetValue(request.ClubRequest, null);

                if (propertyValue is null)
                {
                    continue;
                }

            }
            return query;

        }


        public static IQueryable<Season> ApplyPagedSeasonsFilter(this IQueryable<Season> query, QueryPagedSeasonRequest request)
        {
            if (!string.IsNullOrEmpty(request.Code))
            {
                query = query.Where(s => s.Code != null && s.Code.ToLower().Contains(request.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(s => s.Name != null && s.Name.ToLower().Contains(request.Name.ToLower()));
            }

            return query;
        }

        public static IQueryable<Player> ApplyPagedPlayersFilter(this IQueryable<Player> query, QueryPagedPlayersRequest request)
        {
            if (!string.IsNullOrEmpty(request.Code))
            {
                query = query.Where(p => p.Code != null && p.Code.ToLower().Contains(request.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(p => p.Name != null && p.Name.ToLower().Contains(request.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query.Where(p => p.Status != null && p.Status.ToLower() == request.Status.ToLower());
            }

            if (request.ClubId.HasValue)
            {
                query = query.Where(p => p.ClubId == request.ClubId);
            }

            

            return query;
        }
    }
}