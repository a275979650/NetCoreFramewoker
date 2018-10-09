﻿using Hk.Core.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
namespace Hk.Core.Data.DbContextCore.DbTypeContext
{
    public class MySqlDbContext:BaseDbContext
    {
        public MySqlDbContext(IOptions<DbContextOption> option) : base(option)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_option.ConnectionString);
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
           // MySqlBulkInsert(entities, destinationTableName);
        }

        //private void MySqlBulkInsert<T>(IList<T> entities, string destinationTableName) where T : class
        //{
        //    var tmpDir = Path.Combine(AppContext.BaseDirectory, "Temp");
        //    if (!Directory.Exists(tmpDir))
        //        Directory.CreateDirectory(tmpDir);
        //    var csvFileName = Path.Combine(tmpDir, $"{DateTime.Now:yyyyMMddHHmmssfff}.csv");
        //    if (!File.Exists(csvFileName))
        //        File.Create(csvFileName);
        //    var separator = ",";
        //    entities.SaveToCsv(csvFileName, separator);
        //    using (var conn = Database.GetDbConnection() as MySqlConnection ?? new MySqlConnection(_option.ConnectionString))
        //    {
        //        conn.Open();
        //        var bulk = new MySqlBulkLoader(conn)
        //        {
        //            NumberOfLinesToSkip = 0,
        //            TableName = destinationTableName,
        //            FieldTerminator = separator,
        //            FieldQuotationCharacter = '"',
        //            EscapeCharacter = '"',
        //            LineTerminator = "\r\n"
        //        };
        //        bulk.LoadAsync();
        //        conn.Close();
        //    }
        //    File.Delete(csvFileName);
        //}

    }
}