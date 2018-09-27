using Hk.Core.Util.Dependency;
using Hk.Core.Util.Model;

namespace Hk.Core.Business.Base_SysManage
{
    public interface IHomebusiness
    {
        AjaxResult SubmitLogin(string userName, string password);
    }
}