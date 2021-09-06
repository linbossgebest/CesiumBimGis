using Cesium.Core.Extensions;
using Cesium.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices.System
{
    public interface ISysAppMenuService: IDependency
    {
        /// <summary>
        /// 通过type查询app菜单
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IEnumerable<SysAppMenu>> GetSysAppMenu(int type);

        /// <summary>
        /// 查询所有App菜单
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SysAppMenu>> GetAllSysAppMenu();
    }
}
