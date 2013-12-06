using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace nCommon.Repository.Infrastructure
{
    public interface IRepo<T>
    {
        T Get(object id);
        T Get(Expression<Func<T, bool>> filterExpr);
        T First(Expression<Func<T, bool>> filterExpr, Expression<Func<T, object>> orderExpr, bool isAsc = true);
        int Count(Expression<Func<T, bool>> filterExpr);
        List<T> Gets(Expression<Func<T, bool>> filterExpr = null, PageParameter p = null);
        T Insert(T entity);
        void Delete(T entity);
        void Delete(object id);
        void Delete(Expression<Func<T, bool>> filterExpr);
        void Update(T entity);
        void Update(Expression<Func<T, bool>> filterExpr, Expression<Func<T, T>> updateExpr);
        List<T> GetAll();
        IUnitOfWork UnitOfWork { get; }
    }
}
