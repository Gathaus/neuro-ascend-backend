using System.Linq.Expressions;

namespace Neuro.Domain.Repositories;

    public interface IRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : class
    {
    }

    public interface IRepository<TEntity, TPrimaryKey>
    {
        // CREATE operations
        Task<TEntity> InsertAsync(TEntity entity);
        TEntity Insert(TEntity entity);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        Task BulkInsertAsync(IEnumerable<TEntity> entities);
        Task BulkInsertAsync(IEnumerable<TEntity> entities,object? bulkConfig);

        // READ operations
        Task<TEntity> GetByIdAsync(TPrimaryKey id, params string[] includeParams);
        Task<TEntity> GetAsync(params string[] includeParams);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, params string[] includeParams);
        IQueryable<TEntity> FindBy(params string[] includeParams);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> expression, params string[] includeParams);
        IQueryable<TEntity> QueryFromSql(string sql, params object[] parameters);
        // UPDATE operations
        void Update(TEntity entity);
        Task BulkUpdateAsync(IEnumerable<TEntity> entities); 

        // DELETE operations
        bool Delete(TEntity entity);
        bool HardDelete(TEntity entity);
        Task BulkDeleteAsync(IEnumerable<TEntity> entities);
    }