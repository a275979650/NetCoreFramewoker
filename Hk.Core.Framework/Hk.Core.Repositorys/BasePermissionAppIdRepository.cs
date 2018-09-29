using System.Collections.Generic;
using System.Linq;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;

namespace Hk.Core.Repositorys
{
    public class BasePermissionAppIdRepository:BaseRepository<Base_PermissionAppId,string>,IBasePermissionAppIdRepository
    {
        public BasePermissionAppIdRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }

        public List<string> GetPermissionAppIdLists(string appId)
        {
            return Get().Where(x => x.AppId == appId).Select(x => x.PermissionValue).ToList();
        }
    }
}