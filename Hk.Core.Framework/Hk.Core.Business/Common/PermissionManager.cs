using System;
using  System.Linq;
using Hk.Core.Entity;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Helper;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Hk.Core.Business.BaseBusiness;
using Hk.Core.Business.Base_SysManage;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util;
using Hk.Core.Util.Cache;

namespace Hk.Core.Business.Common
{
    public class PermissionManager
    {
        private string _cacheKey { get; } = "Permission";
        private string BuildCacheKey(string key)
        {
            return $"{GlobalSwitch.ProjectName}_{_cacheKey}_{key}";
        }
        private string _permissionConfigFile
        {
            get
            {
                string rootPath = AutofacHelper.GetService<IHostingEnvironment>().WebRootPath;
                return Path.Combine(rootPath, "Config", "Permission.config");
            }
        }
        private List<PermissionEntity> _allPermissionModules { get; set; }
        private List<string> _allPermissionValues { get; set; }
       //private IBaseUnitTestRepository _baseUnitTestRepository;
        private IPermissionRoleRepository _permissionRoleRepository;
        private IBasePermissionAppIdRepository _appIdRepository;
        private IBaseUserRepository _baseUserRepository;
        private IBaseUserRoleMapRepository _baseUserRoleMapRepository;
        private IBasePermissionUserRepository _basePermissionUserRepository;
        private IBasePermissionRoleRepository _basePermissionRoleRepository;
        public PermissionManager()
        {
            //_baseUnitTestRepository = Ioc.DefaultContainer.GetService<IBaseUnitTestRepository>();
            _permissionRoleRepository = Ioc.DefaultContainer.GetService<IPermissionRoleRepository>(); 
            _appIdRepository = Ioc.DefaultContainer.GetService<IBasePermissionAppIdRepository>();
            _baseUserRepository = Ioc.DefaultContainer.GetService<IBaseUserRepository>();
            _baseUserRoleMapRepository = Ioc.DefaultContainer.GetService<IBaseUserRoleMapRepository>();
            _basePermissionUserRepository = Ioc.DefaultContainer.GetService<IBasePermissionUserRepository>();
            _basePermissionRoleRepository = Ioc.DefaultContainer.GetService<IBasePermissionRoleRepository>();
            InitAllPermissionModules();
            InitAllPermissionValues();
        }

        #region 角色权限

        /// <summary>
        /// 获取角色权限模块
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<PermissionEntity> GetRolePermissionModules(string roleId)
        {
            var hasPermissions = _permissionRoleRepository.GetRolePermissionModules(roleId);

            return GetPermissionModules(hasPermissions);
        }

        #endregion
        #region AppId权限

        /// <summary>
        /// 获取AppId权限模块
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public List<PermissionEntity> GetAppIdPermissionModules(string appId)
        {
            var hasPermissions = GetAppIdPermissionValues(appId);

            return GetPermissionModules(hasPermissions);
        }

        /// <summary>
        /// 获取AppId权限值
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public List<string> GetAppIdPermissionValues(string appId)
        {
            string cacheKey = BuildCacheKey(appId);
            var permissions = CacheHelper.Cache.GetCache<List<string>>(cacheKey);
            if (permissions == null)
            {
               
                permissions = _appIdRepository.GetPermissionAppIdLists(appId);

                CacheHelper.Cache.SetCache(cacheKey, permissions);
            }

            return permissions.DeepClone();
        }

        /// <summary>
        /// 设置AppId权限
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="permissions">权限值列表</param>
        public void SetAppIdPermission(string appId, List<string> permissions)
        {
            //更新缓存
            string cacheKey = BuildCacheKey(appId);
            CacheHelper.Cache.SetCache(cacheKey, permissions);

            //更新数据库
            _appIdRepository.Delete(appId);
           

            List<Base_PermissionAppId> insertList = new List<Base_PermissionAppId>();
            permissions.ForEach(newPermission =>
            {
                insertList.Add(new Base_PermissionAppId
                {
                    //Id = Guid.NewGuid().ToSequentialGuid(),
                    AppId = appId,
                    PermissionValue = newPermission
                });
            });

            _appIdRepository.AddRange(insertList);
        }

        #endregion
        #region 用户权限

        /// <summary>
        /// 获取用户权限模块
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<PermissionEntity> GetUserPermissionModules(string userId)
        {
            var hasPermissions = GetUserPermissionValues(userId);

            return GetPermissionModules(hasPermissions);
        }

