﻿using Cesium.Core.Extensions;
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
    public class SysRoleService : ISysRoleService, IDependency
    {
        private readonly ISysRoleRepository _sysRoleRepository;

        public SysRoleService(ISysRoleRepository sysRoleRepository)
        {
            _sysRoleRepository = sysRoleRepository;
        }

        /// <summary>
        /// 通过角色编号获取角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>

        public async Task<SysRole> GetRoleInfoByUserId(int roleId)
        {
            var role = await _sysRoleRepository.GetAsync(roleId);

            return role;
        }
    }
}
