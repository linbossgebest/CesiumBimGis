using Cesium.Core;
using Cesium.Core.Extensions;
using Cesium.IRepository.System;
using Cesium.Models.System;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Respository.System
{
    public class SysRoleMenuRepository : BaseRepository<SysRoleMenu, int>, IDependency, ISysRoleMenuRepository
    {
        public SysRoleMenuRepository(IOptionsSnapshot<DbOption> options) : base(options.Get("DbOption"))
        {
        }

        public async Task<int> AddListAsync(List<SysRoleMenu> list)
        {
            string sql = @" INSERT INTO SysRoleMenu (RoleId,MenuId,CreateTime,CreatorId,CreatorName,ModifyTime,ModifyId,ModifyName)
             VALUES(@RoleId,@MenuId,@CreateTime,@CreatorId,@CreatorName,@ModifyTime,@ModifyId,@ModifyName); ";
            return await _dbConnection.ExecuteAsync(sql, list);
        }
    }
}
