using System.Collections.Generic;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Dependency;

namespace Hk.Core.IRepositorys
{
    public interface IBasePermissionAppIdRepository:IRepository<Base_PermissionAppId,string>,IScopeDependency
    {
        List<string> GetPermissionAppIdLists(string appId);
    }
}