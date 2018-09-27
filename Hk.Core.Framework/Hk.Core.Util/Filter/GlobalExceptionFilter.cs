using System;
using System.Collections.Generic;
using System.Text;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Helper;
using Hk.Core.Util.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Hk.Core.Util.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            string msg = ExceptionHelper.GetExceptionAllMsg(ex);

            context.Result = new ContentResult { Content = new AjaxResult { Success = false, Msg = msg }.ToJson() };
        }
    }
}
