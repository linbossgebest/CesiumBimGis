using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CesiumBimGisApi.Controllers
{
    /// <summary>
    /// 抽象基类控制器
    /// </summary>
    //[Authorize]//添加Authorize标签
    //[EnableCors("any")]
    public abstract class BaseController: ControllerBase
    {
    }
}
