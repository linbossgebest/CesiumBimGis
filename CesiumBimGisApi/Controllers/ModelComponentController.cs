using Cesium.Core.Helper;
using Cesium.IServices;
using Cesium.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CesiumBimGisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelComponentController : ControllerBase
    {
        private readonly IModelComponentCommentService _modelComponentCommentService;

        public ModelComponentController(IModelComponentCommentService modelComponentCommentService)
        {
            _modelComponentCommentService = modelComponentCommentService;
        }

        [HttpPost]
        [Route("AddComment")]
        public async Task<string> AddComment(CommentModel model)
        {
            var result = await _modelComponentCommentService.AddCommentAsync(model);
            return JsonHelper.ObjectToJSON(result);
        }
    }
}
