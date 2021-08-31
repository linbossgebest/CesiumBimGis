using Cesium.Core.Extensions;
using Cesium.Models.System;
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
        /// 生成菜单树
        /// </summary>
        /// <returns></returns>
        Task<List<MenuTree>> GetMenuTree(List<SysAuthMenu> menus);
    }
}
