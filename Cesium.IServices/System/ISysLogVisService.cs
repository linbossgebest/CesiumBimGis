using Cesium.Core.CustomEnum;
using Cesium.Core.Extensions;
using Cesium.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices.System
{
    public interface ISysLogVisService: IDependency
    {
        /// <summary>
        /// 添加访问日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddLog(SysLogVis model);


        /// <summary>
        /// 根据日志名称 登录类型 查询操作日志
        /// </summary>
        /// <param name="logName">日志名称</param>
        /// <param name="visType">登录类型</param>
        /// <returns></returns>
        Task<IEnumerable<SysLogVis>> GetLogs(string logName, int? visType);
    }
}
