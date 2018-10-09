using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Hk.Core.Data.Models;
using Hk.Core.Data.Options;
using Hk.Core.Util.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hk.Core.Data.DbContextCore.DbTypeContext
{
    public class SqlServerDbContext:BaseDbContext
    {
        public SqlServerDbContext(IOptions<DbContextOption> option) : base(option)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_option.ConnectionString);
            optionsBuilder.UseSqlServer(_option.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public override void BulkInsert<T, TKey>(IList<T> entities, string destinationTableName = null)
        {
            if (entities == null || !entities.Any()) return;
            if (string.IsNullOrEmpty(destinationTableName))
            {
                var mappingTableName = typeof(T).GetCustomAttribute<TableAttribute>()?.Name;
                destinationTableName = string.IsNullOrEmpty(mappingTableName) ? typeof(T).Name : mappingTableName;
            }
            SqlBulkInsert<T, TKey>(entities, destinationTableName);
        }

        private void SqlBulkInsert<T, TKey>(IList<T> entities, string destinationTableName = null) where T : BaseModel<TKey>
        {
            using (var dt = entities.ToDataTable())
            {
                dt.TableName = destinationTableName;
                using (var conn = Database.GetDbConnection() as SqlConnection ?? new SqlConnection(_option.ConnectionString))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var bulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran)
                            {
                                BatchSize = entities.Count,
                                DestinationTableName = dt.TableName,
                            };
                            GenerateColumnMappings<T, TKey>(bulk.ColumnMappings);
                            bulk.WriteToServerAsync(dt);
                            tran.Commit();
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                    conn.Close();
                }
            }
        }

        private void GenerateColumnMappings<T, TKey>(SqlBulkCopyColumnMappingCollection mappings)
            where T : BaseModel<TKey>
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttributes<KeyAttribute>().Any())
                {
                    mappings.Add(new SqlBulkCopyColumnMapping(property.Name, typeof(T).Name + property.Name));
                }
                else
                {
                    mappings.Add(new SqlBulkCopyColumnMapping(property.Name, property.Name));
                }
            }
        }

        public override PaginationResult SqlQueryByPagnation<T, TView>(string sql, string[] orderBys, int pageIndex, int pageSize,
            Action<TView> eachAction = null)
        {
            var total = SqlQuery<T, int>($"select count(1) from ({sql}) as s").FirstOrDefault();
            var jsonResults = SqlQuery<T, TView>(
                    $"select * from (select *,row_number() over (order by {string.Join(",", orderBys)}) as RowId from ({sql}) as s) as t where RowId between {pageSize * (pageIndex - 1) + 1} and {pageSize * pageIndex} order by {string.Join(",", orderBys)}")
                .ToList();
            if (eachAction != null)
            {
                jsonResults = jsonResults.Each(eachAction).ToList();
            }

            return new PaginationResult(true, string.Empty, jsonResults)
            {
                pageIndex = pageIndex,
                pageSize = pageSize,
                total = total
            };
        }

        public override DataTable GetDataTableWithSql(string sql)
        {
            using (var conn = Database.GetDbConnection() as SqlConnection ?? new SqlConnection(_option.ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (SqlCommand cmd = new SqlCommand(sql,conn))
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    SqlDataAdapter adapter = new SqlDataAdapter(sql, conn)
                    {
                        SelectCommand = cmd
                    };
                    DataSet table = new DataSet();
                    adapter.Fill(table);

                    return table.Tables[0];
                }
            }
        }
    }
}