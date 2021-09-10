using Cesium.Core.Extensions;
using Cesium.Models.System;
using Cesium.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IRepository.System
{
    public interface ISysRoleRepository : IDependency, IBaseRepository<SysRole, int>
    {
        /// <summary>
        /// 事务执行 添加角色信息以及添加角色菜单信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdate(RoleModel model, TokenInfo tokenInfo);
    }
}
