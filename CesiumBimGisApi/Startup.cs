using Autofac;
using Cesium.Core;
using Cesium.Core.Extensions;
using Cesium.Core.Extensions.ServiceExtensions;
using Cesium.Core.Helper;
using Cesium.Core.Hubs;
using Cesium.Core.Options;
using CesiumBimGisApi.CustomMiddleware;
using CesiumBimGisApi.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CesiumBimGisApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IServiceCollection Services { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new AppSettingsHelper(Configuration));
            services.Configure<DbOption>("DbOption", Configuration.GetSection("DbOption"));
            services.Configure<JWTOption>("JWTOption", Configuration.GetSection("JWTConfigurations"));

            services.AddRedisCacheSet();
            services.AddAutoMapperSet();
            services.AddRabbitMQSet();
            services.AddAuthentication_JWTSet();

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(RequestActionFilter));
            }).AddFluentValidation(options =>
               {
                   options.DisableDataAnnotationsValidation = true;
                   var validatorList = FluentValidationSetExtension.GetFluentValidationValidator("Cesium.Core");
                   foreach (var item in validatorList)
                   {
                       //批量注入Validators
                       options.RegisterValidatorsFromAssemblyContaining(item);
                   }
               });

            services.AddFluentValidationErrorMessage();

            services.AddSwaggerSet();
            services.AddCorsSet();
            services.AddSignalR().AddNewtonsoftJsonProtocol();//防止SignalR乱码
        }

    
        public void ConfigureContainer(ContainerBuilder builder)
        {
            Services.AddModule(builder, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CesiumBimGisApi v1"));
            }

            app.UseCustomExceptionMiddleware();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = (c) =>
                {
                    c.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");//静态文件跨域访问
                },
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseCors(AppSettingsHelper.app(new string[] { "Cors", "PolicyName" }));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
               {
                   endpoints.MapControllers();

                   endpoints.MapHub<ChatHub>("/api/chatHub");//配置集线器路由地址
               });
        }
    }
}
