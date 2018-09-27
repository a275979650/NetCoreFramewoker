using Autofac;
using Hk.Core.Util.Dependency;

namespace Hk.Core.Business.Base_SysManage
{
    public class HomeBusinessConfig:ConfigBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddSingleton<IHomebusiness, HomeBusiness>();
        }
    }
}