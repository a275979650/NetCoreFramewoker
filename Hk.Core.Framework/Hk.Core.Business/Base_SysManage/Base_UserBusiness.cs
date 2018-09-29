using Hk.Core.Business.BaseBusiness;
using Hk.Core.Business.Cache;
using Hk.Core.Business.Common;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Hk.Core.Data.DbContextCore;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Helper;

namespace Hk.Core.Business.Base_SysManage
{
    public class Base_UserBusiness : BaseBusiness<Base_User,string>
    {
        private IBaseUserRoleMapRepository _baseUserRoleMapRepository;
        private IBasePermissionUserRepository _basePermissionUserRepository;
        private IBasePermissionRoleRepository _basePermissionRoleRepository;
        private static Base_UserModelCache _cache { get; } = new Base_UserModelCache(Ioc.DefaultContainer.GetService<IDbContextCore>());
        private static UserRoleCache _userRoleCache { get; } = new UserRoleCache();
        public Base_UserBusiness(IDbContextCore dbContext) : base(dbContext)
        {
            _baseUserRoleMapRepository = Ioc.DefaultContainer.GetService<IBaseUserRoleMapRepository>();
            _basePermissionUserRepository = Ioc.DefaultContainer.GetService<IBasePermissionUserRepository>();
            _basePermissionRoleRepository = Ioc.DefaultContainer.GetService<IBasePermissionRoleRepository>();
        }
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<Base_UserModel> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var whereExpre = LinqHelper.True<Base_UserModel>();
            Expression<Func<Base_User, Base_UserModel>> selectExpre = a => new Base_UserModel
            {

            };

            selectExpre = selectExpre.BuildExtendSelectExpre();

            var q = from a in Get().AsExpandable()
                    select selectExpre.Invoke(a);

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);
            //Service.HandleSqlLog = log =>
            //{
            //    LogHelper.WriteLog_LocalTxt(log);
            //};
            var list = q.GetPagination(pagination).ToList();

            return list;
        }

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public Base_User GetTheData(string id)
        {
            return GetSingle(id);
        }

        public void AddData(Base_User newData)
        {
            if (Get().Any(x => x.UserName == newData.UserName))
                throw new Exception("该用户名已存在！");

            Add(newData);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void UpdateData(Base_User theData)
        {
            if (theData.UserId == "Admin" && Operator.UserId != theData.UserId)
                throw new Exception("禁止更改超级管理员！");

            Update(theData);
        }

        public void SetUserRole(string userId, List<string> roleIds)
        {
            _baseUserRoleMapRepository.Delete(userId);
            var insertList = roleIds.Select(x => new Base_UserRoleMap
            {
                Id = GuidHelper.GenerateKey(),
                UserId = userId,
                RoleId = x
            }).ToList();

            _baseUserRoleMapRepository.AddRange(insertList);
            _cache.UpdateCache(userId);
            _userRoleCache.UpdateCache(userId);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public void DeleteData(List<string> ids)
        {
            var userIds = Get().Where(x => ids.Contains(x.Id)).Select(x => x.UserId).ToList();
            var adminUser = GetTheUser("Admin");
            if (ids.Contains(adminUser.Id))
                throw new Exception("超级管理员是内置账号,禁止删除！");

            ids.ForEach(x=>Delete(x));
            _cache.UpdateCache(ids);
            _userRoleCache.UpdateCache(ids);
        }

        /// <summary>
        /// 获取当前操作者信息
        /// </summary>
        /// <returns></returns>
        public static Base_UserModel GetCurrentUser()
        {
            return GetTheUser(Operator.UserId);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static Base_UserModel GetTheUser(string userId)
        {
            return _cache.GetCache(userId);
        }

        public static List<string> GetUserRoleIds(string userId)
        {
            return _userRoleCache.GetCache(userId);
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="oldPwd">老密码</param>
        /// <param name="newPwd">新密码</param>
        public AjaxResult ChangePwd(string oldPwd, string newPwd)
        {
            AjaxResult res = new AjaxResult() { Success = true };
            string userId = Operator.UserId;
            oldPwd = oldPwd.ToMD5String();
            newPwd = newPwd.ToMD5String();
            var theUser = Get().Where(x => x.UserId == userId && x.Password == oldPwd).FirstOrDefault();
            if (theUser == null)
            {
                res.Success = false;
                res.Msg = "原密码不正确！";
            }
            else
            {
                theUser.Password = newPwd;
                Update(theUser);
            }

            _cache.UpdateCache(userId);

            return res;
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="permissions">权限值</param>
        public void SavePermission(string userId, List<string> permissions)
        {
            _basePermissionUserRepository.Delete(userId);
            var roleIdList = _baseUserRoleMapRepository.Get().Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
            var existsPermissions =_basePermissionRoleRepository.Get()
                .Where(x => roleIdList.Contains(x.RoleId) && permissions.Contains(x.PermissionValue))
                .GroupBy(x => x.PermissionValue)
                .Select(x => x.Key)
                .ToList();
            permissions.RemoveAll(x => existsPermissions.Contains(x));

            List<Base_PermissionUser> insertList = new List<Base_PermissionUser>();

            permissions.ForEach(newPermission =>
            {
                insertList.Add(new Base_PermissionUser
                {
                    Id = Guid.NewGuid().ToSequentialGuid(),
                    UserId = userId,
                    PermissionValue = newPermission
                });
            });

            _basePermissionUserRepository.AddRange(insertList);
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion


    }
}