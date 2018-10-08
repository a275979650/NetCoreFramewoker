using System.Collections.Generic;

namespace Hk.Core.Util.Cache.BaseCache
{
    public interface IBaseCache<T> where T : class
    {
        T GetCache(string idKey);
        void UpdateCache(string idKey);
        void UpdateCache(List<string> idKeys);
    }
}