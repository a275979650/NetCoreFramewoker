using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Dependency;

namespace Hk.Core.IRepositorys
{
    public interface IBasePermissionRoleRepository:IRepository<Base_PermissionRole,string>,IScopeDependency
    {
        
    }
}