using Cesium.Core.Extensions;
using Cesium.Models.System;
using Cesium.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices.System
{
    public interface ISysUserRoleService : IDependency
    {
        /// <summary>
        /// 根据用户编号获取用户角色信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<SysUserRole>> GetUserRoleListAsync(int userId);

        /// <summary>
        /// 获取所有用户角色信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SysUserRole>> GetUserRoleListAsync();

        /// <summary>
        /// 添加用户角色信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult> AddUserRoleInfo(int userId);

        /// <summary>
        /// 根据用户编号删除用户角色信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult> DeleteUserRoleInfo(int userId);
    }
}
