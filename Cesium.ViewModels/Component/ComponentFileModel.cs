using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.ViewModels
{
   public class ComponentFileModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 模型Id
        /// </summary>
        public int ModelId { get; set; }

        /// <summary>
        /// 构件Id
        /// </summary>
        public string ComponentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
    }
}
