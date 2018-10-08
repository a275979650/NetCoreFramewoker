using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Helper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Hk.Core.Business
{
    public class Base_UserModel : Base_User
    {
        [NotMapped] public List<string> RoleIdList
        {
            get
            {
                IBaseUserRepository _baseUserRepository = 
                    Ioc.DefaultContainer.GetService<IBaseUserRepository>();
                return _baseUserRepository.GetUserRoleIds(UserId);
            }
        } 

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