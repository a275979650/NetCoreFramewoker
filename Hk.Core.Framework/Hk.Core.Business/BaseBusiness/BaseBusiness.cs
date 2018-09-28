using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Models;
using Hk.Core.Data.Repositories;

namespace Hk.Core.Business.BaseBusiness
{
    public class BaseBusinessT<T,TKey>:BaseRepository<T,TKey> where T : BaseModel<TKey>
    {
        public BaseBusinessT(IDbContextCore dbContext) : base(dbContext)
        {
        }
    }
}