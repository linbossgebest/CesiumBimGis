using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.ViewModels
{
    /// <summary>
    /// 构件菜单模型
    /// </summary>
    public class ComponentMenuModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 构件类型编号
        /// </summary>
        public int ComponentTypeId { get; set; }

        /// <summary>
        /// 对应的app显示菜单Id
        /// </summary>
        public int AppMenuId { get; set; }

        /// <summary>
        /// 自定义菜单名称
        /// </summary>
        public string CustomMenuName { get; set; }

        /// <summary>
        /// 自定义菜单资源
        /// </summary>
        public string CustomMenuSrc { get; set; }
    }
}
