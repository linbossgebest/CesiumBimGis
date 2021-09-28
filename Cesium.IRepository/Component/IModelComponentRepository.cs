using Cesium.Core.Extensions;
using Cesium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IRepository
{
    public interface IModelComponentRepository : IDependency, IBaseRepository<ModelComponent, string>
    {
        /// <summary>
        /// 批量更新模型构件信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> AddListAsync(List<ModelComponent> list);
    }
}
