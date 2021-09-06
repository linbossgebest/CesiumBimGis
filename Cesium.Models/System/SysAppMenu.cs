using Cesium.Core.CustomEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models.System
{
    public class SysAppMenu: BaseEntity
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public AppMenuType MenuType { get; set; }

        ///// <summary>
        ///// 地址
        ///// </summary>
        //public string Url { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 对应操作
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 排序编号
        /// </summary>
        public int? OrderNo { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public int IsEnabled { get; set; }
    }
}
