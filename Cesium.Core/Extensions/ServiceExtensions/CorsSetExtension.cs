using Cesium.Core.Helper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Core.Extensions.ServiceExtensions
{
    public static class CorsSetExtension
    {
        /// <summary>
        /// 添加cors 服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddCorsSet(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddCors(options =>
            {
                if (AppSettingsHelper.app(new string[] { "Cors", "EnableAllIPs" }).ObjToBool())
                {
                    options.AddPolicy(AppSettingsHelper.app(new string[] { "Cors", "PolicyName" }), builder =>
                    {
                        builder.AllowAnyMethod()
                               .AllowAnyHeader()
                               .SetIsOriginAllowed(_ => true) //= AllowAnyOrigin()
                               .AllowCredentials();
                    });
                }
                else
                {
                    options.AddPolicy(AppSettingsHelper.app(new string[] { "Cors", "PolicyName" }),

                       policy =>
                       {

                           policy
                           .WithOrigins(AppSettingsHelper.app(new string[] { "Cors", "IPs" }).Split(','))
                           .AllowAnyHeader()//Ensures that the policy allows any header.
                           .AllowAnyMethod();
                       });
                }
            });

        }
    }
}
