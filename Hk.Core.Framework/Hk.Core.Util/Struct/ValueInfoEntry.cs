using System;
using Hk.Core.Util.Enum;

namespace Hk.Core.Util.Struct
{
    /// <summary>
    /// 值信息
    /// </summary>
    public struct ValueInfoEntry
    {
        public string Value { get; set; }
        public string TypeName { get; set; }
        public TimeSpan? ExpireTime { get; set; }
        public ExpireType? ExpireType { get; set; }
    }
}