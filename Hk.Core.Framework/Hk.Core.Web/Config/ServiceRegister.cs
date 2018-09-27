using Hk.Core.Business.Base_SysManage;
using Hk.Core.Util.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Hk.Core.Web.Config
{
    /// <summary>
    /// 服务注册
    /// </summary>
    public class ServiceRegister : IDependencyRegistrar
    {
        /// <summary>
        /// 注册依赖
        /// </summary>
        public void Regist(IServiceCollection services)
        {
            #region 配置跨域处理
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin() //允许任何来源的主机访问
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();//指定处理cookie
                });
            });
            #endregion
        }
    }
}