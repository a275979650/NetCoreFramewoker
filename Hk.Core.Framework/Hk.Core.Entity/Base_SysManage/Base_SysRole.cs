﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hk.Core.Data.Models;

namespace Hk.Core.Entity.Base_SysManage
{
    /// <summary>
    /// 系统角色
    /// </summary>
    [Table("Base_SysRole")]
    public class Base_SysRole:BaseModel<string>
    {

        /// <summary>
        /// 代理主键
        /// </summary>
        [Key]
        public override string Id { get; set; }

        /// <summary>
        /// 逻辑主键，角色Id
        /// </summary>
        public String RoleId { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public String RoleName { get; set; }

    }
}