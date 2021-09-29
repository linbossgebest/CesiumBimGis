using Cesium.Core.Extensions;
using Cesium.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices.System
{
    public interface ISysLogExService: IDependency
    {
        /// <summary>
        /// 添加异常日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddLog(SysLogEx model);


        /// <summary>
        /// 根据类名 方法名 模糊查询异常日志
        /// </summary>
        /// <param name="className">类名 </param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        Task<IEnumerable<SysLogEx>> GetLogs(string className, string methodName);
    }
}
