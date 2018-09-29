using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Dependency;

namespace Hk.Core.IRepositorys
{
    public interface IBaseSysRoleRepository:IRepository<Base_SysRole,string>,IScopeDependency
    {
        
    }
}