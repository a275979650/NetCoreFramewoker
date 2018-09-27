﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Hk.Core.Util;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Enum;
using Hk.Core.Util.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hk.Core.DataRepository
{
    public class BaseDbContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nameOrConStr">数据库连接名或连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="entityNamespace">数据库实体命名空间</param>
        public BaseDbContext(string nameOrConStr, DatabaseType dbType, string entityNamespace)
        {
            _nameOrConStr = nameOrConStr;
            _dbType = dbType;
            _entityNamespace = entityNamespace.IsNullOrEmpty() ? "Hk.Core.Entity" : entityNamespace;
        }

        private string _nameOrConStr { get; set; }
        private DatabaseType _dbType { get; set; }
        private string _entityNamespace { get; }
        private static ILoggerFactory _loger { get; } = new LoggerFactory(new ILoggerProvider[] { new EFCoreSqlLogeerProvider() });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_nameOrConStr.IsNullOrEmpty())
                _nameOrConStr = GlobalSwitch.DefaultDbConName;

            string conStr = DbProviderFactoryHelper.GetConStr(_nameOrConStr);

            switch (_dbType)
            {
                case DatabaseType.SqlServer: optionsBuilder.UseSqlServer(conStr, x => x.UseRowNumberForPaging()).EnableSensitiveDataLogging(); break;
                case DatabaseType.MySql: optionsBuilder.UseMySQL(conStr); break;
                case DatabaseType.PostgreSql: optionsBuilder.UseNpgsql(conStr); break;
                default: throw new Exception("暂不支持该数据库！");
            }

            optionsBuilder.UseLoggerFactory(_loger);
        }

        /// <summary>
        /// 初始化DbContext
        /// </summary>
        /// <param name="modelBuilder">模型建造者</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TheEntity>();
            //以下代码最终目的就是将所有需要的实体类调用上面的方法加入到DbContext中，成为其中的一部分
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var entityMethod = typeof(ModelBuilder).GetMethod("Entity", new Type[] { });
            List<Type> types = Assembly.Load("Hk.Core.Entity").GetTypes()
                .Where(x => x.GetCustomAttribute(typeof(TableAttribute)) != null && x.FullName.Contains(_entityNamespace))
                .ToList();

            foreach (var type in types)
            {
                entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, null);
            }
        }
    }
}
