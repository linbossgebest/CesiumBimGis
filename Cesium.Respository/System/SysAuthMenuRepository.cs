using Cesium.Core;
using Cesium.Core.Extensions;
using Cesium.IRepository.System;
using Cesium.Models.System;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Cesium.Respository.System
{
    public class SysAuthMenuRepository: BaseRepository<SysAuthMenu, int>, IDependency, ISysAuthMenuRepository
    {
        public SysAuthMenuRepository(IOptionsSnapshot<DbOption> options) : base(options.Get("DbOption"))
        {
        }

        public async Task<IEnumerable<SysAuthMenu>> GetSysAuthMenuListAsync()
        {
            string sql = "SELECT * FROM SysAuthMenu sam INNER JOIN SysAuthMenuMeta samm ON sam.Id = samm.MenuId ";
            HashSet<SysAuthMenu> list = new();
            SysAuthMenu item = null;
            var result = await _dbConnection.QueryAsync<SysAuthMenu, SysAuthMenuMeta, SysAuthMenu>(sql,(sysAuthMenu,sysAuthMenuMeta)=> {
                if (item == null || item.Id != sysAuthMenu.Id)
                    item = sysAuthMenu;

                if (sysAuthMenuMeta != null)
                    item.Meta = sysAuthMenuMeta;

                if (!list.Any(m => m.Id == item.Id))
                    list.Add(item);

                return null;
            });

            return list;
        }

    }
}
