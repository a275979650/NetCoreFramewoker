using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hk.Core.Data.Models;

namespace Hk.Core.Entity.Base_SysManage
{
    /// <summary>
    /// Base_UserRoleMap
    /// </summary>
    [Table("Base_UserRoleMap")]
    public class Base_UserRoleMap:BaseModel<string>
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public override string Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// RoleId
        /// </summary>
        public String RoleId { get; set; }

    }
}