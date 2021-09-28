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

        public async Task<bool> AddListAsync(List<ModelComponent> list)
        {
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    List<string> componentIds = new List<string>();
                    componentIds = list.Select(f => f.ComponentId).ToList();
                    //foreach (var item in list)
                    //{
                    //    componentIds.Add(item.ComponentId);
                    //}
                    string deletesql = "Delete from ModelComponent where ComponentId in @componentIds";
                    await _dbConnection.ExecuteAsync(deletesql, new { componentIds }, transaction);

                    string sql = @" INSERT INTO ModelComponent (ComponentId,ComponentName,ModelId,Status,CompletedTime,ParentId,ComponentTypeId,AdditionalProperties)
             VALUES(@ComponentId,@ComponentName,@ModelId,@Status,@CompletedTime,@ParentId,@ComponentTypeId,@AdditionalProperties); ";
                    await _dbConnection.ExecuteAsync(sql, list, transaction);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
               
            }
        }
    }
}
