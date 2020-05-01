using Infrastructure;
using Kogel.Dapper.Extension.Core.Interfaces;
using Kogel.Dapper.Extension.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Domain
{
    public interface IRepository<TEntity> where TEntity : AggregateRoot, IBaseRepository
    {
        TEntity QueryEntity(Expression<Func<TEntity, bool>> funcWhere);
        IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> funcWhere);
        PageList<TEntity> QueryPage(Expression<Func<TEntity, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<TEntity, Object>> funcOrderby, bool isAsc = true);
        IQuerySet<TEntity> QuerySet();
    }
}
