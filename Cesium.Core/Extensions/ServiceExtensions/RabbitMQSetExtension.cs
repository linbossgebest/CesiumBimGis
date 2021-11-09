using Cesium.Core.Helper;
using Cesium.Core.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Core.Extensions.ServiceExtensions
{
    public static class RabbitMQSetExtension
    {
        /// <summary>
        ///  添加RabbitMQ 连接服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddRabbitMQSet(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (AppSettingsHelper.app(new string[] { "RabbitMQ", "Enabled" }).ObjToBool())
            {

                services.AddSingleton<IRabbitMQConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<RabbitMQConnection>>();

                    var factory = new ConnectionFactory()
                    {
                        HostName = AppSettingsHelper.app(new string[] { "RabbitMQ", "Connection" }),
                        DispatchConsumersAsync = true
                    };

                    if (!string.IsNullOrEmpty(AppSettingsHelper.app(new string[] { "RabbitMQ", "UserName" })))
                    {
                        factory.UserName = AppSettingsHelper.app(new string[] { "RabbitMQ", "UserName" });
                    }

                    if (!string.IsNullOrEmpty(AppSettingsHelper.app(new string[] { "RabbitMQ", "Password" })))
                    {
                        factory.Password = AppSettingsHelper.app(new string[] { "RabbitMQ", "Password" });
                    }

                    var retryCount = 5;
                    if (!string.IsNullOrEmpty(AppSettingsHelper.app(new string[] { "RabbitMQ", "RetryCount" })))
                    {
                        retryCount = int.Parse(AppSettingsHelper.app(new string[] { "RabbitMQ", "RetryCount" }));
                    }

                    return new RabbitMQConnection(factory, logger, retryCount);
                });
            }
        }
    }
}
