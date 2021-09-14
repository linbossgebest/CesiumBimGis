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
    public interface ISysAuthMenuRepository : IDependency, IBaseRepository<SysAuthMenu, int>
    {
        /// <summary>
        /// 自定义获取菜单信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SysAuthMenu>> GetSysAuthMenuListAsync();

        /// <summary>
        /// 根据条件自定义获取菜单信息
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        Task<IEnumerable<SysAuthMenu>> GetSysAuthMenuListAsync(List<int> menuIds);

        /// <summary>
        /// 添加或修改菜单信息（SysAuthMenu表，SysAuthMenuMeta表）
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdate(SysAuthMenu model, TokenInfo tokenInfo);

        /// <summary>
        /// 删除菜单信息以及关联的子菜单信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<bool> DeleteMenuInfo(int menuId);
    }
}
