using Hk.Core.Util.Extentions;
using System;

namespace Hk.Core.DataRepository.Models
{
    /// <summary>
    /// 所有数据表实体类都必须实现此接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    [Serializable]
    public abstract class BaseModel<TKey> 
    {
        public abstract TKey Id { get; set; }

        public virtual string UniqueId { get; set; } = Guid.NewGuid().ToSequentialGuid();
    }
}