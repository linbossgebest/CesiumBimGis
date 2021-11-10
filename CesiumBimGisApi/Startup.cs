using Autofac;
using Cesium.Core;
using Cesium.Core.Extensions;
using Cesium.Core.Extensions.ServiceExtensions;
using Cesium.Core.Helper;
using Cesium.Core.Options;
using CesiumBimGisApi.CustomMiddleware;
using CesiumBimGisApi.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddAutoMapperSet();
            services.AddRabbitMQSet();
            services.AddAuthentication_JWTSet();

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(RequestActionFilter));
            });

            services.AddSwaggerSet();
            services.AddCorsSet();

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
                    c.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");//¾²Ì¬ÎÄ¼þ¿çÓò·ÃÎÊ
                },
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseCors(AppSettingsHelper.app(new string[] { "Cors", "PolicyName" }));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
               {
                   endpoints.MapControllers();
               });
        }
    }
}
