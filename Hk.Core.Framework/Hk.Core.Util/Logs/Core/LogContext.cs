using System;
using System.Diagnostics;
using Hk.Core.Util.Contexts;
using Hk.Core.Util.Helper;
using Hk.Core.Util.Logs.Abstractions;
using Hk.Core.Util.Logs.Internal;

namespace Hk.Core.Util.Logs.Core
{
    /// <summary>
    /// 日志上下文
    /// </summary>
    public class LogContext : ILogContext
    {
        /// <summary>
        /// 日志上下文信息
        /// </summary>
        private LogContextInfo _info;

        /// <summary>
        /// 初始化日志上下文
        /// </summary>
        /// <param name="context">上下文</param>
        public LogContext(IContext context)
        {
            Context = context;
        }

        /// <summary>
        /// 上下文
        /// </summary>
        public IContext Context { get; set; }

        /// <summary>
        /// 跟踪号
        /// </summary>
        public string TraceId => GetInfo().TraceId;

        /// <summary>
        /// 计时器
        /// </summary>
        public Stopwatch Stopwatch => GetInfo().Stopwatch;

        /// <summary>
        /// IP
        /// </summary>
        public string Ip => GetInfo().Ip;
        /// <summary>
        /// 主机
        /// </summary>
        public string Host => GetInfo().Host;
        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser => GetInfo().Browser;
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url => GetInfo().Url;

        /// <summary>
        /// 获取日志上下文信息
        /// </summary>
        private LogContextInfo GetInfo()
        {
            if (_info != null)
                return _info;
            var key = "Util.Logs.LogContext";
            _info = Context.Get<LogContextInfo>(key);
            if (_info != null)
                return _info;
            _info = CreateInfo();
            Context.Add(key, _info);
            return _info;
        }

        /// <summary>
        /// 创建日志上下文信息
        /// </summary>
        protected virtual LogContextInfo CreateInfo()
        {
            return new LogContextInfo
            {
                TraceId = GetTraceId(),
                Stopwatch = GetStopwatch(),
                Ip = WebHelper.Ip,
                Host = WebHelper.Host,
                Browser = WebHelper.Browser,
                Url = WebHelper.Url
            };
        }

        /// <summary>
        /// 获取跟踪号
        /// </summary>
        protected string GetTraceId()
        {
            var traceId = Context.TraceId;
            return string.IsNullOrWhiteSpace(traceId) ? Guid.NewGuid().ToString() : traceId;
        }

        /// <summary>
        /// 获取计时器
        /// </summary>
        protected Stopwatch GetStopwatch()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }
    }
}