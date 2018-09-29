using System.Collections.Generic;

namespace Hk.Core.Entity
{
    public class PermissionEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<PermissionItemEntity> Items { get; set; }
    }
}