using Autofac;

namespace Hk.Core.Util.Helper
{
    public class AutofacHelper
    {
        public static IContainer Container { get; set; }

        public static T GetService<T>()
        {
            //return (T)Container?.Resolve(typeof(T));
            return Ioc.DefaultContainer.GetService<T>();
            //return AspectCoreHelper.Resolve<T>();
        }
    }
}
