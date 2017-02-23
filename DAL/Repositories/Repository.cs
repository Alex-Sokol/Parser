using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly MyContext _db;
        private bool _disposed;

        public Repository(string name)
        {
            _db = new MyContext(name);
        }

        public void Create(TEntity item)
        {
            _db.Set<TEntity>().Add(item);
        }

        public void AddRange(IEnumerable<TEntity> items)
        {
            _db.Set<TEntity>().AddRange(items);
        }

        public void Delete(int id)
        {
            var s = _db.Set<TEntity>().Find(id);
            if (s != null)
                _db.Set<TEntity>().Remove(s);
        }

        public void DeleteRange(IEnumerable<TEntity> items)
        {
            _db.Set<TEntity>().RemoveRange(items);
        }

        public IEnumerable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return _db.Set<TEntity>().Where(predicate).ToList();
        }

        public TEntity Get(int id)
        {
            return _db.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _db.Set<TEntity>();
        }

        public void Update(TEntity item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                _db.Dispose();
            _disposed = true;
        }

        public void AddOrUpdate(Expression<Func<TEntity, object>> identifierExpression, params TEntity[] entities)
        {
            _db.Set<TEntity>().AddOrUpdate(identifierExpression, entities);
        }
    }
}