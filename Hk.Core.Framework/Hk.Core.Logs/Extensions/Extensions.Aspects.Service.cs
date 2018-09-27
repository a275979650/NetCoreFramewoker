using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Hk.Core.Logs.Aspects;
using Hk.Core.Util.Aspects.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Hk.Core.Logs.Extensions
{
    public static partial class Extensions
    {
        public static void AddInterceptor(this IServiceCollection services)
        {
            services.ConfigureDynamicProxy();
        }
    }
}