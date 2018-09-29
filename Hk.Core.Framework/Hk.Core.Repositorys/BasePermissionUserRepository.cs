using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;

namespace Hk.Core.Repositorys
{
    public class BasePermissionUserRepository:BaseRepository<Base_PermissionUser,string>,IBasePermissionUserRepository
    {
        public BasePermissionUserRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }

        public List<string> GetBasePermissionUserLists(string userId)
        {
            return  Get().Where(x => x.UserId == userId).Select(x => x.PermissionValue).ToList();
        }
    }
}