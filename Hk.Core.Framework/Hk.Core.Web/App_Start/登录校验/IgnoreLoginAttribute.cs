using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hk.Core.Web
{
    /// <summary>
    /// 忽略登录校验
    /// </summary>
    public class IgnoreLoginAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}