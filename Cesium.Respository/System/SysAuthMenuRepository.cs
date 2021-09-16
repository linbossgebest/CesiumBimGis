using Cesium.Core;
using Cesium.Core.Extensions;
using Cesium.IRepository.System;
using Cesium.Models.System;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Cesium.ViewModels.System;

namespace Cesium.Respository.System
{
    public class SysAuthMenuRepository: BaseRepository<SysAuthMenu, int>, IDependency, ISysAuthMenuRepository
    {
        public SysAuthMenuRepository(IOptionsSnapshot<DbOption> options) : base(options.Get("DbOption"))
        {
        }

        /// <summary>
        /// 添加或修改菜单信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        public async Task<bool> AddOrUpdate(SysAuthMenu model, TokenInfo tokenInfo)
        {
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    SysAuthMenu sysAuthMenu;
                    if (model.Id == 0)
                    {
                        sysAuthMenu = new SysAuthMenu
                        {
                            ParentId = model.ParentId,
                            Path=model.Path,
                            Name=model.Name,
                            Component=model.Component,
                            Hidden=model.Hidden,
                            AlwaysShow=model.AlwaysShow,
                            Redirect=model.Redirect,
                            OrderNo=model.OrderNo,
                            CreateTime = DateTime.Now,
                            CreatorId = tokenInfo.UserId,
                            CreatorName = tokenInfo.UserName,
                        };
                        sysAuthMenu.Id = (int)await _dbConnection.InsertAsync(sysAuthMenu, transaction);
                        SysAuthMenuMeta sysAuthMenuMeta = model.Meta;
                        sysAuthMenuMeta.MenuId = sysAuthMenu.Id;
                        sysAuthMenuMeta.Title = sysAuthMenu.Name;
                        await _dbConnection.InsertAsync(sysAuthMenuMeta, transaction);//插入菜单对应的元数据
                    }
                    else
                    {
                        sysAuthMenu = _dbConnection.Get<SysAuthMenu>(model.Id);
                        sysAuthMenu.ParentId = model.ParentId;
                        sysAuthMenu.Path = model.Path;
                        sysAuthMenu.Name = model.Name;
                        sysAuthMenu.Component = model.Component;
                        sysAuthMenu.Hidden = model.Hidden;
                        sysAuthMenu.AlwaysShow = model.AlwaysShow;
                        sysAuthMenu.Redirect = model.Redirect;
                        sysAuthMenu.OrderNo = model.OrderNo;
                        sysAuthMenu.ModifyTime = DateTime.Now;
                        sysAuthMenu.ModifyId = tokenInfo.UserId;
                        sysAuthMenu.ModifyName = tokenInfo.UserName;

                        await _dbConnection.UpdateAsync(sysAuthMenu, transaction);
                        SysAuthMenuMeta sysAuthMenuMeta = model.Meta;
                        sysAuthMenuMeta.MenuId = sysAuthMenu.Id;
                        sysAuthMenuMeta.Title = sysAuthMenu.Name;
                        await _dbConnection.UpdateAsync(sysAuthMenuMeta, transaction);//更新菜单对应的元数据
                    }
                  

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }


        /// 删除菜单信息以及关联的子菜单信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteMenuInfo(int menuId)
        {
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    await _dbConnection.DeleteAsync<SysAuthMenu>(menuId, transaction);//删除该菜单信息
                    string deleteSql = "Delete from SysAuthMenuMeta where MenuId = @menuId";
                    await _dbConnection.ExecuteAsync(deleteSql, new { menuId }, transaction);
                    /*  await _dbConnection.DeleteAsync<SysAuthMenuMeta>(new { MenuId = menuId }, transaction);*///删除该菜单元数据
                    var list = await _dbConnection.GetListAsync<SysAuthMenu>(new { ParentId = menuId });//获取所有以该菜单Id作为父Id的菜单列表
                    if (list.Any())//如果存在子菜单则删除
                    {
                        List<int> menuIds = new List<int>();
                        foreach (var item in list)
                        {
                            menuIds.Add(item.Id);
                        }
                        string sql = "Delete from SysAuthMenuMeta where MenuId in @menuIds";
                        await _dbConnection.DeleteListAsync<SysAuthMenu>(new { ParentId = menuId }, transaction);
                        await _dbConnection.ExecuteAsync(sql, new { menuIds }, transaction);
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public async Task<IEnumerable<SysAuthMenu>> GetSysAuthMenuListAsync()
        {
            string sql = "SELECT * FROM SysAuthMenu sam INNER JOIN SysAuthMenuMeta samm ON sam.Id = samm.MenuId ";
            HashSet<SysAuthMenu> list = new();
            SysAuthMenu item = null;
            var result = await _dbConnection.QueryAsync<SysAuthMenu, SysAuthMenuMeta, SysAuthMenu>(sql,(sysAuthMenu,sysAuthMenuMeta)=> {
                if (item == null || item.Id != sysAuthMenu.Id)
                    item = sysAuthMenu;

                if (sysAuthMenuMeta != null)
                    item.Meta = sysAuthMenuMeta;

                if (!list.Any(m => m.Id == item.Id))
                    list.Add(item);

                return null;
            });

            return list;
        }

        public async Task<IEnumerable<SysAuthMenu>> GetSysAuthMenuListAsync(List<int> menuIds)
        {
            string sql = "SELECT * FROM SysAuthMenu sam INNER JOIN SysAuthMenuMeta samm ON sam.Id = samm.MenuId WHERE sam.Id in @menuIds";
            HashSet<SysAuthMenu> list = new();
            SysAuthMenu item = null;
            var result = await _dbConnection.QueryAsync<SysAuthMenu, SysAuthMenuMeta, SysAuthMenu>(sql, (sysAuthMenu, sysAuthMenuMeta) =>
            {
                if (item == null || item.Id != sysAuthMenu.Id)
                    item = sysAuthMenu;

                if (sysAuthMenuMeta != null)
                    item.Meta = sysAuthMenuMeta;

                if (!list.Any(m => m.Id == item.Id))
                    list.Add(item);

                return null;
            }, new { menuIds = menuIds });

            return list;
        }

    }
}
