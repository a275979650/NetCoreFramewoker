using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hk.Core.Web
{
    /// <summary>
    /// 忽略校验用户接口权限
    /// </summary>
    public class IgnoreUrlPermissionAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}