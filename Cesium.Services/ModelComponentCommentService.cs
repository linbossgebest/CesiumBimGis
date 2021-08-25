using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.IServices;
using Cesium.Models;
using Cesium.ViewModels;
using Cesium.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Services
{
    public class ModelComponentCommentService : IModelComponentCommentService, IDependency
    {
        private readonly IModelComponentCommentRepository _modelComponentCommentRepository;
        public ModelComponentCommentService(IModelComponentCommentRepository modelComponentCommentRepository)
        {
            _modelComponentCommentRepository = modelComponentCommentRepository;
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResult> AddCommentAsync(CommentModel model)
        {
            var result = new BaseResult();
            if (model.Id == 0) 
            {
                ModelComponentComment comment = new ModelComponentComment();
                comment.ModelId = model.ModelId;
                comment.ModelName = model.ModelName;
                comment.ComponentId = model.ComponentId;
                comment.ComponentName = model.ComponentName;
                comment.Comment = model.Comment;
                comment.CreateTime = DateTime.Now;
                comment.CreatorId = 1;
                comment.CreatorName = "admin";

                if (await _modelComponentCommentRepository.InsertAsync(comment) > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "操作成功";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "操作失败";
                }     
            }
            return result;
        }
    }
}
