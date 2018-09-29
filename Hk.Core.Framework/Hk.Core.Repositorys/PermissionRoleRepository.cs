using System;
using System.Collections.Generic;
using System.Linq;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;

namespace Hk.Core.Repositorys
{
    public class PermissionRoleRepository : BaseRepository<Base_PermissionRole, string>,IPermissionRoleRepository
    {
        public PermissionRoleRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }

        public List<string> GetRolePermissionModules(string roleId)
        {
            var hasPermissions = Get().Where(x => x.RoleId == roleId).Select(x => x.PermissionValue).ToList();

            return hasPermissions;
        }
    }
}
