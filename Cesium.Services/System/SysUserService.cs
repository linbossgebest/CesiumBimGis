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
        /// 通过用户编号获取用户信息
        /// </summary>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        public async Task<SysUser> GetUserInfoAsync(int userId)
        {
            var user = await _sysUserRepository.GetAsync(userId);

            return user;
        }

        /// <summary>
        /// 添加或修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        public async Task<BaseResult> AddOrModifyUserAsync(UserModel model, TokenInfo tokenInfo)
        {
            var result = new BaseResult();
            if (await _sysUserRepository.AddOrUpdate(model, tokenInfo))
            {
                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SysUser>> GetUsersAsync()
        {
            var users = await _sysUserRepository.GetListAsync();

            return users;
        }

        /// <summary>
        /// 通过用户编号删除用户信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public async Task<BaseResult> DeleteUserInfo(int userId)
        {
            var result = new BaseResult();
            if (await _sysUserRepository.DeleteUserInfo(userId))
            {
                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

    }
}
