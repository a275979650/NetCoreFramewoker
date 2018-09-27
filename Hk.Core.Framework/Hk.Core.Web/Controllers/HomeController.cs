using Hk.Core.Business.Base_SysManage;
using Hk.Core.Business.Common;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Model;
using Microsoft.AspNetCore.Mvc;

namespace Hk.Core.Web
{
    public class HomeController : BaseMvcController
    {
        public HomeController(IHomebusiness homebus)
        {
            _homeBus = homebus;
        }

        private IHomebusiness _homeBus { get; }

        #region 视图功能

        public IActionResult Index()
        {
            return View();
        }

        [IgnoreLogin]
        public IActionResult Login()
        {
            if (Operator.Logged())
            {

                string loginUrl = Url.Content("~/");
                string script = $@"    
<html>
    <script>
        top.location.href = '{loginUrl}';
    </script>
</html>
";
                return Content(script, "text/html");
            }

            return View();
        }

        public IActionResult Desktop()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }

        #endregion

        #region 获取数据

        #endregion

        #region 提交数据

        [IgnoreLogin]
        public IActionResult SubmitLogin(string userName, string password)
        {
            AjaxResult res = _homeBus.SubmitLogin(userName, password);

            return Content(res.ToJson());
        }

        /// <summary>
        /// 注销
        /// </summary>
        public IActionResult Logout()
        {
            Operator.Logout();

            return Success("注销成功！");
        }

        #endregion
    }
}