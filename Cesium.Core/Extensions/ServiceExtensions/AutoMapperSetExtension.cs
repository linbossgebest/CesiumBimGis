using Cesium.Core.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Cesium.Core.Extensions.ServiceExtensions
{
    public static class AutoMapperSetExtension
    {
        /// <summary>
        /// 添加AutoMapper服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoMapperSet(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(AutoMapperConfig));
            AutoMapperConfig.RegisterMappings();
        }
    }
}
