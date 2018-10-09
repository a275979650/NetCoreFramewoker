using System.Collections.Generic;
using Hk.Core.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hk.Core.Data.DbContextCore.DbTypeContext
{
    public class OracleDbContext:BaseDbContext
    {
        public OracleDbContext(IOptions<DbContextOption> option) : base(option)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(_option.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public override void BulkInsert<T, TKey>(IList<T> entities, string destinationTableName = null)
        {
            base.BulkInsert<T, TKey>(entities, destinationTableName);
        }
    }
}