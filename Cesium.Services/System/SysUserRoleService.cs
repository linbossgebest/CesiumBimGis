using Cesium.Core.Extensions;
using Cesium.IRepository.System;
using Cesium.IServices.System;
using Cesium.Models.System;
using Cesium.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Services.System
{
    public class SysUserRoleService : ISysUserRoleService, IDependency
    {
        private readonly ISysUserRoleRepository _sysUserRoleRepository;

        public SysUserRoleService(ISysUserRoleRepository sysUserRoleRepository)
        {
            _sysUserRoleRepository = sysUserRoleRepository;
        }


        public Task<BaseResult> AddUserRoleInfo(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult> DeleteUserRoleInfo(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SysUserRole>> GetUserRoleListAsync(int userId)
        {
            return await _sysUserRoleRepository.GetListAsync(new { UserId = userId });
        }
    }
}
