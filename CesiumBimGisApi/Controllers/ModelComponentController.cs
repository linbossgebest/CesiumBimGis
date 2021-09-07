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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Cesium.ViewModels.SceneTreeModel;

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
        /// 根据构件类型编号获取构件菜单
        /// </summary>
        /// <param name="componentId">构件编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetComponentMenu")]
        [AllowAnonymous]
        public async Task<BaseResult> GetComponentDataSourceList(string componentId)
        {
            BaseResult result = new BaseResult();

            var component = await _modelComponentService.GetComponentInfoAsync(componentId);

            if (component != null)
            {
                var componentTypeId = component.ComponentTypeId;//获取该构件的构件类型编号
                var list = await _modelComponentDataSourceService.GetComponentDataSourceListAsync(componentTypeId);

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

        #region 构件信息

        /// <summary>
        /// 查询所有构件信息
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页的数量</param>
        /// <param name="componentId">构件编号</param>
        /// <param name="componentName">构件名称</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetComponents")]
        [AllowAnonymous]
        public async Task<BaseResult> GetAllComponents(string componentId, string componentName, int pageIndex, int pageSize)
        {
            BaseResult result = new BaseResult();
            var components = await _modelComponentService.GetComponentsAsync(componentId, componentName);
            var total = components.Count();

            components = components.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            if (components != null)
            {
                var data = new
                {
                    items = components,
                    total = total
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
        /// 上传构件json文件
        /// </summary>
        /// <param name="fileinput"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadComponents")]
        [AllowAnonymous]
        public async Task<string> UploadComponentFile(IFormFile fileinput)
        {
            var stream = fileinput.OpenReadStream();
            string json = "";
            using (StreamReader sr = new StreamReader(stream))
            {
                json = sr.ReadToEnd().ToString();
            }

            var list = Json2Models(json);
            var result = await _modelComponentService.AddModelComponentListAsync(list);

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
                    ComponentTypeId = 1
                };
                list.Add(item);
            }

            var result = await _modelComponentService.AddModelComponentListAsync(list);
            return JsonHelper.ObjectToJSON(result);
        }



        #region private method

        private List<ModelComponent> Json2Models(string json)
        {
            List<ModelComponent> modelComponents = new List<ModelComponent>();
            Rootobject root = JsonConvert.DeserializeObject<Rootobject>(json);//反序列化对象
            foreach (Scene scene in root.scenes)
            {
                foreach (Child child in scene.children)
                {
                    HasChild(child, modelComponents, scene.id);
                }
            }

            return modelComponents;
        }

        private static bool HasChild(Child child, List<ModelComponent> modelComponents, string ParentId)
        {
            if (child.children != null)
            {
                var children = child.children;
                foreach (Child childItem in children)
                {
                    HasChild(childItem, modelComponents, child.id);
                }
            }
            ModelComponent modelComponent = new ModelComponent();
            modelComponent.ComponentId = child.id;
            modelComponent.ComponentName = child.name;
            modelComponent.ComponentTypeId = 1;
            modelComponent.ParentId = ParentId;
            modelComponent.Status = ComponentStatus.InProgress;
            modelComponent.ModelId = 99999;
            modelComponents.Add(modelComponent);

            return true;
        }



        #endregion


    }
}
