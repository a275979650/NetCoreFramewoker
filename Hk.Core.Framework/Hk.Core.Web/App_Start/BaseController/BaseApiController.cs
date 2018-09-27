using Hk.Core.Util.Properties;
using Hk.Core.Web;
using HK.Core.Webs.Commons;
using Microsoft.AspNetCore.Mvc;

namespace Hk.Core.Controllers.Web
{
    /// <summary>
    /// Mvc对外接口基控制器
    /// </summary>
    [CheckSign]
    [CheckAppIdPermission]
    public class BaseApiController : BaseController
    {
    }
}