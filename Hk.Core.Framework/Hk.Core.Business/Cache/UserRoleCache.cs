using Hk.Core.IRepositorys;
using Hk.Core.Util.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Hk.Core.Business.Cache
{
    public class UserRoleCache : BaseCache<List<string>>
    {
        public UserRoleCache()
            : base("UserRoleCache", userId =>
            {
                IBaseUserRoleMapRepository baseUserRoleMapRepository =
                    Ioc.DefaultContainer.GetService<IBaseUserRoleMapRepository>();
                var list = baseUserRoleMapRepository.Get()
                    .Where(x => x.UserId == userId)
                    .Select(x => x.RoleId)
                    .ToList();
                return list;
            })
        {

        }
    }
}