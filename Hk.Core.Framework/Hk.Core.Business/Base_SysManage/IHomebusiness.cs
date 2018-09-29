using Hk.Core.Util.Dependency;
using Hk.Core.Util.Model;

namespace Hk.Core.Business.Base_SysManage
{
    public interface IHomebusiness
    {
        AjaxResult SubmitLogin(string userName, string password);
        AjaxResult Success();
        AjaxResult Success(string msg);
        AjaxResult Success(object data);
        AjaxResult Success(string msg, object data);
        AjaxResult Error();
        AjaxResult Error(string msg);
    }
}