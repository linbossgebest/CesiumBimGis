using Cesium.Core.Extensions;
using Cesium.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices.System
{
    public interface ISysLogOpService : IDependency
    {
        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddLog(SysLogOp model);


        /// <summary>
        /// 根据日志名称 请求方式 模糊查询操作日志
        /// </summary>
        /// <param name="logName"></param>
        /// <param name="requestMethod"></param>
        /// <returns></returns>
        Task<IEnumerable<SysLogOp>> GetLogs(string logName,string requestMethod);
    }
}
