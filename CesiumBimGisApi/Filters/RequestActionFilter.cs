using Cesium.Core.Helper;
using Cesium.IServices.System;
using Cesium.Models.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using UAParser;

namespace CesiumBimGisApi.Filters
{
    /// <summary>
    /// Action操作过滤器 记录操作日志
    /// </summary>
    public class RequestActionFilter : IAsyncActionFilter
    {
        private readonly ISysLogOpService _sysLogOpService;
        private readonly ILogger<RequestActionFilter> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysLogOpService"></param>
        /// <param name="logger"></param>
        public RequestActionFilter(ISysLogOpService sysLogOpService, ILogger<RequestActionFilter> logger)
        {
            _sysLogOpService = sysLogOpService;
            _logger = logger;
        }

        /// <summary>
        /// 异步执行 Action完成后记录操作日志
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = new Stopwatch();
            sw.Start();
            var actionContext = await next();
            sw.Stop();

            var httpContext = context.HttpContext;
            var httpRequest = httpContext.Request;
            // 判断是否请求成功（没有异常就是请求成功）
            var isRequestSucceed = actionContext.Exception == null;
            var headers = httpRequest.Headers;
            var clientInfo = headers.ContainsKey("User-Agent")
                ? Parser.GetDefault().Parse(headers["User-Agent"])
                : null;
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            SysLogOp log = new SysLogOp()
            {
                Name = httpContext.User?.FindFirstValue(ClaimTypes.Name),
                Success = isRequestSucceed,
                Ip = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                Location = httpRequest.Path,
                Browser = clientInfo?.UA.Family + clientInfo?.UA.Major,
                Os = clientInfo?.OS.Family + clientInfo?.OS.Major,
                Url = httpRequest.Path,
                ClassName = context.Controller.ToString(),
                MethodName = actionDescriptor?.ActionName,
                ReqMethod = httpRequest.Method,
                Param = JsonHelper.ObjectToJSON(context.ActionArguments.Count < 1 ? "" : context.ActionArguments),
                Result = actionContext.Result?.GetType() == typeof(JsonResult) ? JsonHelper.ObjectToJSON(actionContext.Result) : "",
                ElapsedTime = sw.ElapsedMilliseconds,
                OpTime = DateTime.Now,
                Account = httpContext.User?.FindFirstValue("UserId")
            };
            await _sysLogOpService.AddLog(log);//日志记录到数据库中
            _logger.LogInformation(JsonHelper.ObjectToJSON(log));
        }
    }
}
