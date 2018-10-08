using Hk.Core.Entity.Base_SysManage;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Hk.Core.Business.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Helper;

namespace Hk.Core.Business
{
    public class Base_UserModel : Base_User
    {
        //public List<string> RoleIdList { get => Base_UserBusiness.GetUserRoleIds(UserId); }
        //public List<string> RoleNameList { get => RoleIdList.Select(x => Base_SysRoleBusiness.GetRoleName(x)).ToList(); }
        //  public string RoleNames { get => string.Join(",", RoleNameList); }
        //public EnumType.RoleType RoleType
        //{
        //    get
        //    {
        //        int type = 0;

        //        var values = typeof(EnumType.RoleType).GetEnumValues();
        //        foreach (var aValue in values)
        //        {
        //            if (RoleNames.Contains(aValue.ToString()))
        //                type += (int)aValue;
        //        }

        //        return (EnumType.RoleType)type;
        //    }
        //}
        [NotMapped] public List<string> RoleIdList => Base_UserBusiness.GetUserRoleIds(UserId);

        [NotMapped]
        public List<string> RoleNameList
        {
            get
            {
                IBaseSysRoleRepository _baseSysRoleRepository =
                    Ioc.DefaultContainer.GetService<IBaseSysRoleRepository>();
                return RoleIdList.Select(x => _baseSysRoleRepository.GetRoleName(x)).ToList();
            }
        }

        [NotMapped] public string RoleNames => string.Join(",", RoleNameList);

        [NotMapped]
        public EnumType.RoleType RoleType
        {
            get
            {
                int type = 0;

                var values = typeof(EnumType.RoleType).GetEnumValues();
                foreach (var aValue in values)
                {
                    if (RoleNames.Contains(aValue.ToString()))
                        type += (int) aValue;
                }

                return (EnumType.RoleType) type;
            }
        }
    }
}