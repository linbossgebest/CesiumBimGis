using Cesium.Core.CustomEnum;
using Cesium.Core.Helper;
using Cesium.IServices;
using Cesium.Models;
using Cesium.ViewModels;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CesiumBimGisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelComponentController : BaseController
    {
        private readonly IModelComponentCommentService _modelComponentCommentService;
        private readonly IModelInfoService _modelInfoService;
        private readonly IModelComponentService _modelComponentService;
        private readonly IModelComponentDataSourceService _modelComponentDataSourceService;

        public ModelComponentController(IModelComponentCommentService modelComponentCommentService, IModelInfoService modelInfoService, IModelComponentService modelComponentService, IModelComponentDataSourceService modelComponentDataSourceService)
        {
            _modelComponentCommentService = modelComponentCommentService;
            _modelInfoService = modelInfoService;
            _modelComponentService = modelComponentService;
            _modelComponentDataSourceService = modelComponentDataSourceService;
        }

        #region 构件菜单

        /// <summary>
        /// 根据构件Id获取构件菜单
        /// </summary>
        /// <param name="componentId">构件编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetComponentMenu")]
        [AllowAnonymous]
        public async Task<BaseResult> GetComponentDataSourceList(string componentId)
        {
            BaseResult result = new BaseResult();

            var list = await _modelComponentDataSourceService.GetComponentDataSourceListAsync(componentId);

            if (list != null)
            {
                var data = new
                {
                    list
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

        /// <summary>
        /// 获取所有构件菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllComponentMenu")]
        public async Task<BaseResult> GetAllComponentDataSourceList()
        {
            BaseResult result = new BaseResult();

            var list = await _modelComponentDataSourceService.GetAllComponentDataSourceListAsync();

            if (list != null)
            {
                var data = new
                {
                    list
                };

                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
                result.data = JsonHelper.ObjectToJSON(data);
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

        #endregion

        #region 构件评论

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddComment")]
        public async Task<string> AddComment([FromBody] CommentModel model)
        {
            var result = await _modelComponentCommentService.AddCommentAsync(model);
            return JsonHelper.ObjectToJSON(result);
        }

        #endregion


        /// <summary>
        /// 添加模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddModel")]
        public async Task<string> AddModelInfo([FromBody] ModelInfo model)
        {
            var info = HttpContext.AuthenticateAsync().Result.Principal.Claims;//获取用户身份信息
            TokenInfo tokenInfo = new()
            {
                UserId = Int32.Parse(info.FirstOrDefault(f => f.Type.Equals("UserId")).Value),
                UserName = info.FirstOrDefault(f => f.Type.Equals(ClaimTypes.Name)).Value
            };
            var result = await _modelInfoService.AddModelInfoAsync(model, tokenInfo);
            return JsonHelper.ObjectToJSON(result);
        }

        [HttpGet]
        [Route("Test")]
        [AllowAnonymous]
        public async Task<string> TestModelComponentList()
        {
            List<ModelComponent> list = new List<ModelComponent>();
            for (int i = 0; i < 10; i++)
            {
                ModelComponent item = new ModelComponent()
                {
                    ComponentId = i.ToString(),
                    ComponentName = i.ToString() + "名称",
                    ModelId = 1,
                    Status = ComponentStatus.InProgress,
                    CompletedTime = DateTime.Now,
                    ParentId = "0",
                    Type = ComponentType.node

                };
                list.Add(item);
            }

            var result = await _modelComponentService.AddModelComponentListAsync(list);
            return JsonHelper.ObjectToJSON(result);
        }
    }
}
