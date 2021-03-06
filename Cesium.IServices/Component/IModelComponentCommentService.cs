using Cesium.Core.Extensions;
using Cesium.Models;
using Cesium.ViewModels;
using Cesium.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices
{
    public interface IModelComponentCommentService: IDependency
    {
        /// <summary>
        /// 添加模型构件评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult> AddCommentAsync(CommentModel model);

        /// <summary>
        /// 删除模型构件评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseResult> DeleteCommentInfo(int id);

        /// <summary>
        /// 获取所有评论信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ModelComponentComment>> GetCommentsAsync(string componentId,string componentName);
    }
}
