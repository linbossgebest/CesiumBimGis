using Cesium.Core.Extensions;
using Cesium.IRepository.System;
using Cesium.IServices.System;
using Cesium.Models.System;
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
            List<SysAuthMenu> children = menus.Where(f => f.ParentId == parentId).ToList();
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
