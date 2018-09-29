using Hk.Core.Business.BaseBusiness;
using Hk.Core.Business.Cache;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Autofac.Core;
using Hk.Core.Data.DbContextCore;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Helper;

namespace Hk.Core.Business.Base_SysManage
{
    public class Base_SysRoleBusiness : BaseBusiness<Base_SysRole,string>
    {
        private IBasePermissionRoleRepository _basePermissionRoleRepository;
        public Base_SysRoleBusiness(IDbContextCore dbContext) : base(dbContext)
        {
            _basePermissionRoleRepository = Ioc.DefaultContainer.GetService<IBasePermissionRoleRepository>();
        }
        static Base_SysRoleCache _cache { get; } = new Base_SysRoleCache();
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<Base_SysRole> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = Get();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);

            return q.GetPagination(pagination).ToList();
        }

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public Base_SysRole GetTheData(string id)
        {
            return GetSingle(id);
        }

        public static string GetRoleName(string userId)
        {
            return _cache.GetCache(userId)?.RoleName;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public void AddData(Base_SysRole newData)
        {
            Add(newData);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void UpdateData(Base_SysRole theData)
        {
            Update(theData);
            _cache.UpdateCache(theData.RoleId);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public void DeleteData(List<string> ids)
        {
            var roleIds = Get().Where(x => ids.Contains(x.RoleId)).Select(x => x.RoleId).ToList();
            //删除角色
           ids.ForEach(x=>Delete(x));
            _cache.UpdateCache(roleIds);
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="permissions">权限值</param>
        public void SavePermission(string roleId, List<string> permissions)
        {
            _basePermissionRoleRepository.Delete(roleId);
            List<Base_PermissionRole> insertList = new List<Base_PermissionRole>();
            permissions.ForEach(newPermission =>
            {
                insertList.Add(new Base_PermissionRole
                {
                    Id = Guid.NewGuid().ToSequentialGuid(),
                    RoleId = roleId,
                    PermissionValue = newPermission
                });
            });

            _basePermissionRoleRepository.AddRange(insertList);
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion


    }
}