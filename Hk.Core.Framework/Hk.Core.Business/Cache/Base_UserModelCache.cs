using Hk.Core.Business.Base_SysManage;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using System.Linq;

namespace Hk.Core.Business.Cache
{
    public class Base_UserModelCache : BaseCache<Base_UserModel>
    {
        public Base_UserModelCache()
            : base("Base_UserModel", userId =>
            {
                if (userId.IsNullOrEmpty())
                    return null;
                return new Base_UserBusiness().GetDataList("UserId", userId, new Pagination()).FirstOrDefault();
            })
        {

        }
    }
}