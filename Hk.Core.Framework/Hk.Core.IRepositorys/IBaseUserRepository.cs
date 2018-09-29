using System.Collections.Generic;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Dependency;
using Hk.Core.Util.Model;

namespace Hk.Core.IRepositorys
{
    public interface IBaseUserRepository:IRepository<Base_User,string>,IScopeDependency
    {
    }
}