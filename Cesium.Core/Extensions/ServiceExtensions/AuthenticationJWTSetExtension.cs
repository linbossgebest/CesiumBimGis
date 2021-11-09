using Cesium.Core.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Core.Extensions.ServiceExtensions
{
    public static class AuthenticationJWTSetExtension
    {
        /// <summary>
        /// 添加jwt 服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthentication_JWTSet(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var Issuer = AppSettingsHelper.app(new string[] { "JWTConfigurations", "Issuer" });
            var Audience = AppSettingsHelper.app(new string[] { "JWTConfigurations", "Audience" });
            var SecurityKey = AppSettingsHelper.app(new string[] { "JWTConfigurations", "SecurityKey" });

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                // 令牌验证参数
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = Audience,//Audience
                    ValidIssuer = Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    //ValidAudience = tokenConfigurations.Audience,//Audience
                    //ValidIssuer = tokenConfigurations.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))//拿到SecurityKey
                };
            });
        }
    }
}
