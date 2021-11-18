using Cesium.ViewModels.ResultModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Core.Extensions.ServiceExtensions
{
    public static class FluentValidationSetExtension
    {
        /// <summary>   
        /// 添加FluentValidation服务，返回400(报错信息)
        /// </summary>
        /// <param name="services"></param>
        public static void AddFluentValidationErrorMessage(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState
                    .Values
                    .SelectMany(x => x.Errors
                    .Select(p => p.ErrorMessage))
                    .ToList();
                    var result = new ResponseResult
                    {
                        code = ResultCodeMsg.CommonFailCode,
                        data = null,
                        message = string.Join(",", errors.Select(e => string.Format("{0}", e)).ToList()),
                        isSuccess = false
                    };

                    return new BadRequestObjectResult(result);
                };
            });
        }

        /// <summary>
        /// 注入指定程序集中的Fluent Validator
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetFluentValidationValidator(string assemblyName)
        {
            if (assemblyName == null)
                throw new ArgumentNullException(nameof(assemblyName));
            if (string.IsNullOrEmpty(assemblyName))
                throw new ArgumentNullException(nameof(assemblyName));

            var implementAssembly = Assembly.Load(assemblyName);
            if (implementAssembly == null)
            {
                throw new DllNotFoundException($"the dll ConferenceWebApi not be found");
            }
            var validatorList = implementAssembly.GetTypes().Where(e => e.Name.EndsWith("Validator"));
            return validatorList;
        }
    }
}
