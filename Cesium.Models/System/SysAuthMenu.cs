using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models.System
{
    /// <summary>
    /// 动态菜单
    /// </summary>
    public class SysAuthMenu:BaseEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 父菜单Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对应组件
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// 根菜单是否显示
        /// </summary>
        public bool AlwaysShow { get; set; }

        /// <summary>
        /// 重定向
        /// </summary>
        public string Redirect { get; set; }

        /// <summary>
        /// 动态菜单元数据
        /// </summary>
        public SysAuthMenuMeta Meta { get; set; }

        /// <summary>
        /// 排序编号
        /// </summary>
        public int? OrderNo { get; set; }

    }
}
