using Cesium.Core;
using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Respository
{
    public class ModelComponentTypeRepository:BaseRepository<ModelComponentType, int>, IDependency, IModelComponentTypeRepository
    {
        public ModelComponentTypeRepository(IOptionsSnapshot<DbOption> options) : base(options.Get("DbOption"))
        {
        }
    }
}
