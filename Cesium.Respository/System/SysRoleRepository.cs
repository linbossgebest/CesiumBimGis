using Cesium.Core;
using Cesium.Core.Extensions;
using Cesium.IRepository.System;
using Cesium.Models.System;
using Cesium.ViewModels.System;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Respository.System
{
    public class SysRoleRepository : BaseRepository<SysRole, int>, IDependency, ISysRoleRepository
    {
        public SysRoleRepository(IOptionsSnapshot<DbOption> options) : base(options.Get("DbOption"))
        {
        }

        public async Task<bool> AddOrUpdate(RoleModel model, TokenInfo tokenInfo)
        {
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    SysRole role;
                    if (model.Id == 0)
                    {
                        role = new SysRole
                        {
                            RoleName = model.RoleName,
                            RoleType = model.RoleType,
                            OrderNo = model.OrderNo,
                            CreateTime = DateTime.Now,
                            CreatorId = tokenInfo.UserId,
                            CreatorName = tokenInfo.UserName
                        };
                        await _dbConnection.InsertAsync<SysRole>(role);
                    }
                    else
                    {
                        role = await _dbConnection.GetAsync<SysRole>(model.Id);
                        role.RoleName = model.RoleName;
                        role.RoleType = model.RoleType;
                        role.OrderNo = model.OrderNo;
                        role.ModifyTime = DateTime.Now;
                        role.ModifyId = tokenInfo.UserId;
                        role.ModifyName = tokenInfo.UserName;

                        await _dbConnection.UpdateAsync<SysRole>(role);
                    }

                    string sql = @"INSERT INTO SysRoleMenu (RoleId,MenuId,CreateTime,CreatorId,CreatorName,ModifyTime,ModifyId,ModifyName) VALUES (@RoleId,@MenuId,@CreateTime,@CreatorId,@CreatorName,@ModifyTime,@ModifyId,@ModifyName); ";

                    _dbConnection.Execute(sql, model.SysRoleMenus);//添加该用户角色信息

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
