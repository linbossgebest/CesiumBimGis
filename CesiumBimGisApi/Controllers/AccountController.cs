using Cesium.Core;
using Cesium.Core.Helper;
using Cesium.Core.Options;
using Cesium.IServices;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CesiumBimGisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly ISysUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly DbOption _option;
        private readonly JWTOption _JWToption;

        public AccountController(ISysUserService userService, IOptionsSnapshot<DbOption> option, IConfiguration configuration, IOptionsSnapshot<JWTOption> JWToption)
        {
            _userService = userService;
            _option = option.Get("DbOption");
            _configuration = configuration;
            _JWToption = JWToption.Get("JWTOption");

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<string> Login([FromBody]LoginModel model)
        {
            BaseResult result = new BaseResult();
            var user = await _userService.SignInAsync(model);
            if (user == null)
            {
                result.IsSuccess = false;
                result.Code = ResultCodeMsg.CommonFailCode;
                result.Message = ResultCodeMsg.CommonFailMsg;
            }
            else
            {
                result.IsSuccess = true;
                result.Code = ResultCodeMsg.CommonSuccessCode;
                result.Message = ResultCodeMsg.CommonSuccessMsg;
            }

            return JsonHelper.ObjectToJSON(result);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<string> AddUser(UserModel model)
        {
            var info = HttpContext.AuthenticateAsync().Result.Principal.Claims;//获取用户身份信息
            TokenInfo tokenInfo = new() {
                UserId = Int32.Parse(info.FirstOrDefault(f => f.Type.Equals("UserId")).Value),
                UserName = info.FirstOrDefault(f => f.Type.Equals(ClaimTypes.Name)).Value
            };
            var result = await _userService.AddUserAsync(model, tokenInfo);
            return JsonHelper.ObjectToJSON(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("RequestToken")]
        public async Task<string> RequestToken([FromBody]LoginModel model)
        {
            BaseResult result = new BaseResult();
            var user = await _userService.SignInAsync(model);
            if (user != null)
            {
                var claims = new[]
               {
                   new Claim(ClaimTypes.Name, user.UserName),
                   new Claim("UserId", user.Id.ToString()),
                   new Claim(ClaimTypes.Role, user.RoleId.ToString())
               };
                //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
                /**
                 * Claims (Payload)
                    Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:
                    iss: The issuer of the token，token 是给谁的
                    sub: The subject of the token，token 主题
                    exp: Expiration Time。 token 过期时间，Unix 时间戳格式
                    iat: Issued At。 token 创建时间， Unix 时间戳格式
                    jti: JWT ID。针对当前 token 的唯一标识
                    除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
                 * */
                var token = new JwtSecurityToken(
                    issuer: _JWToption.Issuer,
                    audience: _JWToption.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);


                var data = new
                {
                    username = model.UserName,
                    access_token = accessToken,
                    expires_in = 1800
                };

                result.IsSuccess = true;
                result.Code = ResultCodeMsg.CommonSuccessCode;
                result.Message = ResultCodeMsg.CommonSuccessMsg;
                result.Data = JsonHelper.ObjectToJSON(data);

            }
            else
            {
                result.IsSuccess = false;
                result.Code = ResultCodeMsg.SignInPasswordOrUserNameErrorCode;
                result.Message = ResultCodeMsg.SignInPasswordOrUserNameErrorMsg;
            }
            return JsonHelper.ObjectToJSON(result);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            var info = HttpContext.AuthenticateAsync().Result.Principal.Claims;//获取用户身份信息
            return new string[] { "value1", "value2" };
        }

    }
}
