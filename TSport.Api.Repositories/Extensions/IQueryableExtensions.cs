using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TSport.Api.Models.RequestModels.Club;
using TSport.Api.Models.RequestModels.Order;
using TSport.Api.Models.RequestModels.Player;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.RequestModels.ShirtEdition;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Shared.Enums;

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

                query = query.Where(s => s.ShirtEdition != null && s.ShirtEdition.DiscountPrice != null &&
                    s.ShirtEdition.DiscountPrice >= request.StartPrice && s.ShirtEdition.DiscountPrice <= request.EndPrice)
                    .Union(query.Where(s => s.ShirtEdition.DiscountPrice == null && 
                    s.ShirtEdition.StockPrice >= request.StartPrice && s.ShirtEdition.StockPrice <= request.EndPrice));
            }

            if (request.Sizes is not [])
            {
                query = query.Where(s => s.ShirtEdition != null && request.Sizes.Contains(s.ShirtEdition.Size.ToUpper()));
            }
            if (request.SeasonIds is not [])
            {
                query = query.Where(s => request.SeasonIds.Contains(s.SeasonPlayer.SeasonId));
            }
            if (request.PlayerIds is not [])
            {
                query = query.Where(s => request.PlayerIds.Contains(s.SeasonPlayer.PlayerId));
            }
            if (request.ClubIds is not [])
            {
                query = query.Where(s => s.SeasonPlayer.Season.ClubId != null && request.ClubIds.Contains((int)s.SeasonPlayer.Season.ClubId));
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
        public static IQueryable<ShirtEdition> ApplyPagedShirtEditionFilterFilter(this IQueryable<ShirtEdition> query, QueryPagedShirtEditionRequest request)
        {
            if (!string.IsNullOrEmpty(request.Code))
            {
                query = query.Where(p => p.Code != null && p.Code.ToLower().Contains(request.Code.ToLower()));
            }

            if (request.Size != null && request.Size.Length > 0)
            {
                query = query.Where(s => request.Size.Contains(s.Size.ToUpper()));
            }

            if (request.HasSignature.HasValue)
            {
                query = query.Where(p => p.HasSignature == request.HasSignature.Value);
            }

            if (!string.IsNullOrEmpty(request.Color))
            {
                query = query.Where(p => p.Color != null && p.Color.ToLower().Contains(request.Color.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.Origin))
            {
                query = query.Where(p => p.Origin != null && p.Origin.ToLower().Contains(request.Origin.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.Material))
            {
                query = query.Where(p => p.Material != null && p.Material.ToLower().Contains(request.Material.ToLower()));
            }
            if (request.SeasonId.HasValue)
            {
                query = query.Where(p => p.SeasonId == request.SeasonId);
            }

            if (request.StartPrice < request.EndPrice)
            {
                query = query.Where(s => s.DiscountPrice >= request.StartPrice &&
                                        s.DiscountPrice <= request.EndPrice);
            }


            return query;

        }

        public static IQueryable<Order> ApplyPagedOrdersFilter(this IQueryable<Order> query, QueryPagedOrderRequest request)
        {
            query = query.Where(o => o.Status != OrderStatus.InCart.ToString());

            if (request.StartDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= request.EndDate.Value);
            }

            if (request.CreatedAccountId.HasValue)
            {
                query = query.Where(o => o.CreatedAccountId == request.CreatedAccountId.Value);
            }

            if (request.Status != null)
            {
                query = query.Where(o => request.Status.Contains(o.Status));
            }



            return query;
        }
    }
}