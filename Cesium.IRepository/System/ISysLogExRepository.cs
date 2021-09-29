using Cesium.Core.Extensions;
using Cesium.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IRepository.System
{
    public interface ISysLogExRepository : IDependency, IBaseRepository<SysLogEx, int>
    {
    }
}
