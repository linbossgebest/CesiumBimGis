using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models.System
{
    [Obsolete("不再使用的权限表")]
    public class SysPermission:BaseEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

    }
}
