using Cesium.Core;
using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.Models;
using Cesium.Models.System;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Respository
{
    public class ModelComponentDataSourceRepository : BaseRepository<ModelComponentDataSource, int>, IDependency, IModelComponentDataSourceRepository
    {
        public ModelComponentDataSourceRepository(IOptionsSnapshot<DbOption> options) : base(options.Get("DbOption"))
        {
        }

        public async Task<IEnumerable<ModelComponentDataSource>> GetComponentDataSourceListAsync(string componentId)
        {
            string sql = "SELECT * FROM ModelComponentDataSource mcds INNER JOIN SysAppMenu sam ON mcds.AppMenuId = sam.Id Where ComponentId=@componentId ORDER BY OrderNo";
            HashSet<ModelComponentDataSource> list = new();
            ModelComponentDataSource item = null;
            var result = await _dbConnection.QueryAsync<ModelComponentDataSource, SysAppMenu, ModelComponentDataSource>(sql, (componentDataSource, sysAppMenu) =>
            {
                if (item == null || item.Id != componentDataSource.Id)
                    item = componentDataSource;

                if (sysAppMenu != null)
                    item.AppMenu = sysAppMenu;

                if (!list.Any(m => m.Id == item.Id))
                    list.Add(item);

                return null;
            }, new { ComponentId = componentId });

            return list;
        }


        public async Task<IEnumerable<ModelComponentDataSource>> GetAllComponentDataSourceListAsync()
        {
            string sql = "SELECT * FROM ModelComponentDataSource mcds INNER JOIN SysAppMenu sam ON mcds.AppMenuId = sam.Id ORDER BY OrderNo";
            HashSet<ModelComponentDataSource> list = new();
            ModelComponentDataSource item = null;
            var result = await _dbConnection.QueryAsync<ModelComponentDataSource, SysAppMenu, ModelComponentDataSource>(sql, (componentDataSource, sysAppMenu) =>
            {
                if (item == null || item.Id != componentDataSource.Id)
                    item = componentDataSource;

                if (sysAppMenu != null)
                    item.AppMenu = sysAppMenu;

                if (!list.Any(m => m.Id == item.Id))
                    list.Add(item);

                return null;
            });

            return list;
        }
    }
}
