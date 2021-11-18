using Cesium.Core.Extensions;
using Cesium.Models;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.IServices
{
    public interface IModelInfoService : IDependency
    {
        Task<ResponseResult> AddModelInfoAsync(ModelInfo model, TokenInfo tokenInfo);
    }
}
