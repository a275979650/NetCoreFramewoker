using Hk.Core.Util.Logs;

namespace Hk.Core.Logs.Aspects
{
    /// <summary>
    /// 跟踪日志
    /// </summary>
    public class TraceLogInterceptorAttribute : LogInterceptorAttributeBase
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        protected override bool Enabled(ILog log)
        {
            return log.IsTraceEnabled;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        protected override void WriteLog(ILog log)
        {
            log.Trace();
        }
    }
}