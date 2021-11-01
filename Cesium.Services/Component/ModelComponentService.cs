using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.IServices;
using Cesium.Models;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
using Dapper;
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
            if (await _modelComponentRepository.AddListAsync(list))
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
        /// 通过构件编号获取构件信息
        /// </summary>
        /// <param name="componentId"></param>
        /// <returns></returns>
        public async Task<ModelComponent> GetComponentInfoAsync(string componentId)
        {
            var component = await _modelComponentRepository.GetAsync(componentId);

            return component;
        }

        /// <summary>
        /// 查询所有构件信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ModelComponent>> GetComponentsAsync(string componentId, string componentName)
        {
            string conditions = " where 1=1 ";
            //var args = new DynamicParameters(new { });
            if (!componentId.IsNullOrWhiteSpace())
            {
                conditions += "And ComponentId = @ComponentId";
                //args.Add("ComponentId", componentId);
            }
            if (!componentName.IsNullOrWhiteSpace())
            {
                conditions += "And ComponentName = @ComponentName";
                //args.Add("ComponentName", componentName);
            }

            var components = await _modelComponentRepository.GetListAsync(conditions, new { ComponentId = componentId, ComponentName = componentName });

            return components;
        }

        /// <summary>
        /// 新增模型构件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResult> AddModelComponentAsync(ModelComponent model)
        {
            var result = new BaseResult();
            var existComponent =await _modelComponentRepository.GetAsync(model.ComponentId);

            if (existComponent != null)
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.DuplicateNumberCode;
                result.message = ResultCodeMsg.DuplicateNumberErrorMsg;
            }
            else
            {
                if (!(await _modelComponentRepository.InsertByKeyAsync(model)).IsNullOrWhiteSpace())
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

        /// <summary>
        /// 修改模型构件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResult> UpdateModelComponentAsync(ModelComponent model)
        {
            var result = new BaseResult();
            if (await _modelComponentRepository.UpdateAsync(model) > 0)
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
        /// 删除构件数据
        /// </summary>
        /// <param name="componentId"></param>
        /// <returns></returns>
        public async Task<BaseResult> DeleteModelComponent(string componentId)
        {
            var result = new BaseResult();
            if (await _modelComponentRepository.DeleteAsync(componentId) > 0)
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
        /// 获取构件额外属性（json对象）
        /// </summary>
        /// <param name="componentId"></param>
        /// <returns></returns>
        public async Task<string> GetAddtionalProperties(string componentId)
        {
            var existComponent = await _modelComponentRepository.GetAsync(componentId);

            return existComponent.AdditionalProperties;
        }

        /// <summary>
        /// 获取所有已完成构件
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ModelComponent>> GetCompletedComponentsAsync()
        {
            string conditions = " where `Status`=1 ";
            //var args = new DynamicParameters(new { });

            var components = await _modelComponentRepository.GetListAsync(conditions);

            return components;
        }
    }
}
