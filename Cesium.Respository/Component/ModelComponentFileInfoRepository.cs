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
    public class ModelComponentFileInfoRepository : BaseRepository<ModelComponentFileInfo, int>, IDependency, IModelComponentFileInfoRepository
    {
        public ModelComponentFileInfoRepository(IOptionsSnapshot<DbOption> options) : base(options.Get("DbOption"))
        {
        }
    }
}
