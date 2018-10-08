using Hk.Core.Business.Base_SysManage;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util;
using Hk.Core.Util.Enum;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Webs.WebApp;

namespace Hk.Core.Business.Common
{
    /// <summary>
    /// 操作者
    /// </summary>
    public static class Operator
    {
        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        public static string UserId
        {
            get
            {
                if (GlobalSwitch.RunModel == RunModel.LocalTest)
                    return "Admin";
                else
                    return SessionHelper.Session["UserId"]?.ToString();
            }
        }

        public static Base_UserModel Property { get => Base_UserBusiness.GetTheUser(UserId); }

        #region 操作方法

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        public static bool Logged()
        {
            return !UserId.IsNullOrEmpty();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userId">用户逻辑主键Id</param>
        public static void Login(string userId)
        {
            SessionHelper.Session["UserId"] = userId;
        }

        /// <summary>
        /// 注销
        /// </summary>
        public static void Logout()
        {
            SessionHelper.Session["UserId"] = null;
        }

        /// <summary>
        /// 判断是否为超级管理员
        /// </summary>
        /// <returns></returns>
        public static bool IsAdmin()
        {
            return UserId == "Admin";
        }

        #endregion
    }
}