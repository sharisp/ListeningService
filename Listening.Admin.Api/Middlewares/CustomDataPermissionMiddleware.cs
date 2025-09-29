using Domain.SharedKernel.Interfaces;
using Infrastructure.SharedKernel;
using Listening.Admin.Api.Helpers;
using Listening.Infrastructure.Options;
using Listening.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Listening.Admin.Api.Middlewares;


public class CustomDataPermissionMiddleware(
    RequestDelegate next,
    ILogger<CustomDataPermissionMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            if (AppHelper.ReadAppSettingsSection<bool>("Enable:DataPermission")==false)
            {
                await next(context);
                return;
            }  
            if (context == null) throw new ArgumentNullException(nameof(context));
            var rolIdstr = context?.User.FindFirstValue("RoleIds");
            if (string.IsNullOrEmpty(rolIdstr))
            {
                await next(context);
                return;
            }
            var rolIds = rolIdstr.Split(',');
            List<RowPermissionList> rowPermissions = new List<RowPermissionList>();
            if (rolIds != null && rolIds.Any())
            {

                var memoryCacheHelper = context.RequestServices.GetRequiredService<MemoryCacheHelper>();
                var permissionHelper = context.RequestServices.GetRequiredService<PermissionCheckHelper>();
                foreach (var roleId in rolIds)
                {
                    var dataPermissionBlackList = await memoryCacheHelper.GetOrCreateAsync<DataPermission?>($"DataPermission_Role" + roleId, async entry =>
                    {
                        return await permissionHelper.QueryDataPermissionBlackList(Convert.ToInt64(roleId));

                    }, 5 * 60);
                    if (dataPermissionBlackList != null)
                    {
                        if (dataPermissionBlackList.RowPermissions != null && dataPermissionBlackList.RowPermissions.Count > 0)
                        {
                            rowPermissions.AddRange(dataPermissionBlackList.RowPermissions);
                        }
                    }
                }


            }
            context.Items["RowDataPermission"] = rowPermissions;

            await next(context);
        }
        catch (Exception ex)
        {
            //  httpContext.Response.ContentType = "application/problem+json";

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            logger.LogError("WebApi——异常", ex);
            var res = ApiResponse<string>.Fail(ex.Message);

            await context.Response.WriteAsJsonAsync(res);

        }
    }

    private bool CheckWhiteList(string url)
    {
        var whiteList = AppHelper.ReadAppSettingsSection<List<string>>("WhiteList");

        if (whiteList == null || whiteList.Count == 0) return false;

        foreach (var urlitem in whiteList)
            if (urlitem.Trim('/').Equals(url.Trim('/'), StringComparison.OrdinalIgnoreCase))
                return true;
        /*  if (Urlitem.Url.IndexOf("****") > 0)
              {
                  string UrlitemP = Urlitem.url.Replace("****", "");
                  if (Regex.IsMatch(url, UrlitemP, RegexOptions.IgnoreCase)) return true;
                  if (url.Length >= UrlitemP.Length && UrlitemP.ToLower() == url.Substring(0, UrlitemP.Length).ToLower()) return true;

              }*/
        return false;
    }
}