using System.Collections.Generic;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using System.Linq;
namespace Hk.Core.Repositorys
{
    public class BaseUnitTestRepository : BaseRepository<Base_UnitTest, string>, IBaseUnitTestRepository
    {
        public BaseUnitTestRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }

        public List<string> GetBaseUnitTestIdList()
        {
            return Get().Select(x => x.UserId).ToList();
        }
    }
}