using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Domain
{
    public interface IRepository<TEntity> where TEntity : AggregateRoot, IBaseRepository
    {
        TEntity Find(string id);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> funcWhere);
        PageResult<TEntity> QueryPage<S>(Expression<Func<TEntity, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<TEntity, S>> funcOrderby, bool isAsc = true);
    }
}
