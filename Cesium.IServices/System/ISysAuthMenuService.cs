using Cesium.Core.Extensions;
using Cesium.Models.System;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices.System
{
    public interface ISysAuthMenuService: IDependency
    {
        /// <summary>
        /// 获取所有菜单信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SysAuthMenu>> GetMenuInfo();

        /// <summary>
        /// 通过menuIds获取对应的菜单信息
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        Task<IEnumerable<SysAuthMenu>> GetMenuInfo(List<int> menuIds);

        /// <summary>
        /// 生成菜单树
        /// </summary>
        /// <returns></returns>
        Task<List<MenuTree>> GetMenuTree(List<SysAuthMenu> menus);

        /// <summary>
        /// 添加或修改菜单信息
        /// </summary>
        /// <returns></returns>
        Task<BaseResult> AddOrModifyMenuAsync(SysAuthMenu model, TokenInfo tokenInfo);

        /// <summary>
        /// 通过菜单编号删除菜单以及关联的子菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<BaseResult> DelelteMenuInfo(int menuId);
    }
}
