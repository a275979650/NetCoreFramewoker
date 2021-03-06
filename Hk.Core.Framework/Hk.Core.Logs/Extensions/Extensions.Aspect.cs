﻿using System.Collections.Generic;
using System.Linq;
using AspectCore.DynamicProxy.Parameters;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Helper;
using Hk.Core.Util.Logs;

namespace Hk.Core.Logs.Extensions
{
    /// <summary>
    /// AOP扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 添加日志参数
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="log">参数</param>
        public static void AppendTo(this Parameter parameter, ILog log)
        {
            log.Params(parameter.ParameterInfo.ParameterType.FullName, parameter.Name, GetParameterValue(parameter));
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        private static string GetParameterValue(Parameter parameter)
        {
            if (Reflection.IsGenericCollection(parameter.RawType) == false)
                return parameter.Value.SafeString();
            if (!(parameter.Value is IEnumerable<object> list))
                return parameter.Value.SafeString();
            return list.Select(t => t.SafeString()).Join();
        }
    }
}