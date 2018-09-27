using System.Linq;
using Hk.Core.DataRepository;
using Hk.Core.Entity.Base_SysManage;

namespace Hk.Core.Business.Cache
{
    class Base_SysRoleCache : BaseCache<Base_SysRole>
    {
        public Base_SysRoleCache()
            : base("UserRoleCache", roleId =>
            {
                return DbFactory.GetRepository().GetIQueryable<Base_SysRole>().Where(x => x.RoleId == roleId).FirstOrDefault();
            })
        {

        }
    }
}