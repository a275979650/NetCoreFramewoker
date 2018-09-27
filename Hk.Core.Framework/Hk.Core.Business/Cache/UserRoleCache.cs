using System.Collections.Generic;
using System.Linq;
using Hk.Core.DataRepository;
using Hk.Core.Entity.Base_SysManage;

namespace Hk.Core.Business.Cache
{
    class UserRoleCache : BaseCache<List<string>>
    {
        public UserRoleCache()
            : base("UserRoleCache", userId =>
            {
                var list = DbFactory.GetRepository()
                    .GetIQueryable<Base_UserRoleMap>()
                    .Where(x => x.UserId == userId)
                    .Select(x => x.RoleId)
                    .ToList();
                return list;
            })
        {

        }
    }
}