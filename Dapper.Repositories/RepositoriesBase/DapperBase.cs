using Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Kogel.Dapper.Extension.Model;
using Kogel.Dapper.Extension.MySql;
using Kogel.Dapper.Extension.Core.Interfaces;
using MySqlConnector;

namespace Dapper.Repositories
{
    public class DapperBase<T> : IRepository<T> where T : AggregateRoot
    {
        private readonly MySqlConnection _mySqlConnection;
        public DapperBase(MySqlConnection mySqlConnection)
        {
            _mySqlConnection = mySqlConnection;
        }
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public IEnumerable<T> Query(Expression<Func<T, bool>> funcWhere)
        {
            return _mySqlConnection.QuerySet<T>().Where(funcWhere).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public T QueryEntity(Expression<Func<T, bool>> funcWhere)
        {
            var query = _mySqlConnection.QuerySet<T>();
            var res = query.Where(funcWhere).Get();
            return res;
        }
        /// <summary>
        /// 基本分页查询
        /// </summary>
        /// <param name="funcWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderby"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public PageList<T> QueryPage(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, object>> funcOrderby, bool isAsc = true)
        {
            var res = _mySqlConnection.QuerySet<T>().Where(funcWhere).OrderBy(funcOrderby).PageList(pageIndex, pageSize);
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQuerySet<T> QuerySet()
        {
            return _mySqlConnection.QuerySet<T>();
        }
    }
}
