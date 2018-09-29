using System.Collections.Generic;
using System.Linq;

namespace Hk.Core.Entity.Base_SysManage
{
    public class Base_UserModel : Base_User
    {
        public List<string> RoleIdList { get; set; }
        public List<string> RoleNameList { get; set; }
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