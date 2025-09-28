using Listening.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Listening.Infrastructure.Repository
{
    public class CommonQuery(AppDbContext dbContext)
    {
        public IQueryable<T> Query<T>() where T : class
        {
            return dbContext.Set<T>().AsQueryable();
        }
        public IQueryable<T> ApplyQueryWithPermission<T>(bool isAnd=false) where T : class
        {
            return dbContext.Set<T>().ApplyRowPermission(isAnd);
        } 
    }
}
