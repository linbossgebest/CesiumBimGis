using Cesium.Models.System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models.System
{
    public class MenuTree
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 父菜单Id
        /// </summary>
        [JsonProperty("parentid")]
        public int ParentId { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 对应组件
        /// </summary>
        [JsonProperty("component")]
        public string Component { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        /// <summary>
        /// 根菜单是否显示
        /// </summary>
        [JsonProperty("alwaysShow")]
        public bool AlwaysShow { get; set; }

        /// <summary>
        /// 重定向
        /// </summary>
        [JsonProperty("redirect")] 
        public string Redirect { get; set; }

        /// <summary>
        /// 动态菜单元数据
        /// </summary>
        [JsonProperty("meta")]
        public SysAuthMenuMeta Meta { get; set; }

        /// <summary>
        /// 排序编号
        /// </summary>
        [JsonProperty("order")]
        public int? OrderNo { get; set; }

        [JsonProperty("children")]
        public List<MenuTree> Children { get; set; }
    }
}
