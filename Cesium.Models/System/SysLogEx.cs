using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models.System
{
    /// <summary>
    /// 系统异常日志
    /// </summary>
    public class SysLogEx
    {
        public int Id { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 异常名称
        /// </summary>
        public string ExceptionName { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMsg { get; set; }

        /// <summary>
        /// 异常源
        /// </summary>
        public string ExceptionSource { get; set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// 参数对象
        /// </summary>
        public string ParamsObj { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        public DateTime? ExceptionTime { get; set; }
    }
}
