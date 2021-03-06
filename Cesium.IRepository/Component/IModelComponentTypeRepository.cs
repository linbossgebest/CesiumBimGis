using Cesium.Core.Extensions;
using Cesium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IRepository
{
    public interface IModelComponentTypeRepository: IDependency, IBaseRepository<ModelComponentType, int>
    {
    }
}
