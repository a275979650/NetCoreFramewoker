using Hk.Core.Util.Enum;
using Hk.Core.Util.Helper;
using Microsoft.AspNetCore.Hosting;

namespace Hk.Core.Util
{
    /// <summary>
    /// 全局控制
    /// </summary>
    public class GlobalSwitch
    {
        #region 构造函数

        static GlobalSwitch()
        {
            //#if !DEBUG
            //            RunModel = RunModel.Publish;
            //#endif
        }

        #endregion

        #region 参数

        /// <summary>
        /// 项目名
        /// </summary>
        public static string ProjectName { get; } = "Hk.Core.Framework";

        #endregion

        #region 运行

        /// <summary>
        /// 运行模式
        /// </summary>
        public static RunModel RunModel { get; } = RunModel.Publish;

        /// <summary>
        /// 网站文件根路径
        /// </summary>
        public static string WebRootPath { get => AutofacHelper.GetService<IHostingEnvironment>().WebRootPath; }

        #endregion

        #region 数据库

        /// <summary>
        /// 默认数据库类型
        /// </summary>
        public static DatabaseType DatabaseType { get; } = DatabaseType.SqlServer;

        /// <summary>
        /// 默认数据库连接名
        /// </summary>
        public static string DefaultDbConName { get; } = "BaseDb";

        #endregion

        #region 缓存

        /// <summary>
        /// 默认缓存
        /// </summary>
        public static CacheType CacheType { get; } = CacheType.SystemCache;

        /// <summary>
        /// Redis配置字符串
        /// </summary>
        public static string RedisConfig { get; } = null /*"localhost:6379,password=123456"*/;

        #endregion
    }
}
