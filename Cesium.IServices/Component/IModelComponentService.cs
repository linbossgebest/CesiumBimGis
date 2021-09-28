using Cesium.Core.Extensions;
using Cesium.Models;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices
{
    public interface IModelComponentService : IDependency
    {
        /// <summary>
        /// 批量新增模型构件
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<BaseResult> AddModelComponentListAsync(List<Cesium.Models.ModelComponent> list);

        /// <summary>
        /// 新增模型构件数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<BaseResult> AddModelComponentAsync(Cesium.Models.ModelComponent model);

        /// <summary>
        /// 修改模型构件数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResult> UpdateModelComponentAsync(Cesium.Models.ModelComponent model);

        /// <summary>
        /// 根据构件编号删除模型构件数据
        /// </summary>
        /// <param name="componentId"></param>
        /// <returns></returns>
        Task<BaseResult> DeleteModelComponent(string componentId);

        /// <summary>
        /// 通过构件编号获取构件信息
        /// </summary>
        /// <param name="componentId"></param>
        /// <returns></returns>
        Task<Cesium.Models.ModelComponent> GetComponentInfoAsync(string componentId);

        /// <summary>
        /// 获取所有构件信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Cesium.Models.ModelComponent>> GetComponentsAsync(string componentId, string componentName);

        /// <summary>
        /// 通过构件编号获取构件的额外属性
        /// </summary>
        /// <param name="componentId"></param>
        /// <returns></returns>
        Task<string> GetAddtionalProperties(string componentId);
    }
}
