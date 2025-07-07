using Listening.Admin.Api;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api
{
    public static class ControllerBaseExtensions
    {
        public static OkObjectResult OkResponse(this ControllerBase controllerBase, long id, int statusCode = 200)
        {
            return controllerBase.Ok(ApiResponse<BaseResponse>.Ok(BaseResponse.Create(id), statusCode));
        }
        public static OkObjectResult OkResponse<T>(this ControllerBase controllerBase, T? data=default, int statusCode = 200)
        {
            return controllerBase.Ok(ApiResponse<T>.Ok(data, statusCode));
        }
        
        public static OkObjectResult FailResponse(this ControllerBase controllerBase,string errMsg,int statusCode = 500)
        {
            return controllerBase.Ok((ApiResponse<BaseResponse>.Fail(errMsg,statusCode)));
        }
        public static OkObjectResult FailResponse<T>(this ControllerBase controllerBase, string errMsg, T? data, int statusCode = 500)
        {
            return controllerBase.Ok(ApiResponse<T>.Fail(errMsg, statusCode, data));
        }
    }
}
