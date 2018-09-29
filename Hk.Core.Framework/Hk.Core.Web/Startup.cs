using Hk.Core.Business.Base_SysManage;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Logs;
using Hk.Core.Logs.Extensions;
using Hk.Core.Util.Dependency;
using Hk.Core.Util.Events.Default;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Filter;
using Hk.Core.Util.Logs.Extensions;
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
        /// ��ʼ����������
        /// </summary>
        /// <param name="configuration">����</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// ����
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ���÷���
        /// </summary>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Log.GetLog("����ʼ").Content("start").Info();
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);      
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton(Configuration);
            services.AddEventBus();
            services.AddNLog();
            services.AddOptions();
            return services.AddUtil(new HomeBusinessConfig());
        }

        /// <summary>
        /// ��������ܵ�
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
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="app"></param>
        private void DevelopmentConfig(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            CommonConfig(app);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="app"></param>
        private void CommonConfig(IApplicationBuilder app)
        {
            //app.UseErrorLog();
            app.UseStaticFiles();//֧�־�̬�ļ�
            ConfigRoute(app);
        }
        /// <summary>
        /// ·������,֧������
        /// </summary>
        private void ConfigRoute(IApplicationBuilder app)
        {
            app.UseMvc(routes => { //Ĭ��·��
                routes.MapRoute(
                    name: "Default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );

                //����
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
        /// <summary>
        /// ������������
        /// </summary>
        private void ProductionConfig(IApplicationBuilder app)
        {
            app.UseExceptionHandler("/Home/Error");
            CommonConfig(app);

        }
    }
}
