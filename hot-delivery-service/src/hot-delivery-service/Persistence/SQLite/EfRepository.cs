using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace hot_delivery_service.Persistence.SQLite
{
    //реализация ef репозитория 
    //повторяет функционал DbSet
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private DbSet<TEntity> _dbSet;

        public EfRepository(DbSet<TEntity> dbSet)
        {
            _dbSet = dbSet;
        }

        public Type ElementType
        {
            get
            {
                return ((IQueryable<TEntity>)_dbSet).ElementType;
            }
        }

        public Expression Expression
        {
            get
            {
                return ((IQueryable<TEntity>)_dbSet).Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return ((IQueryable<TEntity>)_dbSet).Provider;
            }
        }

        public TEntity Add(TEntity entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            return entities;
        }

        public TEntity Attach(TEntity entity)
        {
            return _dbSet.Attach(entity).Entity;
        }

        public TEntity Create()
        {
            var entity = new TEntity();
            _dbSet.Add(entity);
            return entity;
        }
        
        public IEnumerator<TEntity> GetEnumerator()
        {
            return ((IEnumerable<TEntity>)_dbSet).GetEnumerator();
        }

        public TEntity Remove(TEntity entity)
        {
            return _dbSet.Remove(entity).Entity;
        }

        public IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            return entities;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TEntity>)_dbSet).GetEnumerator();
        }
    }
}
