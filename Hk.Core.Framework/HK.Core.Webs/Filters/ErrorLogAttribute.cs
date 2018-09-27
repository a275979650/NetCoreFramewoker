using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Hk.Core.Logs;
using Hk.Core.Logs.Extensions;

namespace HK.Core.Webs.Filters
{
    /// <summary>
    /// 错误日志过滤器
    /// </summary>
    public class ErrorLogAttribute : ExceptionFilterAttribute
    {
        private readonly string _caption;
        public ErrorLogAttribute(string caption = "WebApi全局异常")
        {
            _caption = caption;
        }
        /// <summary>
        /// 异常处理
        /// </summary>
        public override void OnException(ExceptionContext context)
        {
            WriteLog(context);
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            WriteLog(context);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
                return;
            var log = Log.GetLog(context).Caption(_caption);
            context.Exception.Log(log);
        }
    }
}