using System;
using System.Collections.Generic;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Extentions;

namespace Hk.Core.Repositorys
{
    public class BasePermissionRoleRepository:BaseRepository<Base_PermissionRole,string>, IBasePermissionRoleRepository
    {
        public BasePermissionRoleRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }

        public void SavePermission(string roleId, List<string> permissions)
        {
            Delete(x => x.RoleId == roleId);
            List<Base_PermissionRole> insertList = new List<Base_PermissionRole>();
            permissions.ForEach(newPermission =>
            {
                insertList.Add(new Base_PermissionRole
                {
                    Id = Guid.NewGuid().ToSequentialGuid(),
                    RoleId = roleId,
                    PermissionValue = newPermission
                });
            });

            AddRange(insertList);
        }
    }
}