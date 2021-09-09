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
        bool AddOrUpdate(UserModel model, TokenInfo tokenInfo);
    }
}
