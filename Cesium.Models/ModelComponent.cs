using Cesium.Core.CustomEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models
{
    /// <summary>
    /// 模型构件信息
    /// </summary>
    public class ModelComponent
    {
        /// <summary>
        /// 构件id
        /// </summary>
        public string ComponentId { get; set; }

        /// <summary>
        /// 构件名称
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// 构件所属的模型id
        /// </summary>
        public int ModelId { get; set; }

        /// <summary>
        /// 施工状态
        /// </summary>
        public ComponentStatus Status { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompletedTime { get; set; }

        /// <summary>
        /// 父节点Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 构件类型
        /// </summary>
        public ComponentType Type { get; set; }

    }
}
