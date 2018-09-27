using Microsoft.AspNetCore.Mvc;

namespace Hk.Core.Web
{
    /// <summary>
    /// Mvc基控制器
    /// </summary>
    [CheckLogin]
    [CheckUrlPermission]
    public class BaseMvcController : BaseController
    {

    }
}