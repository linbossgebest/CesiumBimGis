using Cesium.Core.Helper;
using Cesium.Core.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Core.Extensions.ServiceExtensions
{
    public static class RedisCacheSetExtension
    {
        /// <summary>
        /// 添加redis缓存服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddRedisCacheSet(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<IRedisCacheManager, RedisCacheManager>();

            // 配置启动Redis服务
            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                //获取连接字符串
                string redisConfiguration = AppSettingsHelper.app(new string[] { "Redis", "ConnectionString" });

                var configuration = ConfigurationOptions.Parse(redisConfiguration, true);

                configuration.ResolveDns = true;

                return ConnectionMultiplexer.Connect(configuration);
            });

        }
    }
}
