using Cesium.Core.Extensions;
using Cesium.IRepository.System;
using Cesium.IServices.System;
using Cesium.Models.System;
using Cesium.ViewModels.ResultModel;
using Cesium.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Services.System
{
    public class SysAuthMenuService : ISysAuthMenuService, IDependency
    {
        private readonly ISysAuthMenuRepository _sysAuthMenuRepository;
 

        public SysAuthMenuService(ISysAuthMenuRepository sysAuthMenuRepository)
        {
            _sysAuthMenuRepository = sysAuthMenuRepository;
        }

        public async Task<ResponseResult> AddOrModifyMenuAsync(SysAuthMenu model, TokenInfo tokenInfo)
        {
            var result = new ResponseResult();
            if (await _sysAuthMenuRepository.AddOrUpdate(model, tokenInfo))
            {
                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
            }
            else
            {
                result.isSuccess = false;
                result.code = ResultCodeMsg.CommonFailCode;
                result.message = ResultCodeMsg.CommonFailMsg;
            }

            return result;
        }

        public async Task<ResponseResult> DelelteMenuInfo(int menuId)
        {
            var result = new ResponseResult();

            if (await _sysAuthMenuRepository.DeleteMenuInfo(menuId))
            {
                result.isSuccess = true;
                result.code = ResultCodeMsg.CommonSuccessCode;
                result.message = ResultCodeMsg.CommonSuccessMsg;
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
        /// 获取所有菜单信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SysAuthMenu>> GetMenuInfo()
        {
            var result =await _sysAuthMenuRepository.GetSysAuthMenuListAsync();
            return result;
        }

        /// <summary>
        /// 通过menuIds获取对应的菜单信息
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysAuthMenu>> GetMenuInfo(List<int> menuIds)
        {
            var result = await _sysAuthMenuRepository.GetSysAuthMenuListAsync(menuIds);
            return result;
        }

        /// <summary>
        /// 根据菜单信息生成菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuTree>> GetMenuTree(List<SysAuthMenu> menus)
        {   
            return await Task.Run(() => GetMenuTree(menus, 0));
            //return GetMenuTree(menus, 0);
        }

        /// <summary>
        /// 生成菜单树
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<MenuTree> GetMenuTree(List<SysAuthMenu> menus,int parentId)
        {
            List<MenuTree> tree = new List<MenuTree>();
            List<SysAuthMenu> children = menus.Where(f => f.ParentId == parentId).OrderBy(f=>f.OrderNo).ToList();
            if (children.Count > 0)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    MenuTree menuTree = new()
                    {
                        Id = children[i].Id,
                        ParentId=children[i].ParentId,
                        Path=children[i].Path,
                        Name=children[i].Name,
                        Component=children[i].Component,
                        Hidden=children[i].Hidden,
                        AlwaysShow=children[i].AlwaysShow,
                        Redirect=children[i].Redirect,
                        Meta=children[i].Meta,
                        OrderNo=children[i].OrderNo,
                        Children= GetMenuTree(menus,children[i].Id)
                    };
                    tree.Add(menuTree);
                }
            }
            return tree;
        }
    }
}
