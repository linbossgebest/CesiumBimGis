using Cesium.Core;
using Cesium.Core.Helper;
using Cesium.Core.Options;
using Cesium.IServices;
using Cesium.IServices.System;
using Cesium.Models;
using Cesium.Models.System;
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
using UAParser;

namespace CesiumBimGisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;//http上下文
        private readonly ISysUserService _userService;//用户服务
        private readonly ISysRoleService _roleService;//角色服务
        private readonly ISysAuthMenuService _sysAuthMenuService;//权限服务
        private readonly IConfiguration _configuration;//配置
        private readonly ISysAppMenuService _sysAppMenuService;//app菜单服务
        private readonly ISysUserRoleService _sysUserRoleService;//用户角色服务
        private readonly ISysRoleMenuService _sysRoleMenuService;//角色菜单服务
        private readonly ISysLogOpService _sysLogOpService;//操作日志服务
        private readonly ISysLogExService _sysLogExService;//异常日志服务
        private readonly ISysLogVisService _sysLogVisService;//访问日志服务
        private readonly DbOption _option;
        private readonly JWTOption _JWToption;

        public AccountController(IHttpContextAccessor httpContextAccessor,ISysUserService userService, ISysRoleService roleService, ISysAuthMenuService sysAuthMenuService, ISysAppMenuService sysAppMenuService, ISysUserRoleService sysUserRoleService, ISysRoleMenuService sysRoleMenuService, ISysLogOpService sysLogOpService, ISysLogExService sysLogExService, ISysLogVisService sysLogVisService,IOptionsSnapshot<DbOption> option, IConfiguration configuration, IOptionsSnapshot<JWTOption> JWToption)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _roleService = roleService;
            _sysAuthMenuService = sysAuthMenuService;
            _sysAppMenuService = sysAppMenuService;
            _sysUserRoleService = sysUserRoleService;
            _sysRoleMenuService = sysRoleMenuService;
            _sysLogOpService = sysLogOpService;
            _sysLogExService = sysLogExService;
            _sysLogVisService = sysLogVisService;
            _option = option.Get("DbOption");
            _JWToption = JWToption.Get("JWTOption");
            _configuration = configuration;

        }

        #region 用户信息

        /// <summary>
        /// 添加用户或者修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddUser")]
        public async Task<string> AddOrModifyUser([FromBody] UserModel model)
        {
            var info = HttpContext.AuthenticateAsync().Result.Principal.Claims;//获取用户身份信息
            TokenInfo tokenInfo = new()
            {
                UserId = Int32.Parse(info.FirstOrDefault(f => f.Type.Equals("UserId")).Value),
                UserName = info.FirstOrDefault(f => f.Type.Equals(ClaimTypes.Name)).Value
            };
            var result = await _userService.AddOrModifyUserAsync(model, tokenInfo);
            return JsonHelper.ObjectToJSON(result);
        }


        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页的数量</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserList")]
        public async Task<ResponseResult> GetUserInfoList(int pageIndex, int pageSize)
        {
            ResponseResult result = new ResponseResult();
            var users = await _userService.GetUsersAsync();
            var total = users.Count();

            users = users.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var data = new
            {
                items = users,
                total = total
            };

            result.isSuccess = true;
            result.code = ResultCodeMsg.CommonSuccessCode;
            result.message = ResultCodeMsg.CommonSuccessMsg;
            result.data = JsonHelper.ObjectToJSON(data);

            return result;
        }

        /// <summary>
        /// 根据userid删除用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteUser")]
        public async Task<ResponseResult> DeleteUserInfo(int userId)
        {
            return await _userService.DeleteUserInfo(userId);
        }

        /// <summary>
        /// 获取单个用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserInfo")]
        public async Task<ResponseResult> GetUserInfo()
        {
            ResponseResult result = new ResponseResult();
            var claimInfo = HttpContext.AuthenticateAsync().Result.Principal.Claims;//获取用户身份信息
            var userId = Int32.Parse(claimInfo.FirstOrDefault(f => f.Type.Equals("UserId")).Value);
            //var roleId = Int32.Parse(claimInfo.FirstOrDefault(f => f.Type.Equals(ClaimTypes.Role)).Value);
            var user = await _userService.GetUserInfoAsync(userId);
            var userRoles = await _sysUserRoleService.GetUserRoleListAsync(userId);
            List<int> roles = new List<int>();
            foreach (var item in userRoles)
            {
                roles.Add(item.RoleId);
            }
            //var role = await _roleService.GetRoleInfoByUserId(roleId);
            if (user != null)
            {
                var data = new
                {
                    name = user.UserName,
                    roles = roles,
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

        /// <summary>
        /// 用户登录操作  获取bearer token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<ResponseResult> Login([FromBody] LoginModel model)
        {
            ResponseResult result = new ResponseResult();
            var user = await _userService.SignInAsync(model);
            if (user != null)
            {
                var claims = new[]
               {
                   new Claim(ClaimTypes.Name, user.UserName),
                   new Claim("UserId", user.Id.ToString()),
                  // new Claim(ClaimTypes.Role, user.RoleId.ToString())
               };
                //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
                //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                var securitykey= AppSettingsHelper.app(new string[] { "JWTConfigurations", "SecurityKey" }); 
                var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitykey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
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
                    expires: DateTime.Now.AddMinutes(1200),
                    signingCredentials: creds);

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);


                var data = new
                {
                    username = model.username,
                    token = accessToken,
                    expires_in = 1200 * 60
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);

                #region 添加登录访问日志

                SysLogVis log = SetSysLogVis(_httpContextAccessor, "登录成功", LoginType.Login, user);
                await _sysLogVisService.AddLog(log);

                #endregion

            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.SignInPasswordOrUserNameErrorCode;
                result.message = ResultCodeMsg.SignInPasswordOrUserNameErrorMsg;
            }
            return result;
        }

       

        /// <summary>
        /// 用户退出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Logout")]
        public async Task<ResponseResult> Logout()
        {
            ResponseResult result = new ResponseResult();

            result.isSuccess = true;
            result.code = ResultCodeMsg.CommonSuccessCode;
            result.message = ResultCodeMsg.CommonSuccessMsg;

            SysLogVis log = SetSysLogVis(_httpContextAccessor, "退出成功", LoginType.LogOut);
            await _sysLogVisService.AddLog(log);

            return result;
        }

        #endregion

        #region 用户角色信息

        /// <summary>
        /// 获取所有用户角色信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserRoleList")]
        public async Task<ResponseResult> GetUserRoleInfoList()
        {
            ResponseResult result = new ResponseResult();
            var userRoles = await _sysUserRoleService.GetUserRoleListAsync();

            if (userRoles != null)
            {
                var data = new

                {
                    items = userRoles,
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }
     

            return result;
        }

        /// <summary>
        /// 根据用户编号获取用户角色信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserRoleListById")]
        public async Task<ResponseResult> GetUserRoleInfoByUserId(int userId)
        {
            ResponseResult result = new ResponseResult();
            var userRoles = await _sysUserRoleService.GetUserRoleListAsync(userId);

            if (userRoles != null)
            {
                var data = new
                {
                    items = userRoles,
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }


            return result;
        }

        #endregion

        #region 角色信息

        /// <summary>
        /// 获取所有角色信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRoleList")]
        public async Task<ResponseResult> GetRoleInfoList()
        {
            ResponseResult result = new ResponseResult();
            var roles = await _roleService.GetRolesAsync();

            if (roles != null)
            {
                var data = new
                {
                    items = roles,
                    total = roles.Count()
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }


            return result;
        }

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteRole")]
        public async Task<ResponseResult> DeleteRoleInfo(int roleId)
        {
            return await _roleService.DeleteRoleInfo(roleId);
        }

        [HttpPost]
        [Route("AddRole")]
        public async Task<ResponseResult> AddOrUpdateSysRole(RoleModel model)
        {
            var info = HttpContext.AuthenticateAsync().Result.Principal.Claims;//获取用户身份信息
            TokenInfo tokenInfo = new()
            {
                UserId = Int32.Parse(info.FirstOrDefault(f => f.Type.Equals("UserId")).Value),
                UserName = info.FirstOrDefault(f => f.Type.Equals(ClaimTypes.Name)).Value
            };
            var result = await _roleService.AddOrModifyRoleAsync(model, tokenInfo);
            return result;
        }


        #endregion

        #region 菜单信息

        /// <summary>
        /// 新增或修改菜单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddMenu")]
        public async Task<ResponseResult> AddOrModifyMenuInfo([FromBody] SysAuthMenu model)
        {
            var info = HttpContext.AuthenticateAsync().Result.Principal.Claims;//获取用户身份信息
            TokenInfo tokenInfo = new()
            {
                UserId = Int32.Parse(info.FirstOrDefault(f => f.Type.Equals("UserId")).Value),
                UserName = info.FirstOrDefault(f => f.Type.Equals(ClaimTypes.Name)).Value
            };
            var result = await _sysAuthMenuService.AddOrModifyMenuAsync(model, tokenInfo);

            return result;
        }

        /// <summary>
        /// 删除菜单及对应子菜单信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteMenu")]
        public async Task<ResponseResult> DeleteMenuInfo(int menuId)
        {
            return await _sysAuthMenuService.DelelteMenuInfo(menuId);
        }

        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMenuInfo")]
        public async Task<ResponseResult> GetMenuInfo()
        {
            ResponseResult result = new ResponseResult();

            var menu = await _sysAuthMenuService.GetMenuInfo();

            if (menu != null)
            {
                var data = new
                {
                    menu
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

        /// <summary>
        /// 根据角色Id获取菜单tree
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMenuTreeByRole")]
        public async Task<ResponseResult> GetMenuTreeByRole(int roleId)
        {
            ResponseResult result = new ResponseResult();
            List<int> menuIds = new List<int>();
            List<MenuTree> menuTree = new List<MenuTree>();

            var roleMenus = await _sysRoleMenuService.GetSysRoleMenuByRole(roleId);
            if (roleMenus != null && roleMenus.Any())
            {
                foreach (var item in roleMenus)
                {
                    menuIds.Add(item.MenuId);
                }
            }

            if (menuIds.Count > 0)
            {
                var menu = await _sysAuthMenuService.GetMenuInfo(menuIds);
                menuTree = await _sysAuthMenuService.GetMenuTree(menu.OrderBy(f=>f.OrderNo).ToList());
            }

            var data = new
            {
                menuTree
            };

            result.isSuccess = true;
            result.code = ResultCodeMsg.CommonSuccessCode;
            result.message = ResultCodeMsg.CommonSuccessMsg;
            result.data = JsonHelper.ObjectToJSON(data);

            return result;
        }

      

        [HttpGet]
        [Route("GetMenuTree")]
        [AllowAnonymous]
        public async Task<ResponseResult> GetMenuTree()
        {
            ResponseResult result = new ResponseResult();

            var menu = await _sysAuthMenuService.GetMenuInfo();
            var menuTree = await _sysAuthMenuService.GetMenuTree(menu.ToList());

            if (menuTree != null)
            {
                var data = new
                {
                    menuTree
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

        [HttpGet]
        [Route("GetAppMenu")]
        [AllowAnonymous]
        public async Task<ResponseResult> GetAppMenuList(int type)
        {
            ResponseResult result = new ResponseResult();

            var menu = await _sysAppMenuService.GetSysAppMenu(type);

            if (menu != null)
            {
                var data = new
                {
                    menu
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }


        /// <summary>
        /// 查询所有App菜单
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页的数量</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllAppMenu")]
        public async Task<ResponseResult> GetAllAppMenuList(int pageIndex, int pageSize)
        {
            ResponseResult result = new ResponseResult();

            var menus = await _sysAppMenuService.GetAllSysAppMenu();
            var total = menus.Count();

            menus = menus.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            if (menus != null)
            {
                var data = new
                {
                    items = menus,
                    total = total
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }




        #endregion

        #region 日志信息

        /// <summary>
        /// 查询操作日志信息
        /// </summary>
        /// <param name="logName">日志名称</param>
        /// <param name="requestMethod">请求方式</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOptionLogs")]
        public async Task<ResponseResult> GetOptionLogs(string logName, string requestMethod, int pageIndex, int pageSize)
        {
            ResponseResult result = new ResponseResult();
            var logs = await _sysLogOpService.GetLogs(logName, requestMethod);
            var total = logs.Count();

            logs=logs.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            if (logs != null)
            {
                var data = new
                {
                    items = logs,
                    total = total
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

        /// <summary>
        /// 异常日志查询
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetExceptionLogs")]
        public async Task<ResponseResult> GetExceptionLogs(string className, string methodName, int pageIndex, int pageSize)
        {
            ResponseResult result = new ResponseResult();
            var logs = await _sysLogExService.GetLogs(className, methodName);
            var total = logs.Count();

            logs=logs.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            if (logs != null)
            {
                var data = new
                {
                    items = logs,
                    total = total
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

        /// <summary>
        /// 访问日志查询
        /// </summary>
        /// <param name="logName">日志名称</param>
        /// <param name="visType">登录类型</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetVisitLogs")]
        public async Task<ResponseResult> GetVisitLogs(string logName, int? visType, int pageIndex, int pageSize)
        {
            ResponseResult result = new ResponseResult();
            var logs = await _sysLogVisService.GetLogs(logName, visType);
            var total = logs.Count();

            logs = logs.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            if (logs != null)
            {
                var data = new
                {
                    items = logs,
                    total = total
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

        #endregion

        #region private method

        /// <summary>
        /// 设置访问日志类容
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private SysLogVis SetSysLogVis(IHttpContextAccessor httpContextAccessor, string message, LoginType type, SysUser user = null)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var httpRequest = httpContext.Request;
            var headers = httpRequest.Headers;
            var clientInfo = headers.ContainsKey("User-Agent")
                ? Parser.GetDefault().Parse(headers["User-Agent"])
                : null;
            string name = "";
            string account = "";
            if (httpContext.User != null)
            {
                name = httpContext.User.FindFirstValue(ClaimTypes.Name);
                account = httpContext.User.FindFirstValue("UserId");
            }
            if (user != null)
            {
                name = user.UserName;
                account = user.Id.ToString();
            }
            SysLogVis log = new SysLogVis
            {
                Name = name,
                Success = true,
                Message = message,
                Ip = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                Location = httpRequest.Path,
                Browser = clientInfo?.UA.Family + clientInfo?.UA.Major,
                Os = clientInfo?.OS.Family + clientInfo?.OS.Major,
                VisType = type,
                VisTime = DateTime.Now,
                Account = account
            };
            return log;
        }

        #endregion
    }
}
