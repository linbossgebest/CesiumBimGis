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


        public Task<ResponseResult> AddUserRoleInfo(int userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除该用户编号关联的所有角色信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult> DeleteUserRoleInfo(int userId)
        {
            var result = new ResponseResult();
            if (await _sysUserRoleRepository.DeleteListAsync(new { UserId = userId }) > 0)
            {
                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

        /// <summary>
        /// 根据用户编号获取用户角色信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysUserRole>> GetUserRoleListAsync(int userId)
        {
            return await _sysUserRoleRepository.GetListAsync(new { UserId = userId });
        }

        /// <summary>
        /// 获取所有用户角色信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SysUserRole>> GetUserRoleListAsync()
        {
            return await _sysUserRoleRepository.GetListAsync();
        }
    }
}
