using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.ViewModels
{
    public class CommentModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 模型Id
        /// </summary>
        public string ModelId { get; set; }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 构件Id
        /// </summary>
        public string ComponentId { get; set; }

        /// <summary>
        /// 构件名称
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// 评论信息
        /// </summary>
        public string Comment { get; set; }
    }
}
