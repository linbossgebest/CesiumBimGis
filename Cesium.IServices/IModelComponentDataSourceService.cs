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

        /// <summary>
        /// 新增或修改模型构件菜单数据资源
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        Task<BaseResult> AddOrModifyComponentDataSourceAsync(ComponentMenuModel model, TokenInfo tokenInfo);

        /// <summary>
        /// 通过编号删除模型构件菜单数据资源信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult> DeleteComponentDataSource(int id);
    }
}
