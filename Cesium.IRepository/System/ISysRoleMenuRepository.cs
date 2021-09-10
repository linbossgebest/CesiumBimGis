using Cesium.Core.Extensions;
using Cesium.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IRepository.System
{
    public interface ISysRoleMenuRepository: IDependency, IBaseRepository<SysRoleMenu, int>
    {
        /// <summary>
        /// 批量插入角色菜单信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<int> AddListAsync(List<SysRoleMenu> list);
    }
}
