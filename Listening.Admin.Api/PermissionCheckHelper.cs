using Identity.Domain.Constants;
using Infrastructure.SharedKernel;
using Listening.Infrastructure.Helper;
using Listening.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Admin.Api
{
    public class PermissionCheckHelper(IConfiguration configuration, ApiClientHelper apiClientHelper, IHttpContextAccessor httpContextAccessor)
    {

        public async Task<bool> CheckPermission(long userId, string permissionKey, CancellationToken cancellation = default)
        {

            var authHeader = Convert.ToString(httpContextAccessor.HttpContext?.Request.Headers["Authorization"]);
            if (string.IsNullOrEmpty(authHeader))
            {
                return false;
            }
            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring(7) : authHeader;
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            var url = configuration["OuterApiUrl:CheckPermissionApiUrl"];
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            var dto = new
            {
                PermissionKey = permissionKey,
                UserId = userId,
                SystemName = ConstantValue.SystemName
            };
            apiClientHelper.SetBearerToken(token);
            var res = await apiClientHelper.PostAsync<ApiResponse<string>>(url, dto);
            if (res?.Success == true)
            {
                return true;
            }
            return false;

        }

        public async Task<DataPermission?> QueryDataPermissionBlackList(long roleId, CancellationToken cancellation = default)
        {

            var authHeader = Convert.ToString(httpContextAccessor.HttpContext?.Request.Headers["Authorization"]);
            if (string.IsNullOrEmpty(authHeader))
            {
                return null;
            }
            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring(7) : authHeader;
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            var url = configuration["OuterApiUrl:DataPermissionApiUrl"];
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

           
            apiClientHelper.SetBearerToken(token);
            var res = await apiClientHelper.GetAsync<ApiResponse<DataPermission>>(url+"/"+roleId);
          
            if (res?.Success == true)
            {
                return res.Data;
            }
            return null;

        }
    }
}
