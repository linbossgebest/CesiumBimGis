using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models.System
{
    public class SysRole : BaseEntity
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public string RoleType { get; set; }

        /// <summary>
        /// 排序编号
        /// </summary>
        public int? OrderNo { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 角色对应菜单
        /// </summary>
        public List<MenuTree> Menus { get; set; }
    }
}
