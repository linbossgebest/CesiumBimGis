using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Core.CustomEnum
{
    /// <summary>
    /// 登陆类型
    /// </summary>
    public enum LoginType
    {
        /// <summary>
        /// 登陆
        /// </summary>
        Login = 0,

        /// <summary>
        /// 登出
        /// </summary>
        LogOut = 1,

        /// <summary>
        /// 注册
        /// </summary>
        Register = 2,

        /// <summary>
        /// 改密
        /// </summary>
        ChangePassWord = 3,

        /// <summary>
        /// 三方授权登陆
        /// </summary>
        AuthorizedLogin = 4

    }
}
