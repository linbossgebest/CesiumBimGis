using Cesium.Core.Extensions;
using Cesium.Core.Helper;
using Cesium.IRepository;
using Cesium.IServices;
using Cesium.Models.System;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Services
{
    public class SysUserService : ISysUserService, IDependency
    {
        private readonly ISysUserRepository _sysUserRepository;

        public SysUserService(ISysUserRepository sysUserRepository)
        {
            _sysUserRepository = sysUserRepository;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model">登录模型</param>
        /// <returns></returns>
        public async Task<SysUser> SignInAsync(LoginModel model)
        {
            model.password = AESEncryptHelper.Encode(model.password.Trim(), CesiumKeys.AesEncryptKeys);
            model.username = model.username.Trim();
            string conditions = $"select * from {nameof(SysUser)} where IsEnabled=1 ";//可用用户
            conditions += $"and (UserName = @UserName or Mobile =@UserName or Email =@UserName) and Password=@Password";
            var user = await _sysUserRepository.GetAsync(conditions, model);

            return user;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        public async Task<SysUser> GetUserInfoAsync(TokenInfo tokenInfo) 
        {
            var user = await _sysUserRepository.GetAsync(tokenInfo.UserId);

            return user;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        public async Task<BaseResult> AddUserAsync(UserModel model, TokenInfo tokenInfo)
        {
            var result = new BaseResult();

            SysUser user = new SysUser
            {
                UserName = model.UserName,
                PassWord = AESEncryptHelper.Encode(model.PassWord.Trim(), CesiumKeys.AesEncryptKeys),
                Mobile = model.Mobile,
                Email = model.Email,
                CreateTime = DateTime.Now,
                CreatorId = tokenInfo.UserId,
                CreatorName = tokenInfo.UserName
            };

            if (await _sysUserRepository.InsertAsync(user) > 0)
            {
                result.IsSuccess = true;
                result.Code = ResultCodeMsg.CommonSuccessCode;
                result.Message = ResultCodeMsg.CommonSuccessMsg;
            }
            else
            {
                result.IsSuccess = false;
                result.Code = ResultCodeMsg.CommonFailCode;
                result.Message = ResultCodeMsg.CommonFailMsg;
            }


            return result;
        }


    }
}
