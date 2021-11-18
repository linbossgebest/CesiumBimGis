using Cesium.Core.Extensions;
using Cesium.Models;
using Cesium.ViewModels;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices
{
    public interface IModelComponentTypeService: IDependency
    {
        /// <summary>
        /// 查询所有构件类型
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ModelComponentType>> GetComponentTypesAsync();

        /// <summary>
        /// 新增或修改菜单类型
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        Task<ResponseResult> AddOrModifyComponentTypeAsync(ComponentTypeModel model, TokenInfo tokenInfo);

        /// <summary>
        /// 通过类型编号删除构件类型信息
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        Task<ResponseResult> DeleteComponentType(int typeId);
    }
}
