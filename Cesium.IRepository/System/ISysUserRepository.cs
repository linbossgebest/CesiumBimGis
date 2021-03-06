using Cesium.Core.Extensions;
using Cesium.Models.System;
using Cesium.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IRepository
{
    public interface ISysUserRepository : IDependency, IBaseRepository<SysUser, int>
    {
        /// <summary>
        /// 事务执行 插入user表以及userrole表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdate(UserModel model, TokenInfo tokenInfo);


        /// <summary>
        /// 事务执行 删除用户信息以及删除用户角色表关联信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteUserInfo(int userId);
    }
}
