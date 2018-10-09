using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Models;
using Hk.Core.Util.Extentions;
using Microsoft.EntityFrameworkCore;

namespace Hk.Core.Data.Repositories
{
    public class BaseRepository:IRepository
    {
        protected readonly IDbContextCore DbContext;

        protected BaseRepository(IDbContextCore dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbContext.EnsureCreatedAsync();
        }

        public void BeginTransaction()
        {
            throw new System.NotImplementedException();
        }

        public bool EndTransaction()
        {
            throw new System.NotImplementedException();
        }

        public virtual int Add<T>(T entity, bool withTrigger = false) where T : class, new()
        {
            return DbContext.Add(entity, withTrigger);
        }

        public virtual async Task<int> AddAsync<T>(T entity, bool withTrigger = false) where T : class, new()
        {
            return await DbContext.AddAsync(entity, withTrigger);
        }

        public virtual int AddRange<T>(ICollection<T> entities, bool withTrigger = false) where T : class, new()
        {
            return DbContext.AddRange(entities, withTrigger);
        }

        public virtual async Task<int> AddRangeAsync<T>(ICollection<T> entities, bool withTrigger = false) where T : class, new()
        {
            return await DbContext.AddRangeAsync(entities, withTrigger);
        }

        public virtual void BulkInsert<T, TKey>(IList<T> entities, string destinationTableName = null) where T : BaseModel<TKey>
        {
            DbContext.BulkInsert<T, TKey>(entities, destinationTableName);
        }


        public virtual int Delete<T, TKey>(TKey key, bool withTrigger = false) where T : BaseModel<TKey>
        {
            return DbContext.Delete<T, TKey>(key, withTrigger);
        }
        public virtual int Delete<T>(Expression<Func<T, bool>> @where) where T : class, new()
        {
            return DbContext.Delete(where);
        }

        public virtual async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> @where) where T : class, new()
        {
            return await DbContext.DeleteAsync(where);
        }

        public virtual int Edit<T, TKey>(T entity, bool withTrigger = false) where T : BaseModel<TKey>
        {
            return DbContext.Edit<T, TKey>(entity, withTrigger);
        }


        public virtual int EditRange<T>(ICollection<T> entities, bool withTrigger = false) where T : class, new()
        {
            return DbContext.EditRange(entities, withTrigger);
        }

        public virtual int BatchUpdate<T>(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateExp) where T : class, new()
        {
            return DbContext.Update(where, updateExp);
        }

        public virtual async Task<int> BatchUpdateAsync<T>(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateExp) where T : class, new()
        {
            return await DbContext.UpdateAsync(@where, updateExp);
        }

        public virtual int Update<T>(T model, bool withTrigger = false, params string[] updateColumns) where T : class, new()
        {
            DbContext.Update(model, withTrigger, updateColumns);
            return DbContext.SaveChanges();
        }

        public virtual int Update<T>(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory) where T : class, new()
        {
            return DbContext.Update(where, updateFactory);
        }

        public virtual async Task<int> UpdateAsync<T>(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory) where T : class, new()
        {
            return await DbContext.UpdateAsync(where, updateFactory);
        }

        public virtual int Count<T>(Expression<Func<T, bool>> @where = null) where T : class, new()
        {
            return DbContext.Count(where);
        }

        public virtual async Task<int> CountAsync<T>(Expression<Func<T, bool>> @where = null) where T : class, new()
        {
            return await DbContext.CountAsync(where);
        }

        public virtual bool Exist<T>(Expression<Func<T, bool>> @where = null) where T : class, new()
        {
            return DbContext.Exist(where);
        }

        public virtual async Task<bool> ExistAsync<T>(Expression<Func<T, bool>> @where = null) where T : class, new()
        {
            return await DbContext.ExistAsync(where);
        }

        public virtual T GetSingle<T, TKey>(TKey key) where T : BaseModel<TKey>
        {
            DbSet<T> dbSet  =  DbContext.GetDbSet<T>();
            return dbSet.Find(key);
        }

        public virtual T GetSingle<T, TKey>(TKey key, Func<IQueryable<T>, IQueryable<T>> includeFunc) where T : BaseModel<TKey>
        {
            DbSet<T> dbSet = DbContext.GetDbSet<T>();
            if (includeFunc == null) return dbSet.Find(key);
            return includeFunc(dbSet.Where(m => m.Id.Equal(key))).AsNoTracking().FirstOrDefault();
        }

        public virtual async Task<T> GetSingleAsync<T,TKey>(TKey key) where T : BaseModel<TKey>
        {
            return await DbContext.FindAsync<T, TKey>(key);
        }

        public virtual T GetSingleOrDefault<T>(Expression<Func<T, bool>> @where = null) where T : class, new()
        {
            return DbContext.GetSingleOrDefault(@where);
        }

        public virtual async Task<T> GetSingleOrDefaultAsync<T>(Expression<Func<T, bool>> @where = null) where T : class, new()
        {
            return await DbContext.GetSingleOrDefaultAsync(where);
        }

        public virtual IQueryable<T> Get<T>(Expression<Func<T, bool>> @where = null) where T : class, new()
        {
            DbSet<T> dbSet = DbContext.GetDbSet<T>();
            return (@where != null ? dbSet.Where(@where).AsNoTracking() : dbSet.AsNoTracking());
        }

        public virtual async Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> @where = null) where T : class, new()
        {
            DbSet<T> dbSet = DbContext.GetDbSet<T>();
            return await dbSet.Where(where).ToListAsync();
        }

        public virtual IEnumerable<T> GetByPagination<T>(Expression<Func<T, bool>> @where, int pageSize, int pageIndex, bool asc = true, params Func<T, object>[] @orderby) where T : class, new()
        {
            var filter = Get(where).AsEnumerable();
            if (orderby != null)
            {
                foreach (var func in orderby)
                {
                    filter = asc ? filter.OrderBy(func) : filter.OrderByDescending(func);
                }
            }
            return filter.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        }


        public virtual DataTable GetDataTableWithSql(string sql)
        {
            return DbContext.GetDataTableWithSql(sql);
        }

        public virtual DataTable GetDataTableWithSql(string sql, List<DbParameter> parameters)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ExecuteSql(string sql)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ExecuteSql(string sql, List<DbParameter> spList)
        {
            throw new System.NotImplementedException();
        }
        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    DbContext?.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~BaseRepositoryTT() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}