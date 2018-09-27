using Hk.Core.Util.Extentions;
using HK.Core.Webs.Commons;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HK.Core.Webs.Filters
{
    /// <summary>
    /// 异常处理过滤器
    /// </summary>
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;
            context.Result = new Result(StateCode.Fail, context.Exception.GetPrompt());
        }
    }
}