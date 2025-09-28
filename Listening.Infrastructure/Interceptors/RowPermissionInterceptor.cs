using Domain.SharedKernel.Interfaces;
using Listening.Infrastructure.Interceptors;
using Listening.Infrastructure.Options;
using Listening.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;

namespace Listening.Infrastructure.Interceptors;

public class RowPermissionInterceptor : IQueryExpressionInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _dbContext;


    public RowPermissionInterceptor(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;

    }

    // EF Core 9.0 的方法签名
    public Expression QueryCompilationStarting(Expression queryExpression, QueryExpressionEventData eventData)
    {
        // 动态获取当前用户权限
        var currentUserPermissions =
            _httpContextAccessor.HttpContext?.Items["RowPermissionBlackList"]
            as List<RowPermissionBlackList> ?? new List<RowPermissionBlackList>();

        // 当前用户 ID
        var currentUserId =
            _httpContextAccessor.HttpContext?.Items["CurrentUserId"] as long? ??
            _dbContext.CurrentUserId;

        // 更新 DbContext 属性，供 Visitor 使用
        _dbContext.CurrentUserId = currentUserId;
        _dbContext.CurrentUserPermissions = currentUserPermissions;

        // 如果没有权限就直接返回原查询
        if (!currentUserPermissions.Any())
            return queryExpression;

        // 访问 Expression，动态添加行权限
        string _dbName = eventData.Context?.Database.GetDbConnection().Database ?? string.Empty;
        var visitor = new RowPermissionVisitor(_dbContext, _dbName);
        var newExpression = visitor.Visit(queryExpression);

        return newExpression ?? queryExpression;
    }
}
