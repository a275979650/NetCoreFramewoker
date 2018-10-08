using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hk.Core.Entity.Base_SysManage
{
    public class Base_UserModel
    {
        /// <summary>
        /// 代理主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户Id,逻辑主键
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public String RealName { get; set; }

        /// <summary>
        /// 性别(1为男，0为女)
        /// </summary>
        public Int32? Sex { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }
        //public List<string> RoleIdList { get => Base_UserBusiness.GetUserRoleIds(UserId); }
        //public List<string> RoleNameList { get => RoleIdList.Select(x => Base_SysRoleBusiness.GetRoleName(x)).ToList(); }
        public List<string> RoleIdList { set; get; }
        public List<string> RoleNameList { set; get; }
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