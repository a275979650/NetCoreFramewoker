using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Helper;
using System.Linq;

namespace Hk.Core.Business.Cache
{
    class Base_SysRoleCache : BaseCache<Base_SysRole>
    {
        public Base_SysRoleCache()
            : base("UserRoleCache", roleId =>
            {
                IBaseSysRoleRepository baseSysRoleRepository =
                    Ioc.DefaultContainer.GetService<IBaseSysRoleRepository>();
                return baseSysRoleRepository.Get().FirstOrDefault(x => x.RoleId == roleId);
            })
        {

        }
    }
}