using Hk.Core.Data.DbContextCore;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using System.Linq;


namespace Hk.Core.Business.Cache
{
    public class Base_UserModelCache : BaseCache<Base_UserModel>
    {
        public Base_UserModelCache(IBaseUserRepository baseUserRepository)
            : base("Base_UserModel", userId =>
            {
                if (userId.IsNullOrEmpty())
                    return null;
                return baseUserRepository.GetDataList("UserId", userId, new Pagination()).FirstOrDefault();
            })
        {

        }
    }
}