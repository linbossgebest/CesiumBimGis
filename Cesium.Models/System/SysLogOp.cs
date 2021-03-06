using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models.System
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public class SysLogOp
    {
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否执行成功（1-是，0-否）
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 具体消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string Os { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 请求方式（GET POST PUT DELETE)
        /// </summary>
        public string ReqMethod { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        public long ElapsedTime { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OpTime { get; set; }
        
        /// <summary>
        /// 操作人
        /// </summary>
        public string Account { get; set; }


    }
}
