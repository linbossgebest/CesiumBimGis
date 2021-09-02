using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.IServices;
using Cesium.Models;
using Cesium.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Services
{
    public class ModelComponentService : IModelComponentService, IDependency
    {
        private readonly IModelComponentRepository _modelComponentRepository;

        public ModelComponentService(IModelComponentRepository modelComponentRepository)
        {
            _modelComponentRepository = modelComponentRepository;
        }

        /// <summary>
        /// 批量插入模型构件
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<BaseResult> AddModelComponentListAsync(List<ModelComponent> list)
        {
            var result = new BaseResult();
            if (await _modelComponentRepository.AddListAsync(list) > 0)
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

        /// <summary>
        /// 单个模型构件添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResult> InsertModelComponentAsync(ModelComponent model)
        {
            var result = new BaseResult();
            if (await _modelComponentRepository.InsertAsync(model) > 0)
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
    }
}
