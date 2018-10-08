using Hk.Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hk.Core.Entity.Base_SysManage
{
    public class BaseUserModelConfiguration : EntityMappingConfiguration<Base_UserModel>
    {
        public override void Map(EntityTypeBuilder<Base_UserModel> b)
        {
            b.Ignore(p => p.RoleNameList);
            b.Ignore(p => p.RoleIdList);
            b.Ignore(p => p.RoleNames);
            b.Ignore(p => p.RoleType);
        }
    }
}