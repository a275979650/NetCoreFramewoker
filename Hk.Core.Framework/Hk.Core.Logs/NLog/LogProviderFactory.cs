using Hk.Core.Util.Logs.Abstractions;

namespace Hk.Core.Logs.NLog
{
    /// <summary>
    /// NLog日志提供程序工厂
    /// </summary>
    public class LogProviderFactory : ILogProviderFactory
    {
        /// <summary>
        /// 创建日志提供程序
        /// </summary>
        /// <param name="logName">日志名称</param>
        /// <param name="format">日志格式化器</param>
        public ILogProvider Create(string logName, ILogFormat format = null)
        {
            return new NLogProvider(logName, format);
        }
    }
}