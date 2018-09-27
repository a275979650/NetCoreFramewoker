using Hk.Core.Util.Helper;

namespace Hk.Core.Util.Contexts
{
    /// <summary>
    /// Web上下文
    /// </summary>
    public class WebContext : IContext
    {
        /// <summary>
        /// 跟踪号
        /// </summary>
        public string TraceId => WebHelper.HttpContext?.TraceIdentifier;

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键名</param>
        /// <param name="value">对象</param>
        public void Add<T>(string key, T value)
        {
            if (WebHelper.HttpContext == null)
                return;
            WebHelper.HttpContext.Items[key] = value;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键名</param>
        public T Get<T>(string key)
        {
            if (WebHelper.HttpContext == null)
                return default(T);
            return ConvertHelper.To<T>(WebHelper.HttpContext.Items[key]);
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="key">键名</param>
        public void Remove(string key)
        {
            WebHelper.HttpContext?.Items.Remove(key);
        }
    }
}