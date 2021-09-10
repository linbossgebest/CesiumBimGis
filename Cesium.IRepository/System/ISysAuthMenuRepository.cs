using Cesium.Core.Extensions;
using Cesium.Models.System;
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
    }
}
