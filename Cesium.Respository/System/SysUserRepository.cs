using Cesium.Core;
using Cesium.Core.DbHelper;
using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.Models.System;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Respository
{
    public class SysUserRepository : BaseRepository<SysUser, int>, IDependency, ISysUserRepository
    {
        public SysUserRepository(IOptionsSnapshot<DbOption> options):base(options.Get("DbOption"))
        {
        }

    }
}
