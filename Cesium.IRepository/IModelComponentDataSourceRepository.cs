using Cesium.Core.Extensions;
using Cesium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IRepository
{
    public interface IModelComponentDataSourceRepository : IDependency, IBaseRepository<ModelComponentDataSource, int>
    {
        /// <summary>
        /// 根据构件Id获取构件动态信息
        /// </summary>
        /// <param name="componentId">构件编号</param>
        /// <returns></returns>
        Task<IEnumerable<ModelComponentDataSource>> GetComponentDataSourceListAsync(string componentId);

        /// <summary>
        /// 获取所有构件动态信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ModelComponentDataSource>> GetAllComponentDataSourceListAsync();
    }
}
