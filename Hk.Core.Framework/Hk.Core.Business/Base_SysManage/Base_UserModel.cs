using System.Collections.Generic;
using System.Linq;
using Hk.Core.Entity.Base_SysManage;

namespace Hk.Core.Business.Base_SysManage
{
    public class Base_UserModel : Base_User
    {
        public List<string> RoleIdList { get => Base_UserBusiness.GetUserRoleIds(UserId); }
        public List<string> RoleNameList { get => RoleIdList.Select(x => Base_SysRoleBusiness.GetRoleName(x)).ToList(); }
        public string RoleNames { get => string.Join(",", RoleNameList); }
        public EnumType.RoleType RoleType
        {
            get
            {
                int type = 0;

                var values = typeof(EnumType.RoleType).GetEnumValues();
                foreach (var aValue in values)
                {
                    if (RoleNames.Contains(aValue.ToString()))
                        type += (int)aValue;
                }

                return (EnumType.RoleType)type;
            }
        }
    }
}