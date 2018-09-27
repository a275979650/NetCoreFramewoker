using Hk.Core.Util.Logs;

namespace Hk.Core.Logs.Aspects
{
    /// <summary>
    /// 调试日志
    /// </summary>
    public class DebugInterceptorAttribute : LogInterceptorAttributeBase
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        protected override bool Enabled(ILog log)
        {
            return log.IsDebugEnabled;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        protected override void WriteLog(ILog log)
        {
            log.Debug();
        }
    }
}