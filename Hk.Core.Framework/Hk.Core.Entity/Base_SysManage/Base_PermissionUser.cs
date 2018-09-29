using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hk.Core.Data.Models;

namespace Hk.Core.Entity.Base_SysManage
{
    /// <summary>
    /// 用户权限表
    /// </summary>
    [Table("Base_PermissionUser")]
    public class Base_PermissionUser:BaseModel<string>
    {

        /// <summary>
        /// 代理主键
        /// </summary>
        [Key]
        public override string Id { get; set; }

        /// <summary>
        /// 用户主键Id
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public String PermissionValue { get; set; }

    }
}