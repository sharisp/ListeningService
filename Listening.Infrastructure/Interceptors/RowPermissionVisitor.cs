using Domain.SharedKernel.Interfaces;
using Listening.Infrastructure.Options;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Domain.SharedKernel.BaseEntity;
using Listening.Infrastructure.Repository;

namespace Listening.Infrastructure.Interceptors;
public class RowPermissionVisitor : ExpressionVisitor
{
    private readonly AppDbContext _dbContext;
    private readonly string _dbName;

    public RowPermissionVisitor(AppDbContext dbContext, string dbName)
    {
        _dbContext = dbContext;
        _dbName = dbName;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        var elementType = node.Method.GetGenericArguments().FirstOrDefault();
        if (elementType == null || !typeof(BaseAuditableEntity).IsAssignableFrom(elementType))
            return base.VisitMethodCall(node);

        var param = Expression.Parameter(elementType, "e");

        // 构建动态权限表达式
        var permissionPredicate = BuildRowPermissionPredicate(param, elementType);
        if (permissionPredicate == null)
            return base.VisitMethodCall(node);

        // 提取已有 Where 并合并参数
        var existingLambda = ExtractExistingWhere(node);
        if (existingLambda != null)
        {
            var replacer = new ParameterReplacer(existingLambda.Parameters[0], param);
            var fixedBody = replacer.Visit(existingLambda.Body);
            permissionPredicate = Expression.AndAlso(fixedBody, permissionPredicate);
        }

        var lambda = Expression.Lambda(permissionPredicate, param);

        var whereMethod = typeof(Queryable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
            .MakeGenericMethod(elementType);

        return Expression.Call(whereMethod, node, Expression.Quote(lambda));
    }

    private Expression? BuildRowPermissionPredicate(ParameterExpression param, Type elementType)
    {
        var entityMetadata = _dbContext.Model.FindEntityType(elementType);
        if (entityMetadata == null) return null;

        var schema = entityMetadata.GetSchema() ?? "dbo";
        var tableName = entityMetadata.GetTableName() ?? elementType.Name;
        var dbName = _dbContext.Database.GetDbConnection().Database ?? _dbName;
        var fullTableName = $"{dbName}.{schema}.{tableName}".ToLowerInvariant();

        // 动态获取当前用户权限列表
        var tablePermissions = _dbContext.CurrentUserPermissions
            .Where(p => string.Equals(p.FullTableName, fullTableName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        Expression? combinedPredicate = null;

        // 默认 IsDel 条件
        combinedPredicate = Expression.Equal(
            Expression.Property(param, nameof(BaseAuditableEntity.IsDel)),
            Expression.Constant(false)
        );

        Expression? rowPermissionPredicate = null;

        foreach (var permission in tablePermissions)
        {
            Expression? rowFilter = null;

            // Personal 权限
            if (permission.DataScopeType == RowDataScopeEnum.Personal &&
                typeof(ICreatorUserId).IsAssignableFrom(elementType))
            {
                // 使用 DbContext.CurrentUserId 动态获取
                var currentUserProperty = Expression.Property(
                    Expression.Constant(_dbContext), nameof(AppDbContext.CurrentUserId)
                );

                rowFilter = Expression.Equal(
                    Expression.Property(param, nameof(ICreatorUserId.CreatorUserId)),
                    currentUserProperty
                );
            }
            // Custom 权限
            else if (permission.DataScopeType == RowDataScopeEnum.Custom &&
                     !string.IsNullOrEmpty(permission.ScopeField))
            {
                var pi = elementType.GetProperty(permission.ScopeField,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (pi != null)
                {
                    // 使用动态 ScopeValue
                    var scopeValueProperty = Expression.Property(
                        Expression.Constant(permission), nameof(RowPermissionBlackList.ScopeValue)
                    );

                    var convertedValue = Expression.Convert(scopeValueProperty, pi.PropertyType);
                    rowFilter = Expression.Equal(
                        Expression.Property(param, pi),
                        convertedValue
                    );
                }
            }

            if (rowFilter != null)
                rowPermissionPredicate = rowPermissionPredicate == null
                    ? rowFilter
                    : Expression.OrElse(rowPermissionPredicate, rowFilter);
        }

        if (rowPermissionPredicate != null)
            combinedPredicate = Expression.AndAlso(combinedPredicate, rowPermissionPredicate);

        return combinedPredicate;
    }

    private LambdaExpression? ExtractExistingWhere(MethodCallExpression node)
    {
        if (node.Method.Name == "Where" && node.Arguments.Count == 2)
        {
            var lambda = StripQuotes(node.Arguments[1]) as LambdaExpression;
            return lambda;
        }
        return null;
    }

    private static Expression StripQuotes(Expression e)
    {
        while (e.NodeType == ExpressionType.Quote)
            e = ((UnaryExpression)e).Operand;
        return e;
    }
}

// 替换 Lambda 参数
public class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _oldParam;
    private readonly ParameterExpression _newParam;

    public ParameterReplacer(ParameterExpression oldParam, ParameterExpression newParam)
    {
        _oldParam = oldParam;
        _newParam = newParam;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == _oldParam ? _newParam : node;
    }
}







