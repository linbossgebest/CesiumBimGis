﻿using Cesium.Core;
using Cesium.Core.Extensions;
using Cesium.IRepository.System;
using Cesium.Models.System;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Respository.System
{
    public class SysPermissionRepository : BaseRepository<SysPermission, int>, IDependency, ISysPermissionRepository
    {
        public SysPermissionRepository(IOptionsSnapshot<DbOption> options) : base(options.Get("DbOption"))
        {
        }
    }
}
