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
    public class SysPermissionService : ISysPermissionService, IDependency
    {
        private readonly ISysPermissionRepository _sysPermissionRepository;

        public SysPermissionService(ISysPermissionRepository sysPermissionRepository)
        {
            _sysPermissionRepository = sysPermissionRepository;
        }

        /// <summary>
        /// 查询所有权限信息
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<SysPermission>> GetPermissionInfo()
        {
           return _sysPermissionRepository.GetListAsync();
        }

        /// <summary>
        /// 通过角色id查询对应的权限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Task<IEnumerable<SysPermission>> GetPermissionInfoByRole(int roleId)
        {
            return _sysPermissionRepository.GetListAsync(new { RoleId = roleId });
        }
    }
}
