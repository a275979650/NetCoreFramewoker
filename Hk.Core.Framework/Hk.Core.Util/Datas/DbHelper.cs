﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using Hk.Core.Util.Enum;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Helper;
using Hk.Core.Util.Model;

namespace Hk.Core.Util.Datas
{
    /// <summary>
    /// 描述：数据库操作抽象帮助类
    /// </summary>
    public abstract class DbHelper
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="conStr">连接名或连接字符串</param>
        public DbHelper(DatabaseType dbType, string conStr)
        {
            _dbType = dbType;
            _conStr = DbProviderFactoryHelper.GetConStr(conStr);
        }

        #endregion

        #region 私有成员

        /// <summary>
        /// 数据库类型
        /// </summary>
        protected DatabaseType _dbType;

        /// <summary>
        /// 连接字符串
        /// </summary>
        protected string _conStr;

        /// <summary>
        /// 实体需要引用的额外命名空间
        /// </summary>
        protected string _extraUsingNamespace { get; set; } = string.Empty;

        /// <summary>
        /// 类型映射字典
        /// </summary>
        protected abstract Dictionary<string, Type> DbTypeDic { get; }

        #endregion

        #region 外部接口

        /// <summary>
        /// 通过数据库连接字符串和Sql语句查询返回DataTable
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public DataTable GetDataTableWithSql(string sql)
        {
            DbProviderFactory dbProviderFactory = DbProviderFactoryHelper.GetDbProviderFactory(_dbType);
            using (DbConnection conn = dbProviderFactory.CreateConnection())
            {
                conn.ConnectionString = _conStr;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    DbDataAdapter adapter = dbProviderFactory.CreateDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet table = new DataSet();
                    adapter.Fill(table);

                    return table.Tables[0];
                }
            }
        }

        /// <summary>
        /// 通过数据库连接字符串和Sql语句查询返回DataTable,参数化查询
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public DataTable GetDataTableWithSql(string sql, List<DbParameter> parameters)
        {
            DbProviderFactory dbProviderFactory = DbProviderFactoryHelper.GetDbProviderFactory(_dbType);
            using (DbConnection conn = dbProviderFactory.CreateConnection())
            {
                conn.ConnectionString = _conStr;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }

                    DbDataAdapter adapter = dbProviderFactory.CreateDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet table = new DataSet();
                    adapter.Fill(table);
                    cmd.Parameters.Clear();

                    return table.Tables[0];
                }
            }
        }

        /// <summary>
        /// 通过数据库连接字符串和Sql语句查询返回List
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sqlStr">Sql语句</param>
        /// <returns></returns>
        public List<T> GetListBySql<T>(string sqlStr)
        {
            return GetDataTableWithSql(sqlStr).ToList<T>();
        }

        /// <summary>
        /// 通过数据库连接字符串和Sql语句查询返回List,参数化查询
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        public List<T> GetListBySql<T>(string sqlStr, List<DbParameter> param)
        {
            return GetDataTableWithSql(sqlStr, param).ToList<T>();
        }

        /// <summary>
        /// 执行无返回值的Sql语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        public int ExecuteSql(string sql)
        {
            int count = 0;
            DbProviderFactory dbProviderFactory = DbProviderFactoryHelper.GetDbProviderFactory(_dbType);
            using (DbConnection conn = dbProviderFactory.CreateConnection())
            {
                conn.ConnectionString = _conStr;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (DbCommand cmd = dbProviderFactory.CreateCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    count = cmd.ExecuteNonQuery();

                    return count;
                }
            }
        }

        /// <summary>
        /// 执行无返回值的Sql语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="spList">查询参数</param>
        public int ExecuteSql(string sql, List<DbParameter> paramters)
        {
            int count = 0;
            DbProviderFactory dbProviderFactory = DbProviderFactoryHelper.GetDbProviderFactory(_dbType);
            using (DbConnection conn = dbProviderFactory.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (DbCommand cmd = dbProviderFactory.CreateCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    if (paramters != null && paramters.Count > 0)
                    {
                        foreach (var item in paramters)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }

                    count = cmd.ExecuteNonQuery();

                    return count;
                }
            }
        }

        /// <summary>
        /// 获取数据库中的所有表
        /// </summary>
        /// <param name="schemaName">模式（架构）</param>
        /// <returns></returns>
        public abstract List<DbTableInfo> GetDbAllTables(string schemaName = "dbo");

        /// <summary>
        /// 通过连接字符串和表名获取数据库表的信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public abstract List<TableInfo> GetDbTableInfo(string tableName);

        /// <summary>
        /// 将数据库类型转为对应C#数据类型
        /// </summary>
        /// <param name="dbTypeStr">数据类型</param>
        /// <returns></returns>
        public virtual Type DbTypeStr_To_CsharpType(string dbTypeStr)
        {
            string _dbTypeStr = dbTypeStr.ToLower();
            Type type = null;
            if (DbTypeDic.ContainsKey(_dbTypeStr))
                type = DbTypeDic[_dbTypeStr];
            else
                type = typeof(string);

            return type;
        }

        /// <summary>
        /// 生成实体文件
        /// </summary>
        /// <param name="infos">表字段信息</param>
        /// <param name="tableName">表名</param>
        /// <param name="tableDescription">表描述信息</param>
        /// <param name="filePath">文件路径（包含文件名）</param>
        /// <param name="nameSpace">实体命名空间</param>
        /// <param name="schemaName">架构（模式）名</param>
        public virtual void SaveEntityToFile(List<TableInfo> infos, string tableName, string tableDescription,
            string filePath, string nameSpace, string schemaName = null)
        {
            string properties = "";
            string schema = "";
            if (!schemaName.IsNullOrEmpty())
                schema = $@", Schema = ""{schemaName}""";
            infos.ForEach(item =>
            {
                string isKey = item.IsKey
                    ? @"[Key]"
                    : "";
                Type type = DbTypeStr_To_CsharpType(item.Type);
                string isNullable = item.IsNullable && type.IsValueType ? "?" : "";
                string description = item.Description.IsNullOrEmpty() ? item.Name : item.Description;
                string columnName = description.Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries)
                    .LastOrDefault();
                string idTest = item.Name == "Id" ? "override " : "";
                string newPropertyStr =
        $@"
        /// <summary>
        /// {description}
        /// </summary>
        {isKey}
        [Column(""{columnName}"")]
        public {idTest}{type.Name}{isNullable} {item.Name.ToCamelFirstNameUpper()} {{ get; set; }}
        ";
                properties += newPropertyStr;
            });

            #region 代码构造

            string fileStr =
$@"using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hk.Core.Data.Models;
{_extraUsingNamespace}
namespace {nameSpace}
{{
    /// <summary>
    /// {tableDescription}
    /// </summary>
    [Table(""{tableDescription}""{schema}), Serializable]
    public class {tableName.ToCamelFirstNameUpper()}:BaseModel<string>
    {{
        {properties}
    }}
}}";

            #endregion
            FileHelper.WriteTxt(fileStr, filePath, FileMode.Create);
        }

        #endregion
    }
}