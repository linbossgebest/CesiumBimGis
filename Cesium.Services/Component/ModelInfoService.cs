using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.IServices;
using Cesium.Models;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Services
{
    public class ModelInfoService : IModelInfoService, IDependency
    {
        private readonly IModelInfoRepository _modelInfoRepository;

        public ModelInfoService(IModelInfoRepository modelInfoRepository)
        {
            _modelInfoRepository = modelInfoRepository;
        }

        /// <summary>
        /// 新增模型
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        public async Task<BaseResult> AddModelInfoAsync(ModelInfo model, TokenInfo tokenInfo)
        {
            var result = new BaseResult();
            model.CreateTime = DateTime.Now;
            model.CreatorId = tokenInfo.UserId;
            model.CreatorName = tokenInfo.UserName;

            if (await _modelInfoRepository.InsertAsync(model) > 0)
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
