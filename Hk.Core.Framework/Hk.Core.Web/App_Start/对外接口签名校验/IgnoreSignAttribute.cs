using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hk.Core.Web
{
    /// <summary>
    /// 忽略接口签名校验
    /// </summary>
    public class IgnoreSignAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}