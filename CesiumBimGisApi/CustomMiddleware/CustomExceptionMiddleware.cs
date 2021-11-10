using Cesium.IServices.System;
using Cesium.Models.System;
using Cesium.ViewModels.ResultModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CesiumBimGisApi.CustomMiddleware
{
    /// <summary>
    /// 自定义异常中间件
    /// </summary>
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        private readonly ISysLogExService _sysLogExService;
        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger, ISysLogExService sysLogExService)
        {
            _next = next;
            _logger = logger;
            _sysLogExService = sysLogExService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            SysLogEx log = new SysLogEx
            {
                Account = context.User?.FindFirstValue("UserId"),
                Name = context.User?.FindFirstValue(ClaimTypes.Name),
                ClassName = ex.TargetSite.DeclaringType?.FullName,
                MethodName = ex.TargetSite.Name,
                ExceptionName = ex.Message,
                ExceptionMsg = ex.Message,
                ExceptionSource = ex.Source,
                StackTrace = ex.StackTrace,
                ParamsObj = ex.TargetSite.GetParameters().ToString(),
                ExceptionTime = DateTime.Now
            };
            await _sysLogExService.AddLog(log);//异常日志记录到数据库

            //异常后,接口返回失败数据
            HttpResponse response = context.Response;
            BaseResult result = new BaseResult
            {
                isSuccess = false,
                code = ResultCodeMsg.CommonExceptionCode,
                message = ResultCodeMsg.CommonExceptionMsg,
            };

            response.ContentType = "application/json";
            await response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }

    /// <summary>
    /// 自定义异常中间件扩展类
    /// </summary>
    public static class CustomExceptionMiddlewareExtensions
    {
        /// <summary>
        /// 自定义异常中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
