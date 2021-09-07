using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.IServices;
using Cesium.Models;
using Cesium.ViewModels.ResultModel;
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
