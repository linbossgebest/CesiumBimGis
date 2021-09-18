using Cesium.Core.CustomEnum;
using Cesium.Core.Extensions;
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
using Microsoft.Extensions.Hosting;
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
        private readonly IModelComponentTypeService _modelComponentTypeService;
        private readonly IModelComponentFileInfoService _modelComponentFileInfoService;
        private readonly IHostEnvironment _hostingEnvironment;


        public ModelComponentController(IModelComponentCommentService modelComponentCommentService, IModelInfoService modelInfoService, IModelComponentService modelComponentService, IModelComponentDataSourceService modelComponentDataSourceService, IModelComponentTypeService modelComponentTypeService, IModelComponentFileInfoService modelComponentFileInfoService, IHostEnvironment hostingEnvironment)
        {
            _modelComponentCommentService = modelComponentCommentService;
            _modelInfoService = modelInfoService;
            _modelComponentService = modelComponentService;
            _modelComponentDataSourceService = modelComponentDataSourceService;
            _modelComponentTypeService = modelComponentTypeService;
            _modelComponentFileInfoService = modelComponentFileInfoService;
            _hostingEnvironment = hostingEnvironment;
        }

        #region 构件菜单

        /// <summary>
        /// 根据构件编号获取构件菜单
        /// </summary>
        /// <param name="componentId">构件编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetComponentMenu")]
        [AllowAnonymous]
        public async Task<BaseResult> GetComponentDataSourceList(string componentId)
        {
            BaseResult result = new BaseResult();

            var component = await _modelComponentService.GetComponentInfoAsync(componentId);//获取构件信息

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
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页的数量</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllComponentMenu")]
        public async Task<BaseResult> GetAllComponentDataSourceList(int pageIndex, int pageSize)
        {
            BaseResult result = new BaseResult();

            var list = await _modelComponentDataSourceService.GetAllComponentDataSourceListAsync();
            var total = list.Count();
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            if (list != null)
            {
                var data = new
                {
                    items = list,
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
        /// 新增修改构件菜单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddComponentMenu")]
        public async Task<string> AddOrModifyComponentDataSource([FromBody] ComponentMenuModel model)
        {
            var info = HttpContext.AuthenticateAsync().Result.Principal.Claims;//获取用户身份信息
            TokenInfo tokenInfo = new()
            {
                UserId = Int32.Parse(info.FirstOrDefault(f => f.Type.Equals("UserId")).Value),
                UserName = info.FirstOrDefault(f => f.Type.Equals(ClaimTypes.Name)).Value
            };
            var result = await _modelComponentDataSourceService.AddOrModifyComponentDataSourceAsync(model, tokenInfo);
            return JsonHelper.ObjectToJSON(result);
        }

        /// <summary>
        /// 删除构件菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteComponentMenu")]
        public async Task<BaseResult> DeleteComponentDataSource(int id)
        {
            return await _modelComponentDataSourceService.DeleteComponentDataSource(id);
        }

        /// <summary>
        /// 通过构件编号获取菜单信息
        /// </summary>
        /// <param name="componentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetComponentDataSourceByComponentId")]
        public async Task<BaseResult> GetComponentDataSourceByComponentId(string componentId)
        {
            BaseResult result = new BaseResult();
            var sources=await _modelComponentDataSourceService.GetComponentDataSourceListByComponentIdAsync(componentId);
            if (sources != null)
            {
                var data = new
                {
                    items = sources
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

        #region 构件菜单类型

        /// <summary>
        /// 添加或修改构件菜单类型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddComponentType")]
        public async Task<string> AddOrModifyComponentType([FromBody] ComponentTypeModel model)
        {
            var info = HttpContext.AuthenticateAsync().Result.Principal.Claims;//获取用户身份信息
            TokenInfo tokenInfo = new()
            {
                UserId = Int32.Parse(info.FirstOrDefault(f => f.Type.Equals("UserId")).Value),
                UserName = info.FirstOrDefault(f => f.Type.Equals(ClaimTypes.Name)).Value
            };
            var result = await _modelComponentTypeService.AddOrModifyComponentTypeAsync(model, tokenInfo);
            return JsonHelper.ObjectToJSON(result);
        }

        /// <summary>
        /// 查询所有构件类型
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页的数量</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetComponentTypes")]
        public async Task<BaseResult> GetComponentTypeList(int pageIndex, int pageSize)
        {
            BaseResult result = new BaseResult();
            var types = await _modelComponentTypeService.GetComponentTypesAsync();
            var total = types.Count();

            types = types.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var data = new
            {
                items = types,
                total = total
            };

            result.isSuccess = true;
            result.code = ResultCodeMsg.CommonSuccessCode;
            result.message = ResultCodeMsg.CommonSuccessMsg;
            result.data = JsonHelper.ObjectToJSON(data);

            return result;
        }


        /// <summary>
        /// 根据类型Id删除类型信息
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteComponentType")]
        public async Task<BaseResult> DeleteComponentTypeInfo(int typeId)
        {
            return await _modelComponentTypeService.DeleteComponentType(typeId);
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
        /// 新增构件信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddComponent")]
        public async Task<BaseResult> AddComponentInfo([FromBody] ModelComponent model)
        {
            return await _modelComponentService.AddModelComponentAsync(model);
        }

        /// <summary>
        /// 修改构件信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateComponent")]
        public async Task<BaseResult> UpdateComponentInfo([FromBody] ModelComponent model)
        {
            return await _modelComponentService.UpdateModelComponentAsync(model);
        }

        /// <summary>
        /// 删除构件信息
        /// </summary>
        /// <param name="componentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteComponent")]
        public async Task<BaseResult> DeleteComponentInfo(string componentId)
        {
            return await _modelComponentService.DeleteModelComponent(componentId);
        }

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

        /// <summary>
        /// 上传excel文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadComponentsExcel")]
        [AllowAnonymous]
        public async Task<string> UploadComponentFileExcel(IFormFile file)
        {
            List<ModelComponent> components = ImportExcelUtil<ModelComponent>.InputExcel(file);

            //var result = new { };
            var result = await _modelComponentService.AddModelComponentListAsync(components);

            return JsonHelper.ObjectToJSON(result);
        }

        [HttpGet]
        [Route("DownloadComponentsExcelTemplate")]
        [AllowAnonymous]
        public FileStreamResult DownloadComponentTemplate()
        {
            //var FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/download/EditPlus64_xp85.com.zip");
            //var stream = new FileStream(FilePath, FileMode.Open);
            //return File();
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var filePath = Path.Combine(contentRootPath, "temp", "component.xlsx");
            var stream = new FileStream(filePath, FileMode.Open);

            return File(stream, "xlsx/xls");
        }

        #endregion

        #region 构件文件信息

        /// <summary>
        /// 获取构件对应文件信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="componentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetComponentFiles")]
        public async Task<BaseResult> GetComponentFileList(int pageIndex, int pageSize, string componentId)
        {
            BaseResult result = new BaseResult();
            var files = await _modelComponentFileInfoService.GetComponentFilesAsync(componentId);
            var total = files.Count();

            files = files.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var data = new
            {
                items = files,
                total = total
            };

            result.isSuccess = true;
            result.code = ResultCodeMsg.CommonSuccessCode;
            result.message = ResultCodeMsg.CommonSuccessMsg;
            result.data = JsonHelper.ObjectToJSON(data);

            return result;
        }

        /// <summary>
        /// 上传构件对应文件信息
        /// </summary>
        /// <param name="file">上传文件</param>
        /// <param name="modelId">模型编号</param>
        /// <param name="componentId">构件编号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadComponentFile")]
        [AllowAnonymous]
        public async Task<BaseResult> UploadComponentFile(IFormFile file, int modelId, string componentId)
        {
            #region 上传文件到静态服务器

            var fileName = file.FileName;
            var fileExt = Path.GetExtension(file.FileName);
            var path = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot\\componentresources\\" + fileName);
            await using var stream = System.IO.File.Create(path);
            await file.CopyToAsync(stream);

            #endregion

            #region 添加构件对应文件信息

            var info = HttpContext.AuthenticateAsync().Result.Principal.Claims;//获取用户身份信息
            TokenInfo tokenInfo = new()
            {
                UserId = Int32.Parse(info.FirstOrDefault(f => f.Type.Equals("UserId")).Value),
                UserName = info.FirstOrDefault(f => f.Type.Equals(ClaimTypes.Name)).Value
            };

            ModelComponentFileInfo model = new()
            {
                ModelId = modelId,
                ComponentId = componentId,
                FileName = fileName,
                FileSrc = fileName,
                FilePath = path,
                FileType = fileExt,
                CreateTime = DateTime.Now,
                CreatorId = tokenInfo.UserId,
                CreatorName = tokenInfo.UserName
            };

            #endregion

            return await _modelComponentFileInfoService.AddModelComponentFileInfoAsync(model);

        }

        /// <summary>
        /// 删除构件对应文件信息
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteComponentFile")]
        public async Task<BaseResult> DeleteComponentFileInfo(int fileId)
        {
            var fileInfo = await _modelComponentFileInfoService.GetModelComponentFileById(fileId);

            var path = fileInfo.FilePath;

            if (!path.IsNullOrEmpty())
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            return await _modelComponentFileInfoService.DeleteComponentFileInfoAsync(fileId);
        }

        [HttpPost]
        [Route("UpdateComponentFile")]
        public async Task<BaseResult> UpdateModelComponentFileInfoAsync([FromBody] ComponentFileModel model)
        {
            return await _modelComponentFileInfoService.UpdateModelComponentFileInfoAsync(model);
        }

        [HttpGet]
        [Route("GetComponentFile")]
        [AllowAnonymous]
        public async Task<BaseResult> GetComponentFile(string componentId, string menuName)
        {
            BaseResult result = new BaseResult();
            var fileInfo=  await _modelComponentFileInfoService.GetModelComponentFileByComponentIdAndMenuName(componentId, menuName);

            if (fileInfo != null)
            {
                var data = new
                {
                    fileInfo
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
