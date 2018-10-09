using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hk.Core.Data.Models;

namespace Hk.Core.Data.Repositories
{
    public interface IRepository : IDisposable
    {
        #region 事物提交

        void BeginTransaction();
        bool EndTransaction();

        #endregion

        #region Insert
        int Add<T>(T entity, bool withTrigger = false) where T : class, new();
        Task<int> AddAsync<T>(T entity, bool withTrigger = false) where T : class, new();
        int AddRange<T>(ICollection<T> entities, bool withTrigger = false) where T : class, new();
        Task<int> AddRangeAsync<T>(ICollection<T> entities, bool withTrigger = false) where T : class, new();
        void BulkInsert<T, TKey>(IList<T> entities, string destinationTableName = null) where T : BaseModel<TKey>;


        #endregion

        #region Delete

        int Delete<T,TKey>(TKey key, bool withTrigger = false) where T : BaseModel<TKey>;
        int Delete<T>(Expression<Func<T, bool>> @where) where T : class, new();
        Task<int> DeleteAsync<T>(Expression<Func<T, bool>> @where) where T : class, new();

        #endregion

        #region Update
        int Edit<T,TKey>(T entity, bool withTrigger = false) where T : BaseModel<TKey>;
        int EditRange<T>(ICollection<T> entities, bool withTrigger = false) where T : class, new();
        int BatchUpdate<T>(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateExp) where T : class, new();
        Task<int> BatchUpdateAsync<T>(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateExp) where T : class, new();
        int Update<T>(T model, bool withTrigger = false, params string[] updateColumns) where T : class, new();
        int Update<T>(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory) where T : class, new();
        Task<int> UpdateAsync<T>(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory) where T : class, new();




        #endregion

        #region Query
        int Count<T>(Expression<Func<T, bool>> @where = null) where T : class, new();
        Task<int> CountAsync<T>(Expression<Func<T, bool>> @where = null) where T : class, new();
        bool Exist<T>(Expression<Func<T, bool>> @where = null) where T : class, new();
        Task<bool> ExistAsync<T>(Expression<Func<T, bool>> @where = null) where T : class, new();
        T GetSingle<T,TKey>(TKey key) where T : BaseModel<TKey>;
        T GetSingle<T,TKey>(TKey key, Func<IQueryable<T>, IQueryable<T>> includeFunc) where T : BaseModel<TKey>;
        Task<T> GetSingleAsync<T,TKey>(TKey key) where T : BaseModel<TKey>;
        T GetSingleOrDefault<T>(Expression<Func<T, bool>> @where = null) where T : class, new();
        Task<T> GetSingleOrDefaultAsync<T>(Expression<Func<T, bool>> @where = null) where T : class, new();
        IQueryable<T> Get<T>(Expression<Func<T, bool>> @where = null) where T : class, new();
        Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> @where = null) where T : class, new();
        IEnumerable<T> GetByPagination<T>(Expression<Func<T, bool>> @where, int pageSize, int pageIndex, bool asc = true,
            params Func<T, object>[] @orderby) where T : class, new();
        DataTable GetDataTableWithSql(string sql);
        DataTable GetDataTableWithSql(string sql, List<DbParameter> parameters);
        #endregion

        #region 执行Sql语句

        void ExecuteSql(string sql);
        void ExecuteSql(string sql, List<DbParameter> spList);

        #endregion
    }
}