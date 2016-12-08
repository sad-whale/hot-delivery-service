using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace hot_delivery_service.Persistence.File
{
    public class ListRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private List<TEntity> _entities;

        public ListRepository(List<TEntity> entities)
        {
            _entities = entities;
        }

        public Type ElementType
        {
            get
            {
                return _entities.AsQueryable().ElementType;
            }
        }

        public Expression Expression
        {
            get
            {
                return _entities.AsQueryable().Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return _entities.AsQueryable().Provider;
            }
        }

        public TEntity Add(TEntity entity)
        {
            _entities.Add(entity);
            return entity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
            return entities;
        }

        public TEntity Attach(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Create()
        {
            var entity = new TEntity();
            _entities.Add(entity);
            return entity;
        }
        
        public IEnumerator<TEntity> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        public TEntity Remove(TEntity entity)
        {
            _entities.Remove(entity);
            return entity;
        }

        public IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _entities.Remove(entity);
            }
            return entities;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entities.GetEnumerator();
        }
    }
}
