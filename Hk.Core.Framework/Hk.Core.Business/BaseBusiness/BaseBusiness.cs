using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Models;
using Hk.Core.Data.Repositories;

namespace Hk.Core.Business.BaseBusiness
{
    public class BaseBusiness<T,TKey>:BaseRepository<T,TKey> where T : BaseModel<TKey>
    {
        public BaseBusiness(IDbContextCore dbContext) : base(dbContext)
        {
        }
    }
}