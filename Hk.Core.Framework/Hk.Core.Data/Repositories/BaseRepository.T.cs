﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Models;
using Hk.Core.Util.Extentions;
using Microsoft.EntityFrameworkCore;

namespace Hk.Core.Data.Repositories
{
    public abstract class BaseRepository<T,TKey>: IRepository<T,TKey> where T : BaseModel<TKey>
    {
        protected readonly IDbContextCore DbContext;
        protected DbSet<T> DbSet => DbContext.GetDbSet<T>();

        protected BaseRepository(IDbContextCore dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbContext.EnsureCreatedAsync();
        }
        public void BeginTransaction()
        {
            DbContext.GetDatabase().BeginTransaction();
        }

        public bool EndTransaction()
        {
            throw new NotImplementedException();
        }
        public void ExecuteSql(string sql)
        {
            DbContext.ExecuteSqlWithNonQuery(sql);
        }

        public void ExecuteSql(string sql, List<DbParameter> spList)
        {
            DbContext.ExecuteSqlWithNonQuery(sql, spList);
        }



        #region Insert

        public virtual int Add(T entity, bool withTrigger = false)
        {
            return DbContext.Add(entity, withTrigger);
        }

        public virtual async Task<int> AddAsync(T entity, bool withTrigger = false)
        {
            return await DbContext.AddAsync(entity, withTrigger);
        }

        public virtual int AddRange(ICollection<T> entities, bool withTrigger = false)
        {
            return DbContext.AddRange(entities, withTrigger);
        }

        public virtual async Task<int> AddRangeAsync(ICollection<T> entities, bool withTrigger = false)
        {
            return await DbContext.AddRangeAsync(entities, withTrigger);
        }

        public virtual void BulkInsert(IList<T> entities, string destinationTableName = null)
        {
            DbContext.BulkInsert<T, TKey>(entities, destinationTableName);
        }

        #endregion

        #region Update

        public virtual int Edit(T entity, bool withTrigger = false)
        {
            return DbContext.Edit<T, TKey>(entity, withTrigger);
        }

        public virtual int EditRange(ICollection<T> entities, bool withTrigger = false)
        {
            return DbContext.EditRange(entities, withTrigger);
        }
        /// <summary>
        /// update query datas by columns.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="updateExp"></param>
        /// <returns></returns>
        public virtual int BatchUpdate(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateExp)
        {
            return DbContext.Update(where, updateExp);
        }

        public virtual async Task<int> BatchUpdateAsync(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateExp)
        {
            return await DbContext.UpdateAsync(@where, updateExp);
        }
        public virtual int Update(T model, bool withTrigger = false, params string[] updateColumns)
        {
            DbContext.Update(model, withTrigger, updateColumns);
            return DbContext.SaveChanges();
        }

        public virtual int Update(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory)
        {
            return DbContext.Update(where, updateFactory);
        }

        public virtual async Task<int> UpdateAsync(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory)
        {
            return await DbContext.UpdateAsync(where, updateFactory);
        }

        #endregion

        #region Delete

        public virtual int Delete(TKey key, bool withTrigger = false)
        {
            return DbContext.Delete<T, TKey>(key, withTrigger);
        }

        public virtual int Delete(Expression<Func<T, bool>> @where)
        {
            return DbContext.Delete(where);
        }

        public virtual async Task<int> DeleteAsync(Expression<Func<T, bool>> @where)
        {
            return await DbContext.DeleteAsync(where);
        }


        #endregion

        #region Query

        public virtual int Count(Expression<Func<T, bool>> @where = null)
        {
            return DbContext.Count(where);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> @where = null)
        {
            return await DbContext.CountAsync(where);
        }


        public virtual bool Exist(Expression<Func<T, bool>> @where = null)
        {
            return DbContext.Exist(where);
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<T, bool>> @where = null)
        {
            return await DbContext.ExistAsync(where);
        }

        /// <summary>
        /// 根据主键获取实体。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T GetSingle(TKey key)
        {
            return DbSet.Find(key);
        }

        public T GetSingle(TKey key, Func<IQueryable<T>, IQueryable<T>> includeFunc)
        {
            if (includeFunc == null) return GetSingle(key);
            return includeFunc(DbSet.Where(m => m.Id.Equal(key))).AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// 根据主键获取实体。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<T> GetSingleAsync(TKey key)
        {
            return await DbContext.FindAsync<T, TKey>(key);
        }

        /// <summary>
        /// 获取单个实体。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual T GetSingleOrDefault(Expression<Func<T, bool>> @where = null)
        {
            return DbContext.GetSingleOrDefault(@where);
        }

        /// <summary>
        /// 获取单个实体。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual async Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> @where = null)
        {
            return await DbContext.GetSingleOrDefaultAsync(where);
        }

        /// <summary>
        /// 获取实体列表。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> @where = null)
        {
            return (@where != null ? DbSet.Where(@where).AsNoTracking() : DbSet.AsNoTracking());
        }

        /// <summary>
        /// 获取实体列表。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual async Task<List<T>> GetAsync(Expression<Func<T, bool>> @where = null)
        {
            return await DbSet.Where(where).ToListAsync();
        }

        /// <summary>
        /// 分页获取实体列表。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual IEnumerable<T> GetByPagination(Expression<Func<T, bool>> @where, int pageSize, int pageIndex, bool asc = true, params Func<T, object>[] @orderby)
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

        public DataTable GetDataTableWithSql(string sql)
        {
            return DbContext.GetDataTableWithSql(sql);
        }
        public DataTable GetDataTableWithSql(string sql, List<DbParameter> parameters)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region other

        public IEnumerator<T> GetEnumerator()
        {
            return DbSet.AsQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType => DbSet.AsQueryable().ElementType;
        public Expression Expression => DbSet.AsQueryable().Expression;
        public IQueryProvider Provider => DbSet.AsQueryable().Provider;


        #endregion

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