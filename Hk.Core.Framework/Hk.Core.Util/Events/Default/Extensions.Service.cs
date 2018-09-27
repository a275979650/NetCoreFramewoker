﻿using Hk.Core.Util.Events.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hk.Core.Util.Events.Default
{
    /// <summary>
    /// 事件总线扩展
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 注册事件总线服务
        /// </summary>
        /// <param name="services">服务集合</param>
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.TryAddSingleton<IEventHandlerManager, EventHandlerManager>();
            services.TryAddSingleton<IEventBus, EventBus>();
            return services;
        }
    }
}