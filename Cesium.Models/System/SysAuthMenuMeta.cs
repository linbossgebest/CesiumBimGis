using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Models.System
{
    public class SysAuthMenuMeta
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [JsonProperty("menuid")]
        public int MenuId { get; set; }

        /// <summary>
        /// 所需要的对应的权限
        /// if not set means it doesn't need any permission.
        /// </summary>
        [JsonProperty("roles")]
        public string Roles { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 是否缓存
        /// if set true, the page will no be cached(default is false)
        /// </summary>
        [JsonProperty("noCache")]
        public bool NoCache { get; set; }

        /// <summary>
        ///  if set true, the tag will affix in the tags-view
        /// </summary>
        [JsonProperty("affix")]
        public bool Affix { get; set; }

        /// <summary>
        /// 是否展示面包屑
        /// if set false, the item will hidden in breadcrumb(default is true)
        /// </summary>
        [JsonProperty("breadcrumb")]
        public bool Breadcrumb { get; set; }

        /// <summary>
        /// 需要高亮显示的路径
        /// if set path, the sidebar will highlight the path you set
        /// </summary>
        [JsonProperty("activeMenu")]
        public string ActiveMenu { get; set; }
    }
}
