using Cesium.Core.Extensions;
using Cesium.Models.System;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices
{
    public interface ISysUserService: IDependency
    {

        /// <summary>
        /// 登录操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SysUser> SignInAsync(LoginModel model);

        /// <summary>
        /// 添加或修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        Task<BaseResult> AddOrModifyUserAsync(UserModel model, TokenInfo tokenInfo);

        /// <summary>
        /// 通过用户编号获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<SysUser> GetUserInfoAsync(int userId);

        /// <summary>
        /// 获取所有的用户信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SysUser>> GetUsersAsync();

        /// <summary>
        /// 通过用户编号删除用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult> DeleteUserInfo(int userId);

    }
}
