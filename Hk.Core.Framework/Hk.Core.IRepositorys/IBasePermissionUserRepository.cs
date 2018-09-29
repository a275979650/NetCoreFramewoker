using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Dependency;

namespace Hk.Core.IRepositorys
{
    public interface IBasePermissionUserRepository:IRepository<Base_PermissionUser,string>,IScopeDependency
    {
        List<string> GetBasePermissionUserLists(string userId);
    }
}