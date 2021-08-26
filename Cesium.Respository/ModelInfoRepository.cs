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
    public class ModelInfoRepository : BaseRepository<ModelInfo, int>, IDependency, IModelInfoRepository
    {
        public ModelInfoRepository(IOptionsSnapshot<DbOption> options) : base(options.Get("DbOption"))
        {
        }
    }
}
