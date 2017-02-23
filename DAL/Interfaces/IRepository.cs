using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAL.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        IEnumerable<TEntity> Find(Func<TEntity, bool> predicate);
        void Create(TEntity item);
        void AddRange(IEnumerable<TEntity> items);
        void Delete(int id);
        void DeleteRange(IEnumerable<TEntity> items);
        void Update(TEntity item);
        void AddOrUpdate(Expression<Func<TEntity, object>> identifierExpression, params TEntity[] entities);
        void Save();
    }
}