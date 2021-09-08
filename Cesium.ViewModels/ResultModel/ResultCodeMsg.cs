using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.ViewModels.ResultModel
{
    public class ResultCodeMsg
    {
        /// <summary>
        /// 通用成功编码
        /// </summary>
        public const int CommonSuccessCode = 0;

        /// <summary>
        /// 通用操作成功信息
        /// </summary>
        public const string CommonSuccessMsg = "操作成功";

        /// <summary>
        /// 通用失败编码
        /// </summary>
        public const int CommonFailCode = 1;

        /// <summary>
        /// 通用操作失败信息
        /// </summary>
        public const string CommonFailMsg = "操作失败";

        /// <summary>
        /// 通用失败，系统异常错误码
        /// </summary>
        public const int CommonExceptionCode = 2;
        /// <summary>
        /// 通用失败，系统异常信息
        /// </summary>
        public const string CommonExceptionMsg = "系统异常";

        /// <summary>
        /// 用户名或者密码错误
        /// </summary>
        public const int SignInPasswordOrUserNameErrorCode = 3;
        /// <summary>
        /// 用户名或者密码错误
        /// </summary>
        public const string SignInPasswordOrUserNameErrorMsg = "对不起，您输入的用户名或者密码错误";

        /// <summary>
        /// 编号重复
        /// </summary>
        public const int DuplicateNumberCode = 4;
        /// <summary>
        /// 编号重复
        /// </summary>
        public const string DuplicateNumberErrorMsg = "编号已存在,请确认输入信息";
    }
}
