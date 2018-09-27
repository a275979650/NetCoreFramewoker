using Hk.Core.Logs.Aspects;
using Hk.Core.Util.Dependency;
using Hk.Core.Util.Validations.Aspects;

namespace Hk.Core.Web.Test
{
    public interface ICustomService:IScopeDependency
    {
        [ErrorLogInterceptor]
        void Call([Valid]string id);
    }
}