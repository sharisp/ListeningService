using Listening.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginationResponse<T>> ToPaginationResponseAsync<T>(
     this IQueryable<T> source, int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var total = await source.CountAsync();

            var dataList = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResponse<T>(total,
               dataList);

            ;
        }

        public static IQueryable<T> ToPaginationQueryAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0 || pageSize <= 0)
            {
                return source;
            }

            return source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
