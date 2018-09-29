using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;

namespace Hk.Core.Repositorys
{
    public class BaseSysRoleRepository:BaseRepository<Base_SysRole,string>, IBaseSysRoleRepository
    {
        public BaseSysRoleRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }
    }
}