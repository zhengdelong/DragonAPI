using System;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure;
using Domain;
using System.Collections.Generic;

namespace Repositories
{
    /// <summary>
    /// ef core 查询
    /// </summary>
    public class EFCoreBase<T>: IRepository<T> where T : AggregateRoot
    {
        public DragonDBContext DragonDBContext { get; private set; }
        public EFCoreBase(DragonDBContext dragonDBContext)
        {
            DragonDBContext = dragonDBContext;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T QueryEntity(Expression<Func<T,bool>> expression)
        {
            return DragonDBContext.Set<T>().Where(expression).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public IEnumerable<T> Query(Expression<Func<T, bool>> funcWhere) 
        {
            if (funcWhere == null)
                return DragonDBContext.Set<T>().ToList();
            else
                return DragonDBContext.Set<T>().Where(funcWhere).ToList();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="funcWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderby"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public PageResult<T> QueryPage<S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, bool isAsc = true)
        {
            var list = DragonDBContext.Set<T>().AsQueryable();
            if (funcWhere != null)
            {
                list = list.Where(funcWhere);
            }

            list = isAsc ? list.OrderBy(funcOrderby) : list.OrderByDescending(funcOrderby);
            var result = new PageResult<T>()
            {
                DataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = list.Count()
            };
            return result;
        }
    }
}
