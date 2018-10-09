using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.IRepositorys;

namespace Hk.Core.Repositorys
{
    public class TestRepository:BaseRepository, ITestRepository
    {
        public TestRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }
    }
}