        /// <summary>
        /// 获取用户拥有的所有权限值
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public List<string> GetUserPermissionValues(string userId)
        {
            string cacheKey = BuildCacheKey(userId);
            var permissions = CacheHelper.Cache.GetCache<List<string>>(cacheKey)?.DeepClone();

            if (permissions == null)
            {
                UpdateUserPermissionCache(userId);
                permissions = CacheHelper.Cache.GetCache<List<string>>(cacheKey)?.DeepClone();
            }

            return permissions;
        }

        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="permissions">权限值列表</param>
        public void SetUserPermission(string userId, List<string> permissions)
        {
            //更新数据库
            _basePermissionUserRepository.Delete(userId);
            var roleIdList = _baseUserRoleMapRepository.GetBaseUserRoleMapList(userId);
            var existsPermissions = _permissionRoleRepository.Get()
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

            //更新缓存
            UpdateUserPermissionCache(userId);
        }

        /// <summary>
        /// 清楚所有用户权限缓存
        /// </summary>
        public void ClearUserPermissionCache()
        {          
            //var userIdList = _baseUnitTestRepository.GetBaseUnitTestIdList();
            var userIdList = _baseUserRepository.Get().Select(x => x.UserId).ToList();
            userIdList.ForEach(aUserId =>
            {
                CacheHelper.Cache.RemoveCache(BuildCacheKey(aUserId));
            });
        }

        /// <summary>
        /// 更新用户权限缓存
        /// </summary>
        /// <param name="userId"><用户Id/param>
        public void UpdateUserPermissionCache(string userId)
        {
            string cacheKey = BuildCacheKey(userId);
            List<string> permissions = new List<string>();

           
            var userPermissions = _basePermissionUserRepository.Get().Where(x => x.UserId == userId).Select(x => x.PermissionValue).ToList();
            var theUser = _baseUserRepository.Get().Where(x => x.UserId == userId).FirstOrDefault();
            var roleIdList = Base_UserBusiness.GetUserRoleIds(userId);
            var rolePermissions = _basePermissionRoleRepository.Get().Where(x => roleIdList.Contains(x.RoleId)).GroupBy(x => x.PermissionValue).Select(x => x.Key).ToList();
            var existsPermissions = userPermissions.Concat(rolePermissions).Distinct();

            permissions = existsPermissions.ToList();
            CacheHelper.Cache.SetCache(cacheKey, permissions);
        }

        #endregion
        #region 当前操作用户权限

        /// <summary>
        /// 获取当前操作者拥有的所有权限值
        /// </summary>
        /// <returns></returns>
        public List<string> GetOperatorPermissionValues()
        {
            if (Operator.IsAdmin())
                return GetAllPermissionValues();
            else
                return GetUserPermissionValues(Operator.UserId);
        }

        /// <summary>
        /// 判断当前操作者是否拥有某项权限值
        /// </summary>
        /// <param name="value">权限值</param>
        /// <returns></returns>
        public bool OperatorHasPermissionValue(string value)
        {
            return GetOperatorPermissionValues().Select(x => x.ToLower()).Contains(value.ToLower());
        }

        #endregion
        #region 所有权限

        /// <summary>
        /// 获取所有权限模块
        /// </summary>
        /// <returns></returns>
        public List<PermissionEntity> GetAllPermissionModules()
        {
            return _allPermissionModules.DeepClone();
        }

        /// <summary>
        /// 获取所有权限值
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllPermissionValues()
        {
            return _allPermissionValues.DeepClone();
        }

        #endregion
        #region 私有方法
        private void InitAllPermissionModules()
        {
            List<PermissionEntity> resList = new List<PermissionEntity>();
            string filePath = _permissionConfigFile;
            XElement xe = XElement.Load(filePath);
            xe.Elements("module")?.ForEach(aModule =>
            {
                PermissionEntity newModule = new PermissionEntity();
                resList.Add(newModule);

                newModule.Name = aModule.Attribute("name")?.Value;
                newModule.Value = aModule.Attribute("value")?.Value;
                newModule.Items = new List<PermissionItemEntity>();
                aModule?.Elements("permission")?.ForEach(aItem =>
                {
                    PermissionItemEntity newItem = new PermissionItemEntity();
                    newModule.Items.Add(newItem);

                    newItem.Name = aItem?.Attribute("name")?.Value;
                    newItem.Value = aItem?.Attribute("value")?.Value;
                });
            });

            _allPermissionModules =  resList;
        }
        private void InitAllPermissionValues()
        {
            List<string> resList = new List<string>();

            GetAllPermissionModules()?.ForEach(aModule =>
            {
                aModule.Items?.ForEach(aItem =>
                {
                    resList.Add($"{aModule.Value}.{aItem.Value}");
                });
            });

            _allPermissionValues = resList;
        }
        private List<PermissionEntity> GetPermissionModules(List<string> hasPermissions)
        {
            var permissionModules = GetAllPermissionModules();
            permissionModules.ForEach(aModule =>
            {
                aModule.Items?.ForEach(aItem =>
                {
                    aItem.IsChecked = hasPermissions.Contains($"{aModule.Value}.{aItem.Value}");
                });
            });

            return permissionModules;
        }



        #endregion
    }
}