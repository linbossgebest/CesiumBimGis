using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.IServices;
using Cesium.Models;
using Cesium.ViewModels;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Services
{
    public class ModelComponentTypeService : IModelComponentTypeService, IDependency
    {
        private readonly IModelComponentTypeRepository _modelComponentTypeRepository;

        public ModelComponentTypeService(IModelComponentTypeRepository modelComponentTypeRepository)
        {
            _modelComponentTypeRepository = modelComponentTypeRepository;
        }

        public async Task<ResponseResult> AddOrModifyComponentTypeAsync(ComponentTypeModel model, TokenInfo tokenInfo)
        {
            var result = new ResponseResult();
            ModelComponentType typeInfo;
            if (model.Id == 0)
            {
                typeInfo = new ModelComponentType
                {
                    TypeName = model.TypeName,
                    CreateTime = DateTime.Now,
                    CreatorId = tokenInfo.UserId,
                    CreatorName = tokenInfo.UserName
                };
                if (await _modelComponentTypeRepository.InsertAsync(typeInfo) > 0)
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
            else
            {
                typeInfo = await _modelComponentTypeRepository.GetAsync(model.Id);
                if (typeInfo != null)
                {
                    typeInfo.TypeName = model.TypeName;
                    typeInfo.ModifyTime = DateTime.Now;
                    typeInfo.ModifyId = tokenInfo.UserId;
                    typeInfo.ModifyName = tokenInfo.UserName;
                    if (await _modelComponentTypeRepository.UpdateAsync(typeInfo) > 0)
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
                else
                {
                    result.isSuccess = false;
                    result.code = ResultCodeMsg.CommonFailCode;
                    result.message = ResultCodeMsg.CommonFailMsg;
                }

            }

            return result;
        }

        public async Task<ResponseResult> DeleteComponentType(int typeId)
        {
            var result = new ResponseResult();
            if (await _modelComponentTypeRepository.DeleteAsync(typeId) > 0)
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

        public async Task<IEnumerable<ModelComponentType>> GetComponentTypesAsync()
        {
            var types = await _modelComponentTypeRepository.GetListAsync();
            return types;
        }
    }
}
