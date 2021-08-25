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
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResult> AddUserAsync(UserModel model);

    }
}
