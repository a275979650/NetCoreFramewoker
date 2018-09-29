using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hk.Core.Data.Models;

namespace Hk.Core.Entity.Base_SysManage
{
    /// <summary>
    /// 应用密钥
    /// </summary>
    [Table("Base_AppSecret")]
    public class Base_AppSecret:BaseModel<string>
    {

        /// <summary>
        /// 代理主键
        /// </summary>
        [Key]
        public override string Id { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public String AppId { get; set; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        public String AppSecret { get; set; }

        /// <summary>
        /// 应用名
        /// </summary>
        public String AppName { get; set; }
    }
}