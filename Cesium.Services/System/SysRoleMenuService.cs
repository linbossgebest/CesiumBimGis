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
        public async Task<IEnumerable<SysRoleMenu>> GetSysRoleMenuInfo()
        {
           return await _sysRoleMenuRepository.GetListAsync();
        }

        /// <summary>
        /// 通过角色id查询对应的权限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysRoleMenu>> GetSysRoleMenuByRole(int roleId)
        {
            return await _sysRoleMenuRepository.GetListAsync(new { RoleId = roleId });
        }


        public async Task<BaseResult> AddSysRoleMenuListAsync(List<SysRoleMenu> list)
        {
            var result = new BaseResult();

            var roleId = list.FirstOrDefault().RoleId;
            //先删除信息 再添加信息
            await _sysRoleMenuRepository.DeleteListAsync(new { RoleId= roleId });

            await _sysRoleMenuRepository.AddListAsync(list);

            result.isSuccess = true;
            result.code = ResultCodeMsg.CommonSuccessCode;
            result.message = ResultCodeMsg.CommonSuccessMsg;

            return result;
        }
    }
}
