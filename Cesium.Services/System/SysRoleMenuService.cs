using Cesium.Core.Extensions;
using Cesium.IRepository.System;
using Cesium.IServices.System;
using Cesium.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Services.System
{
    public class SysRoleMenuService : ISysRoleMenuService, IDependency
    {
        private readonly ISysRoleMenuRepository _sysRoleMenuRepository;

        public SysRoleMenuService(ISysRoleMenuRepository sysRoleMenuRepository)
        {
            _sysRoleMenuRepository = sysRoleMenuRepository;
        }

        /// <summary>
        /// 查询所有权限信息
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<SysRoleMenu>> GetSysRoleMenuInfo()
        {
           return _sysRoleMenuRepository.GetListAsync();
        }

        /// <summary>
        /// 通过角色id查询对应的权限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Task<IEnumerable<SysRoleMenu>> GetSysRoleMenuByRole(int roleId)
        {
            return _sysRoleMenuRepository.GetListAsync(new { RoleId = roleId });
        }
    }
}
