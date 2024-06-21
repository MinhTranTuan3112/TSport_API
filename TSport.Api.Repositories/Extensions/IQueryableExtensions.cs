using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
                if (!filterProperties.Any(p => p.Name == requestProperty.Name)) {
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
    }
}