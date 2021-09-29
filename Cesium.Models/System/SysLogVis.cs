using Cesium.Core.CustomEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models.System
{
    public class SysLogVis
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
        /// 访问类型
        /// </summary>
        public LoginType VisType { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime? VisTime { get; set; }

        /// <summary>
        /// 访问人
        /// </summary>
        public string Account { get; set; }
    }
}
