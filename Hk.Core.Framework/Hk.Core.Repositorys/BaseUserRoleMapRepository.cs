using System.Collections.Generic;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using System.Linq;
namespace Hk.Core.Repositorys
{
    public class BaseUserRoleMapRepository:BaseRepository<Base_UserRoleMap,string>,IBaseUserRoleMapRepository
    {
        public BaseUserRoleMapRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }

        public List<string> GetBaseUserRoleMapList(string userId)
        {
            return  Get().Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
        }
    }
}