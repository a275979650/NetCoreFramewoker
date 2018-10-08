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
        List<Base_UserModel> GetDataList(string condition, string keyword, Pagination pagination);
        Base_User GetTheData(string id);
        void AddData(Base_User newData);
        void UpdateData(Base_User theData);
        void SetUserRole(string userId, List<string> roleIds);
        void DeleteData(List<string> ids);
        Base_UserModel GetTheUser(string userId);
        List<string> GetUserRoleIds(string userId);
        void SavePermission(string userId, List<string> permissions);
        Base_UserModel GetCurrentUser();
        AjaxResult ChangePwd(string oldPwd, string newPwd);
        AjaxResult SubmitLogin(string userName, string password);
        AjaxResult Success();
        AjaxResult Success(string msg);
        AjaxResult Success(object data);
        AjaxResult Success(string msg, object data);
        AjaxResult Error();
        AjaxResult Error(string msg);
    }
}