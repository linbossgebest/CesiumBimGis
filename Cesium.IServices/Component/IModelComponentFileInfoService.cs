using Cesium.Core.Extensions;
using Cesium.Models;
using Cesium.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices
{
    public interface IModelComponentFileInfoService:IDependency
    {

        /// <summary>
        /// 获取所有构件文件信息(模糊查询)
        /// </summary>
        /// <param name="componentId">构件编号</param>
        /// <returns></returns>
        Task<IEnumerable<ModelComponentFileInfo>> GetComponentFilesAsync(string componentId);

        /// <summary>
        /// 添加构件文件信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResult> AddModelComponentFileInfoAsync(ModelComponentFileInfo model);

        /// <summary>
        /// 删除构件文件信息
        /// </summary>
        /// <param name="fileId">文件Id</param>
        /// <returns></returns>
        Task<BaseResult> DeleteComponentFileInfoAsync(int fileId);

        /// <summary>
        /// 通过文件编号获取文件信息
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task<ModelComponentFileInfo> GetModelComponentFileById(int fileId);

        /// <summary>
        /// 通过构件编号和菜单名称  获取文件信息
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="menuName"></param>
        /// <returns></returns>
        Task<ModelComponentFileInfo> GetModelComponentFileByComponentId(int componentId,string menuName);
    }
}
