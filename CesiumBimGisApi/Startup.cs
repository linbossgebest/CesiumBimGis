using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cesium.Core;
using Cesium.Core.Extensions;
using Cesium.Core.Options;
using Cesium.IRepository;
using Cesium.IServices;
using Cesium.Respository;
using Cesium.Services;
using CesiumBimGisApi.CustomMiddleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

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

            //services.AddOptions();
            services.Configure<DbOption>("DbOption", Configuration.GetSection("DbOption"));
            services.Configure<JWTOption>("JWTOption", Configuration.GetSection("JWTConfigurations"));

            var tokenConfigurations = new JWTOption();
            new ConfigureFromConfigurationOptions<JWTOption>(
                Configuration.GetSection("JWTConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//�Ƿ���֤Issuer
                    ValidateAudience = true,//�Ƿ���֤Audience
                    ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                    ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                    ValidAudience = tokenConfigurations.Audience,//Audience
                    ValidIssuer = tokenConfigurations.Issuer,//Issuer���������ǰ��ǩ��jwt������һ��
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))//�õ�SecurityKey
                };
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CesiumBimGisApi", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // ��ȡxml�ļ�·��
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // ��ӿ�������ע�ͣ�true��ʾ��ʾ������ע��
                c.IncludeXmlComments(xmlPath, true);
            });

            //���cors ���� ���ÿ�����            
            services.AddCors(options =>
            {
                options.AddPolicy("default", builder =>
                {
                    builder.AllowAnyMethod()
                           .AllowAnyHeader()
                           .SetIsOriginAllowed(_ => true) //= AllowAnyOrigin()
                           .AllowCredentials();
                });
            });
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

            app.UseCustomExceptionMiddleware();//�Զ���ȫ���쳣�м��

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = (c) =>
                {
                    c.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");//��̬�ļ��������
                },
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseCors("default");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
               {
                   endpoints.MapControllers();
               });
        }
    }
}
