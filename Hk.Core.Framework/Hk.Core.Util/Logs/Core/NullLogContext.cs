﻿using System.Diagnostics;
using Hk.Core.Util.Logs.Abstractions;

namespace Hk.Core.Util.Logs.Core
{
    /// <summary>
    /// 空日志上下文
    /// </summary>
    public class NullLogContext : ILogContext
    {
        /// <summary>
        /// 空日志上下文实例
        /// </summary>
        public static readonly ILogContext Instance = new NullLogContext();
        /// <summary>
        /// 跟踪号
        /// </summary>
        public string TraceId => string.Empty;
        /// <summary>
        /// 计时器
        /// </summary>
        public Stopwatch Stopwatch => new Stopwatch();
        /// <summary>
        /// IP
        /// </summary>
        public string Ip => string.Empty;
        /// <summary>
        /// 主机
        /// </summary>
        public string Host => string.Empty;
        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser => string.Empty;
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url => string.Empty;
    }
}