using Cesium.Core.Extensions;
using Cesium.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices.System
{
    public interface ISysPermissionService: IDependency
    {
        /// <summary>
        /// 获取所有权限信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SysPermission>> GetPermissionInfo();

        /// <summary>
        /// 根据角色Id获取权限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<SysPermission>> GetPermissionInfoByRole(int roleId);
    }
}
