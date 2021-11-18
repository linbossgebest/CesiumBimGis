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
    public class ModelComponentDataSourceService : IModelComponentDataSourceService, IDependency
    {
        private readonly IModelComponentDataSourceRepository _modelComponentDataSourceRepository;

        public ModelComponentDataSourceService(IModelComponentDataSourceRepository modelComponentDataSourceRepository)
        {
            _modelComponentDataSourceRepository = modelComponentDataSourceRepository;
        }

        public async Task<ResponseResult> AddOrModifyComponentDataSourceAsync(ComponentMenuModel model, TokenInfo tokenInfo)
        {
            var result = new ResponseResult();
            ModelComponentDataSource source;
            if (model.Id == 0)//新增
            {
                source = new ModelComponentDataSource
                {
                    ComponentTypeId = model.ComponentTypeId,
                    AppMenuId = model.AppMenuId,
                    CustomMenuName = model.CustomMenuName,
                    CustomMenuSrc = model.CustomMenuSrc,
                };
                if (await _modelComponentDataSourceRepository.InsertAsync(source) > 0)
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
            else//修改
            {
                source = await _modelComponentDataSourceRepository.GetAsync(model.Id);
                if (source != null)
                {
                    source.ComponentTypeId = model.ComponentTypeId;
                    source.AppMenuId = model.AppMenuId;
                    source.CustomMenuName = model.CustomMenuName;
                    source.CustomMenuSrc = model.CustomMenuSrc;
                    if (await _modelComponentDataSourceRepository.UpdateAsync(source) > 0)
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

        public async Task<ResponseResult> DeleteComponentDataSource(int id)
        {
            var result = new ResponseResult();
            if (await _modelComponentDataSourceRepository.DeleteAsync(id) > 0)
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
        /// 获取所有构件菜单数据资源
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ModelComponentDataSource>> GetAllComponentDataSourceListAsync()
        {
            return await _modelComponentDataSourceRepository.GetAllComponentDataSourceListAsync();
        }

        /// <summary>
        /// 获取模型构件菜单数据资源
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ModelComponentDataSource>> GetComponentDataSourceListAsync(int componentTypeId)
        {
            return await _modelComponentDataSourceRepository.GetComponentDataSourceListAsync(componentTypeId);
        }

        public async Task<IEnumerable<ModelComponentDataSource>> GetComponentDataSourceListByComponentIdAsync(string componentId)
        {
            return await _modelComponentDataSourceRepository.GetModelComponentDataSources(componentId);
        }
    }
}
