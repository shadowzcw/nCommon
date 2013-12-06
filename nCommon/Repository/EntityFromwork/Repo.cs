using System;
using System.Data.Entity;
using System.Linq.Expressions;
using EntityFramework.Extensions;
using LinqKit;
using nCommon.Repository.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace nCommon.Repository.EntityFromwork
{
    public class Repo<T> : IRepo<T> where T : class
    {
        private readonly IEntityFromworkUnitOfWork _context;

        public IDbSet<T> Entities
        {
            get { return _context.CreateSet<T>(); }
        }

        public IUnitOfWork UnitOfWork { get { return _context; } }

        public Repo(IEntityFromworkUnitOfWork unitOfWork)
        {
            _context = unitOfWork;
        }

        public virtual T Get(object id)
        {
            return Entities.Find(id);
        }

        public T Get(Expression<Func<T, bool>> filterExpr)
        {
            return Entities.AsExpandable().FirstOrDefault(filterExpr);
        }

        public T First(Expression<Func<T, bool>> filterExpr, Expression<Func<T, object>> orderExpr, bool isAsc = true)
        {
            var query = Entities.AsExpandable().Where(filterExpr);
            var unaryExpression = orderExpr.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                var propertyExpression = (MemberExpression)unaryExpression.Operand;
                var parameters = orderExpr.Parameters;

                if (propertyExpression.Type == typeof(DateTime))
                {
                    var newExpression = Expression.Lambda<Func<T, DateTime>>(propertyExpression, parameters);
                    query = isAsc ? query.OrderBy(newExpression) : query.OrderByDescending(newExpression);
                }
                else if (propertyExpression.Type == typeof(int))
                {
                    var newExpression = Expression.Lambda<Func<T, int>>(propertyExpression, parameters);
                    query = isAsc ? query.OrderBy(newExpression) : query.OrderByDescending(newExpression);
                }
                else if (propertyExpression.Type == typeof(string))
                {
                    var newExpression = Expression.Lambda<Func<T, string>>(propertyExpression, parameters);
                    query = isAsc ? query.OrderBy(newExpression) : query.OrderByDescending(newExpression);
                }
            }
            return query.FirstOrDefault();
        }

        public int Count(Expression<Func<T, bool>> filterExpr)
        {
            return Entities.AsExpandable().Count(filterExpr);
        }

        public List<T> Gets(Expression<Func<T, bool>> filterExpr=null, PageParameter p = null)
        {
            var query = filterExpr != null ? Entities.AsExpandable().Where(filterExpr) : Entities.AsExpandable();
            return p != null ? query.GetPageList(p).ToList() : query.ToList();
        }

        public virtual T Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            Entities.Add(entity);
            return entity;
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            Entities.Remove(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = Get(id);
            Delete(entity);
        }

        public void Delete(Expression<Func<T, bool>> filterExpr)
        {
            Entities.Delete(filterExpr);
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            _context.SetModified(entity);
        }

        public void Update(Expression<Func<T, bool>> filterExpr, Expression<Func<T, T>> updateExpr)
        {
            Entities.Update(filterExpr, updateExpr);
        }

        public virtual List<T> GetAll()
        {
            return Entities.ToList();
        }
    }
}
