using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Models.ResponseModels;

namespace TSport.Api.Repositories.Extensions
{
    public static class IQueryableExtensions
    {
        public static PagedResultResponse<T> ToPagedResultResponseAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        where T : class
        {
            return new PagedResultResponse<T>
            {
                TotalCount = query.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
            };
        }
    }
}