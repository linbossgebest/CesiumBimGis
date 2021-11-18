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
        public async Task<ResponseResult> AddCommentAsync(CommentModel model)
        {
            var result = new ResponseResult();
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
                    result.isSuccess = true;
                    result.code = ResultCodeMsg.CommonSuccessCode;
                    result.message = ResultCodeMsg.CommonSuccessMsg;
                }
                else
                {
                    result.isSuccess = false;
                    result.code = ResultCodeMsg.CommonFailCode;
                    result.message = ResultCodeMsg.CommonFailMsg;
                }
            }
            return result;
        }

        public async Task<ResponseResult> DeleteCommentInfo(int id)
        {
            var result = new ResponseResult();
            if (await _modelComponentCommentRepository.DeleteAsync(id) > 0)
            {
                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

        public async Task<IEnumerable<ModelComponentComment>> GetCommentsAsync(string componentId, string componentName)
        {
            string conditions = " where 1=1 ";
            if (!componentId.IsNullOrWhiteSpace())
            {
                conditions += "And ComponentId = @ComponentId";
            }
            if (!componentName.IsNullOrWhiteSpace())
            {
                conditions += "And ComponentName = @ComponentName";
            }
            var comments = await _modelComponentCommentRepository.GetListAsync(conditions, new { ComponentId = componentId, ComponentName = componentName });

            return comments;
        }
    }
}
