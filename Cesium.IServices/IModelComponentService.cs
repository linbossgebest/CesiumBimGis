﻿using Cesium.Core.Extensions;
using Cesium.Models;
using Cesium.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices
{
    public interface IModelComponentService : IDependency
    {
        /// <summary>
        /// 批量新增模型构件
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<BaseResult> AddModelComponentListAsync(List<ModelComponent> list);

        /// <summary>
        /// 新增模型构件数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<BaseResult> InsertModelComponentAsync(ModelComponent model);

        

    }
}