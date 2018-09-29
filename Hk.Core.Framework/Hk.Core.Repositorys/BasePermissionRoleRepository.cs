using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;

namespace Hk.Core.Repositorys
{
    public class BasePermissionRoleRepository:BaseRepository<Base_PermissionRole,string>, IBasePermissionRoleRepository
    {
        public BasePermissionRoleRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }
    }
}