using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hk.Core.Util.Cache.Extensions
{
    /// <summary>
    /// 还未实现
    /// </summary>
    public static partial class Extention
    {
        public static void AddCache(this IServiceCollection services)
        {
            services.TryAddSingleton<ICache>();
        }
    }
}