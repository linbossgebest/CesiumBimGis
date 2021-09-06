using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.IServices;
using Cesium.Models;
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
        public async Task<IEnumerable<ModelComponentDataSource>> GetComponentDataSourceListAsync(string componentId)
        {
            return await _modelComponentDataSourceRepository.GetComponentDataSourceListAsync(componentId);
        }
    }
}
