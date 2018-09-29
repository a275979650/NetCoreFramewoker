using System.Collections.Generic;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Dependency;

namespace Hk.Core.IRepositorys
{
    public interface IBaseUserRoleMapRepository:IRepository<Base_UserRoleMap,string>,IScopeDependency
    {
        List<string> GetBaseUserRoleMapList(string userId);
    }
}