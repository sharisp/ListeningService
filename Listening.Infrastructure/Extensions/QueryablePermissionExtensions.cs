using Domain.SharedKernel.BaseEntity;
using Domain.SharedKernel.Interfaces;
using Listening.Infrastructure.Options;
using Listening.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Listening.Infrastructure.Extensions
{
    
 
    public static class QueryablePermissionExtensions
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static IQueryable<T> ApplyRowPermission<T>(this IQueryable<T> query, bool isAnd = false)
            where T : class
        {
            if (_httpContextAccessor?.HttpContext == null)
                return query;

            var httpContext = _httpContextAccessor.HttpContext;
            var dbContext = httpContext.RequestServices.GetRequiredService<AppDbContext>();
            var currentUser = httpContext.RequestServices.GetRequiredService<ICurrentUser>();

            var entityType = typeof(T);
            var param = Expression.Parameter(entityType, "e");

            Expression? predicate = null;

           

            // 确定表名
            var entityMetadata = dbContext.Model.FindEntityType(entityType);
            if (entityMetadata == null) return query;

            var schema = entityMetadata.GetSchema() ?? "dbo";
            var tableName = entityMetadata.GetTableName();
            var dbName = dbContext.Database.GetDbConnection().Database;
            var fullTableName = $"{dbName}.{schema}.{tableName}".ToLowerInvariant();

            // 获取用户权限
            var tablePermissions = (httpContext.Items["RowDataPermission"] as List<RowPermissionList> ?? new List<RowPermissionList>())
                .Where(p => string.Equals(p.FullTableName, fullTableName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var permission in tablePermissions)
            {
                if (!permission.RowDataAllowOperateType.HasFlag(RowDataAllowOperateEnum.Read))
                {
                    continue;
                }
                Expression? filter = null;

                if (permission.DataScopeType == RowDataScopeEnum.Personal &&
                    typeof(ICreatorUserId).IsAssignableFrom(entityType))
                {
                    var prop = Expression.Property(param, nameof(ICreatorUserId.CreatorUserId));
                    var userId = Expression.Constant(currentUser.UserId, typeof(long?));
                    filter = Expression.Equal(prop, userId);
                }
                else if (permission.DataScopeType == RowDataScopeEnum.Custom &&
                         !string.IsNullOrEmpty(permission.ScopeField))
                {
                    var pi = entityType.GetProperty(permission.ScopeField,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                    if (pi != null)
                    {
                        var prop = Expression.Property(param, pi);
                        var constExpr = Expression.Constant(
                            Convert.ChangeType(permission.ScopeValue, pi.PropertyType),
                            pi.PropertyType);
                        filter = Expression.Equal(prop, constExpr);
                    }
                }

                if (filter != null)
                {
                    //use or,not and
                    predicate = predicate == null
                        ? filter
                        : (isAnd? Expression.AndAlso(predicate, filter) : Expression.OrElse(predicate, filter));
                }
            }

            if (predicate == null)
                return query;

            var lambda = Expression.Lambda<Func<T, bool>>(predicate, param);
            return query.Where(lambda);
        }
    }


}
