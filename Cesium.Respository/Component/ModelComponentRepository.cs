using Cesium.Core;
using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Respository
{
    public class ModelComponentRepository: BaseRepository<ModelComponent, string>, IDependency, IModelComponentRepository
    {
        public ModelComponentRepository(IOptionsSnapshot<DbOption> options) : base(options.Get("DbOption"))
        {
        }

        public async Task<int> AddListAsync(List<ModelComponent> list)
        {
            string sql = @" INSERT INTO ModelComponent (ComponentId,ComponentName,ModelId,Status,CompletedTime,ParentId,ComponentTypeId)
             VALUES(@ComponentId,@ComponentName,@ModelId,@Status,@CompletedTime,@ParentId,@ComponentTypeId); ";
            return await _dbConnection.ExecuteAsync(sql, list);
        }
    }
}
