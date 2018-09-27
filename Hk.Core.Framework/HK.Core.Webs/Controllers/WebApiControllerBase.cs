using System;
using Microsoft.AspNetCore.Mvc;
using Hk.Core.Util.Properties;
using HK.Core.Webs.Commons;
using HK.Core.Webs.Filters;
using Microsoft.AspNetCore.Cors;

namespace HK.Core.Webs.Controllers
{
    /// <summary>
    /// WebApi控制器
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("any")]
    [ErrorLog]
    public class WebApiControllerBase : Controller
    {
        /// <summary>
        /// 会话
        /// </summary>
        //public virtual ISession Session => Security.Sessions.Session.Instance;

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="message">消息</param>
        protected virtual IActionResult Success(dynamic data = null, string message = null)
        {
            if (message == null)
                message = R.Success;
            return new Result(StateCode.Ok, message, data);
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="message">消息</param>
        protected IActionResult Fail(string message)
        {
            return new Result(StateCode.Fail, message);
        }
    }
}
