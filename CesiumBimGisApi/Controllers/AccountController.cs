using Cesium.Core;
using Cesium.Core.Helper;
using Cesium.Core.Options;
using Cesium.IServices;
using Cesium.IServices.System;
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

namespace CesiumBimGisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly ISysUserService _userService;
        private readonly ISysRoleService _roleService;
        private readonly ISysAuthMenuService _sysAuthMenuService;
        private readonly IConfiguration _configuration;
        private readonly ISysAppMenuService _sysAppMenuService;
        private readonly ISysUserRoleService _sysUserRoleService;
        private readonly ISysRoleMenuService _sysRoleMenuService;
        private readonly DbOption _option;
        private readonly JWTOption _JWToption;

        public AccountController(ISysUserService userService, ISysRoleService roleService, ISysAuthMenuService sysAuthMenuService, ISysAppMenuService sysAppMenuService, ISysUserRoleService sysUserRoleService, ISysRoleMenuService sysRoleMenuService,IOptionsSnapshot<DbOption> option, IConfiguration configuration, IOptionsSnapshot<JWTOption> JWToption)
        {
            _userService = userService;
            _roleService = roleService;
            _sysAuthMenuService = sysAuthMenuService;
            _sysAppMenuService = sysAppMenuService;
            _sysUserRoleService = sysUserRoleService;
            _sysRoleMenuService = sysRoleMenuService;
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
        public async Task<BaseResult> GetUserInfoList(int pageIndex, int pageSize)
        {
            BaseResult result = new BaseResult();
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
        public async Task<BaseResult> DeleteUserInfo(int userId)
        {
            return await _userService.DeleteUserInfo(userId);
        }

        /// <summary>
        /// 获取单个用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserInfo")]
        public async Task<BaseResult> GetUserInfo()
        {
            BaseResult result = new BaseResult();
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
        /// 用户登录获取bearer token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("RequestToken")]
        public async Task<BaseResult> RequestToken([FromBody] LoginModel model)
        {
            BaseResult result = new BaseResult();
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
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
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
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: creds);

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);


                var data = new
                {
                    username = model.username,
                    token = accessToken,
                    expires_in = 1800
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);

            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.SignInPasswordOrUserNameErrorCode;
                result.message = ResultCodeMsg.SignInPasswordOrUserNameErrorMsg;
            }
            //return JsonHelper.ObjectToJSON(result);
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
        public async Task<BaseResult> GetUserRoleInfoList()
        {
            BaseResult result = new BaseResult();
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
        public async Task<BaseResult> GetUserRoleInfoByUserId(int userId)
        {
            BaseResult result = new BaseResult();
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
        public async Task<BaseResult> GetRoleInfoList()
        {
            BaseResult result = new BaseResult();
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
        public async Task<BaseResult> DeleteRoleInfo(int roleId)
        {
            return await _roleService.DeleteRoleInfo(roleId);
        }

        [HttpPost]
        [Route("AddRole")]
        public async Task<BaseResult> AddOrUpdateSysRole(RoleModel model)
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
        public async Task<BaseResult> AddOrModifyMenuInfo([FromBody] SysAuthMenu model)
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
        public async Task<BaseResult> DeleteMenuInfo(int menuId)
        {
            return await _sysAuthMenuService.DelelteMenuInfo(menuId);
        }

        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMenuInfo")]
        public async Task<BaseResult> GetMenuInfo()
        {
            BaseResult result = new BaseResult();

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
        public async Task<BaseResult> GetMenuTreeByRole(int roleId)
        {
            BaseResult result = new BaseResult();
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
        public async Task<BaseResult> GetMenuTree()
        {
            BaseResult result = new BaseResult();

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
        public async Task<BaseResult> GetAppMenuList(int type)
        {
            BaseResult result = new BaseResult();

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
        public async Task<BaseResult> GetAllAppMenuList(int pageIndex, int pageSize)
        {
            BaseResult result = new BaseResult();

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

    }
}
