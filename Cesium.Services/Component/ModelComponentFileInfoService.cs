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
    public class ModelComponentFileInfoService : IModelComponentFileInfoService, IDependency
    {
        private readonly IModelComponentFileInfoRepository _modelComponentFileInfoRepository;

        public ModelComponentFileInfoService(IModelComponentFileInfoRepository modelComponentFileInfoRepository)
        {
            _modelComponentFileInfoRepository = modelComponentFileInfoRepository;
        }
        public async Task<BaseResult> AddModelComponentFileInfoAsync(ModelComponentFileInfo model)
        {
            var result = new BaseResult();
            if (await _modelComponentFileInfoRepository.InsertAsync(model) > 0)
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

        public async Task<BaseResult> DeleteComponentFileInfoAsync(int fileId)
        {
            var result = new BaseResult();
            if (await _modelComponentFileInfoRepository.DeleteAsync(fileId)>0)
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

        public async Task<IEnumerable<ModelComponentFileInfo>> GetComponentFilesAsync(string componentId)
        {
            string conditions = " where 1=1 ";
            if (!componentId.IsNullOrEmpty())
            {
                conditions += "And ComponentId = @ComponentId ";
            }
            var files = await _modelComponentFileInfoRepository.GetListAsync(conditions, new { ComponentId = componentId });
            return files;
        }

        public async Task<ModelComponentFileInfo> GetModelComponentFileByComponentId(int componentId, string menuName)
        {
            string conditions = " where ComponentId=@ComponentId And MenuName=@MenuName";
            var file = await _modelComponentFileInfoRepository.GetAsync(conditions, new { ComponentId = componentId , MenuName =menuName});

            return file;
            
        }

        public async Task<ModelComponentFileInfo> GetModelComponentFileById(int fileId)
        {
            return await _modelComponentFileInfoRepository.GetAsync(fileId);
        }
    }
}
