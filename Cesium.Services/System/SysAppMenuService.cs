using Cesium.Core.Extensions;
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
    public class SysAppMenuService : ISysAppMenuService, IDependency
    {
        private readonly ISysAppMenuRepository _sysAppMenuRepository;

        public SysAppMenuService(ISysAppMenuRepository sysAppMenuRepository)
        {
            _sysAppMenuRepository = sysAppMenuRepository;
        }

        /// <summary>
        /// 查询所有App菜单
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SysAppMenu>> GetAllSysAppMenu()
        {
            return await _sysAppMenuRepository.GetListAsync();
        }

        /// <summary>
        /// 通过type查询app菜单
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysAppMenu>> GetSysAppMenu(int type)
        {
            return await _sysAppMenuRepository.GetListAsync(new { MenuType = type });
        }
    }
}
