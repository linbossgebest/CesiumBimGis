using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models
{
    /// <summary>
    /// 基类Entity
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人编号
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 修改人编号
        /// </summary>
        public int ModifyId { get; set; }

        /// <summary>
        /// 修改人名称
        /// </summary>
        public string ModifyName { get; set; }
    }
}
