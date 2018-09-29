using System;
using System.Collections.Generic;
using  Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Dependency;

namespace Hk.Core.IRepositorys
{
    public interface IPermissionRoleRepository: IRepository<Base_PermissionRole,string>,IScopeDependency
    {
        List<string> GetRolePermissionModules(string roleId);
    }
}
