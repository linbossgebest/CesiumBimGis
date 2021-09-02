using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.ViewModels.ResultModel
{
    public class BaseResult
    {
        /// <summary>
        /// 是否操作成功
        /// </summary>
        public bool isSuccess { get; set; }

        /// <summary>
        /// 是否登录
        /// </summary>
        //public bool IsLogin { get; set; }

        /// <summary>
        /// 结果编码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 结果消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 传递的数据
        /// </summary>
        public string data { get; set; }
    }
}
