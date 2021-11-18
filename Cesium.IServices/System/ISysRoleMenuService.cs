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
    public interface ISysRoleMenuService: IDependency
    {
        /// <summary>
        /// 获取所有权限信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SysRoleMenu>> GetSysRoleMenuInfo();

        /// <summary>
        /// 根据角色Id获取权限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<SysRoleMenu>> GetSysRoleMenuByRole(int roleId);

        /// <summary>
        /// 添加角色菜单对应信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<ResponseResult> AddSysRoleMenuListAsync(List<SysRoleMenu> list);
    }
}
