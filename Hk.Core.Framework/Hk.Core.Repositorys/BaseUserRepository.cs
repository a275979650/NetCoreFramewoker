using Hk.Core.Business.Common;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Helper;
using Hk.Core.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Hk.Core.Business.Cache;

namespace Hk.Core.Repositorys
{
    public class BaseUserRepository:BaseRepository<Base_User,string>,IBaseUserRepository
    {
        private readonly IBaseUserRoleMapRepository _baseUserRoleMapRepository;
        private readonly IBasePermissionUserRepository _basePermissionUserRepository;
        private readonly IBasePermissionRoleRepository _basePermissionRoleRepository;
        private readonly Base_UserModelCache _cache;
        private readonly UserRoleCache _userRoleCache;
        public BaseUserRepository(IDbContextCore dbContext, 
            IBaseUserRoleMapRepository baseUserRoleMapRepository,
            IBasePermissionUserRepository basePermissionUserRepository, 
            IBasePermissionRoleRepository basePermissionRoleRepository) : base(dbContext)
        {
            _baseUserRoleMapRepository = baseUserRoleMapRepository;
            _basePermissionRoleRepository = basePermissionRoleRepository;
            _basePermissionUserRepository = basePermissionUserRepository;
            _userRoleCache = new UserRoleCache(baseUserRoleMapRepository);
            _cache = new Base_UserModelCache(this);
        }

        public List<Base_UserModel> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var whereExpre = LinqHelper.True<Base_UserModel>();
            Expression<Func<Base_User, Base_UserModel>> selectExpre = user => new Base_UserModel { };


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

        public void UpdateData(Base_User theData)
        {
            if (theData.UserId == "Admin" && Operator.UserId != theData.UserId)
                throw new Exception("禁止更改超级管理员！");

            Update(theData);
        }

        public void SetUserRole(string userId, List<string> roleIds)
        {
            _baseUserRoleMapRepository.Delete(x => x.UserId == userId);
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

        public void DeleteData(List<string> ids)
        {
            var userIds = Get().Where(x => ids.Contains(x.Id)).Select(x => x.UserId).ToList();
            var adminUser = GetTheUser("Admin");
            if (ids.Contains(adminUser.Id))
                throw new Exception("超级管理员是内置账号,禁止删除！");

            ids.ForEach(x => Delete(x));
            _cache.UpdateCache(ids);
            _userRoleCache.UpdateCache(ids);
        }
        /// <summary>
        /// 获取当前操作者信息
        /// </summary>
        /// <returns></returns>
        public Base_UserModel GetCurrentUser()
        {
            return GetTheUser(Operator.UserId);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public Base_UserModel GetTheUser(string userId)
        {
            return _cache.GetCache(userId);
        }

        public List<string> GetUserRoleIds(string userId)
        {
            return _userRoleCache.GetCache(userId);
        }
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

        public AjaxResult SubmitLogin(string userName, string password)
        {
            if (userName.IsNullOrEmpty() || password.IsNullOrEmpty())
                return Error("账号或密码不能为空！");
            password = password.ToMD5String();
            var theUser = Get().Where(x => x.UserName == userName && x.Password == password).FirstOrDefault();
            if (theUser != null)
            {
                Operator.Login(theUser.UserId);
                return Success();
            }
            else
                return Error("账号或密码不正确！");
        }

        public void SavePermission(string userId, List<string> permissions)
        {
            _basePermissionUserRepository.Delete(userId);
            var roleIdList = _baseUserRoleMapRepository.Get().Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
            var existsPermissions = _basePermissionRoleRepository.Get()
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

        #region 业务返回

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public AjaxResult Success()
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = "请求成功！",
                Data = null
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public AjaxResult Success(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = msg,
                Data = null
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public AjaxResult Success(object data)
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = "请求成功！",
                Data = data
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public AjaxResult Success(string msg, object data)
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = msg,
                Data = data
            };

            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        public AjaxResult Error()
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = "请求失败！",
                Data = null
            };

            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        public AjaxResult Error(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = msg,
                Data = null
            };

            return res;
        }

        #endregion
    }
}