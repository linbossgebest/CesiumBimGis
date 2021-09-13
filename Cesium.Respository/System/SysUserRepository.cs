using Cesium.Core;
using Cesium.Core.DbHelper;
using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.Models.System;
using Cesium.ViewModels.System;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Cesium.Core.Helper;
using Cesium.ViewModels.ResultModel;

namespace Cesium.Respository
{
    public class SysUserRepository : BaseRepository<SysUser, int>, IDependency, ISysUserRepository
    {
        public SysUserRepository(IOptionsSnapshot<DbOption> options) : base(options.Get("DbOption"))
        {
        }

        public bool AddOrUpdate(UserModel model, TokenInfo tokenInfo)
        {
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    SysUser user;
                    List<SysUserRole> sysUserRoles = new List<SysUserRole>();
                    if (model.Id == 0)
                    {
                        user = new SysUser
                        {
                            UserName = model.UserName,
                            PassWord = AESEncryptHelper.Encode(model.PassWord.Trim(), CesiumKeys.AesEncryptKeys),
                            Mobile = model.Mobile,
                            Email = model.Email,
                            CreateTime = DateTime.Now,
                            CreatorId = tokenInfo.UserId,
                            CreatorName = tokenInfo.UserName,
                            IsEnabled = 1
                        };

                        user.Id = (int)_dbConnection.Insert(user);//新增用户信息
                    }
                    else
                    {
                        user = _dbConnection.Get<SysUser>(model.Id);
                        user.UserName = model.UserName;
                        user.Mobile = model.Mobile;
                        user.Email = model.Email;
                        user.ModifyTime = DateTime.Now;
                        user.ModifyId = tokenInfo.UserId;
                        user.ModifyName = tokenInfo.UserName;

                        _dbConnection.Update(user);//修改用户信息
                    }
                    foreach (var item in model.RoleIds)
                    {
                        SysUserRole sysUserRole = new()
                        {
                            UserId = user.Id,
                            RoleId = item
                        };
                        sysUserRoles.Add(sysUserRole);
                    }
                    string sql = @"INSERT INTO SysUserRole (UserId,RoleId) VALUES (@UserId,@RoleId); ";

                    _dbConnection.DeleteList<SysUserRole>(new { UserId = user.Id });//删除该用户角色信息
                    int nums = _dbConnection.Execute(sql, sysUserRoles);//添加该用户角色信息

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserInfo(int userId)
        {
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    //删除用户信息
                    await _dbConnection.DeleteAsync<SysUser>(userId);
                    //删除该用户对应的用户-角色表关联信息
                    await _dbConnection.DeleteListAsync<SysUserRole>(new { UserId = userId });
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
