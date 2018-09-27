using Hk.Core.Business.Base_SysManage;
using Hk.Core.DataRepository;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Logs;
using Hk.Core.Logs.Extensions;
using Hk.Core.Util.Dependency;
using Hk.Core.Util.Events.Default;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Filter;
using Hk.Core.Util.Logs.Extensions;
using Hk.Core.Web.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hk.Core.Web
{
    public class Startup
    {
        /// <summary>
        /// 初始化启动配置
        /// </summary>
        /// <param name="configuration">配置</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 配置
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 配置服务
        /// </summary>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Log.GetLog("程序开始").Content("start").Info();

            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);      
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();            
            services.AddSingleton(Configuration);
            services.AddEventBus();
            services.AddNLog();
            return services.AddUtil(new HomeBusinessConfig());
        }

        /// <summary>
        /// 配置请求管道
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                DevelopmentConfig(app);
                return;
            }

            ProductionConfig(app);


        }

        private void InitEF()
        {
            Task.Run(() =>
            {
                try
                {
                    var a = DbFactory.GetRepository(null, null, null).GetIQueryable<Base_User>().ToList();
                }
                catch(Exception ex)
                {
                    var log = Log.GetLog().Caption("EF初始化异常");
                    ex.Log(log);
                }
            });
        }
        /// <summary>
        /// 开发环境配置
        /// </summary>
        /// <param name="app"></param>
        private void DevelopmentConfig(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            CommonConfig(app);
        }
        /// <summary>
        /// 公共配置
        /// </summary>
        /// <param name="app"></param>
        private void CommonConfig(IApplicationBuilder app)
        {
            //app.UseErrorLog();
            app.UseStaticFiles();//支持静态文件
            ConfigRoute(app);
            InitEF();
        }
        /// <summary>
        /// 路由配置,支持区域
        /// </summary>
        private void ConfigRoute(IApplicationBuilder app)
        {
            app.UseMvc(routes => { //默认路由
                routes.MapRoute(
                    name: "Default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );

                //区域
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
        /// <summary>
        /// 生产环境配置
        /// </summary>
        private void ProductionConfig(IApplicationBuilder app)
        {
            app.UseExceptionHandler("/Home/Error");
            CommonConfig(app);

        }
    }
}
