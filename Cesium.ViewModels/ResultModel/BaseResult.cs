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
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 是否登录
        /// </summary>
        //public bool IsLogin { get; set; }

        /// <summary>
        /// 结果编码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 传递的数据
        /// </summary>
        public string Data { get; set; }
    }
}
