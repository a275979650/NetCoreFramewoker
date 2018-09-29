using System.Collections.Generic;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Dependency;

namespace Hk.Core.IRepositorys
{
    public interface IBaseUnitTestRepository : IRepository<Base_UnitTest, string>, IScopeDependency
    {
        List<string> GetBaseUnitTestIdList();
    }
}