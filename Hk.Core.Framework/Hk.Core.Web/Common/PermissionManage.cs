using Hk.Core.Business.Common;
using Hk.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hk.Core.Web.Common
{
    /// <summary>
    /// 权限管理静态类
    /// </summary>
    public static class PermissionManage
    {
        private static PermissionManager _permissionManager = new PermissionManager();
        #region 所有权限
        /// <summary>
        /// 获取所有权限模块
        /// </summary>
        /// <returns></returns>
        public static List<PermissionEntity> GetAllPermissionModules()
        {
            return _permissionManager.GetAllPermissionModules();
        }

        /// <summary>
        /// 获取所有权限值
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllPermissionValues()
        {
            return _permissionManager.GetAllPermissionValues();
        }

        #endregion

        #region 角色权限

        /// <summary>
        /// 获取角色权限模块
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static List<PermissionEntity> GetRolePermissionModules(string roleId)
        {
            return _permissionManager.GetRolePermissionModules(roleId);
        }

        #endregion

        #region AppId权限

        /// <summary>
        /// 获取AppId权限模块
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static List<PermissionEntity> GetAppIdPermissionModules(string appId)
        {
            return _permissionManager.GetAppIdPermissionModules(appId);
        }

        /// <summary>
        /// 获取AppId权限值
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static List<string> GetAppIdPermissionValues(string appId)
        {
            return _permissionManager.GetAppIdPermissionValues(appId);
        }

        /// <summary>
        /// 设置AppId权限
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="permissions">权限值列表</param>
        public static void SetAppIdPermission(string appId, List<string> permissions)
        {
            _permissionManager.SetAppIdPermission(appId,permissions);
        }

        #endregion

        #region 用户权限

        /// <summary>
        /// 获取用户权限模块
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<PermissionEntity> GetUserPermissionModules(string userId)
        {
            return _permissionManager.GetUserPermissionModules(userId);
        }

        /// <summary>
        /// 获取用户拥有的所有权限值
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static List<string> GetUserPermissionValues(string userId)
        {
            return _permissionManager.GetUserPermissionValues(userId);
        }

        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="permissions">权限值列表</param>
        public static void SetUserPermission(string userId, List<string> permissions)
        {
            _permissionManager.SetUserPermission(userId,permissions);
        }

        /// <summary>
        /// 清楚所有用户权限缓存
        /// </summary>
        public static void ClearUserPermissionCache()
        {
            _permissionManager.ClearUserPermissionCache();
        }

        /// <summary>
        /// 更新用户权限缓存
        /// </summary>
        /// <param name="userId"><用户Id/param>
        public static void UpdateUserPermissionCache(string userId)
        {
            _permissionManager.UpdateUserPermissionCache(userId);
        }

        #endregion

        #region 当前操作用户权限

        /// <summary>
        /// 获取当前操作者拥有的所有权限值
        /// </summary>
        /// <returns></returns>
        public static List<string> GetOperatorPermissionValues()
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
        public static bool OperatorHasPermissionValue(string value)
        {
            return GetOperatorPermissionValues().Select(x => x.ToLower()).Contains(value.ToLower());
        }

        #endregion
    }
}