using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Dependency;
using System.Collections.Generic;

namespace Hk.Core.IRepositorys
{
    public interface IBaseSysRoleRepository:IRepository<Base_SysRole,string>,IScopeDependency
    {
        List<Base_SysRole> GetDataList(string condition, string keyword, Pagination pagination);
        Base_SysRole GetTheData(string id);
        void AddData(Base_SysRole newData);
        void UpdateData(Base_SysRole theData);
        void DeleteData(List<string> ids);
        string GetRoleName(string userId);
    }
}