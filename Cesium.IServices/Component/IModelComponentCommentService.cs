using Cesium.Core.Extensions;
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
        Task<BaseResult> AddCommentAsync(CommentModel model);
    }
}
