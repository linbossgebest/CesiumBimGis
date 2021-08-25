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
            model.Password = AESEncryptHelper.Encode(model.Password.Trim(), CesiumKeys.AesEncryptKeys);
            model.UserName = model.UserName.Trim();
            string conditions = $"select * from {nameof(SysUser)} where IsEnabled=1 ";//可用用户
            conditions += $"and (UserName = @UserName or Mobile =@UserName or Email =@UserName) and Password=@Password";
            var user = await _sysUserRepository.GetAsync(conditions, model);

            return user;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResult> AddUserAsync(UserModel model)
        {
            var result = new BaseResult();
            if (model.Id == 0)
            {
                SysUser user = new SysUser();
                user.UserName = model.UserName;
                user.PassWord= AESEncryptHelper.Encode(model.PassWord.Trim(), CesiumKeys.AesEncryptKeys);
                user.Mobile = model.Mobile;
                user.Email = model.Email;
                user.CreateTime = DateTime.Now;
                user.CreatorId = 1;
                user.CreatorName = "admin";
                

                if (await _sysUserRepository.InsertAsync(user) > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "操作成功";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "操作失败";
                }
            }

            return result;
        }


    }
}
