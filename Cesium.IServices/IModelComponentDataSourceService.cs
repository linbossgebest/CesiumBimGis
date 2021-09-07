using Cesium.Core.Extensions;
using Cesium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices
{
    public interface IModelComponentDataSourceService: IDependency
    {
        /// <summary>
        /// 根据构件类型编号获取模型构件菜单数据资源
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ModelComponentDataSource>> GetComponentDataSourceListAsync(int componentTypeId);

        /// <summary>
        /// 获取所有模型构件菜单数据资源
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ModelComponentDataSource>> GetAllComponentDataSourceListAsync();
    }
}
