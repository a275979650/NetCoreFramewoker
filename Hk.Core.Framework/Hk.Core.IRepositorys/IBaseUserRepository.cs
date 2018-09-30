using System.Collections.Generic;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Dependency;
using Hk.Core.Util.Model;

namespace Hk.Core.IRepositorys
{
    public interface IBaseUserRepository:IRepository<Base_User,string>,IScopeDependency
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