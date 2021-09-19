using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models.System
{
    public class SystemLog
    {
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 日志信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 日志记录时间
        /// </summary>
        public DateTime LoggedTime { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int UserId { get; set; }


    }
}
