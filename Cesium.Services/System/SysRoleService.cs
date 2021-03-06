using Cesium.Core.Extensions;
using Cesium.IRepository.System;
using Cesium.IServices.System;
using Cesium.Models.System;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
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
        private readonly ISysRoleMenuRepository _sysRoleMenuRepository;

        public SysRoleService(ISysRoleRepository sysRoleRepository, ISysRoleMenuRepository sysRoleMenuRepository)
        {
            _sysRoleRepository = sysRoleRepository;
            _sysRoleMenuRepository = sysRoleMenuRepository;
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

        /// <summary>
        /// 获取所有角色信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SysRole>> GetRolesAsync()
        {
            var roles = await _sysRoleRepository.GetListAsync();

            return roles;
        }

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<ResponseResult> DeleteRoleInfo(int roleId)
        {
            var result = new ResponseResult();
            
            if (await _sysRoleRepository.DeleteAsync(roleId) > 0)
            {
                await _sysRoleMenuRepository.DeleteListAsync(new { RoleId = roleId });
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

        public async Task<ResponseResult> AddOrModifyRoleAsync(RoleModel model, TokenInfo tokenInfo)
        {
            var result = new ResponseResult();
            if (await _sysRoleRepository.AddOrUpdate(model, tokenInfo))
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

  
    }
}
