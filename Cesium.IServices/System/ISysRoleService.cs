using Cesium.Core.Extensions;
using Cesium.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices.System
{
    public interface ISysRoleService: IDependency
    {
        /// <summary>
        /// 根据角色编号获取角色信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<SysRole> GetRoleInfoByUserId(int roleId);
    }
}
