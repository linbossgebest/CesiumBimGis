using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models.System
{
    public class SysMenu : BaseEntity
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父菜单Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 排序编号
        /// </summary>
        public int? OrderNo { get; set; }
    }
}
