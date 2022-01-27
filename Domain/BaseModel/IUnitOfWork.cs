using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain
{
    public interface IUnitOfWork : IBaseRepository
    {
        /// <summary>
        /// 新增单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Add<T>(T entity) where T : class, IAggregateRoot;

        /// <summary>
        /// 修改单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update<T>(Expression<Func<T, bool>> funcWhere, T entity) where T : class, IAggregateRoot;

        /// <summary>
        /// 修改所有实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        void Update<T>(Expression<Func<T, bool>> expression, Expression<Func<T, T>> updateExpression) where T : class, IAggregateRoot;

        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Delete<T>(Expression<Func<T, bool>> funcWhere) where T : class, IAggregateRoot;
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        Task BulkInsert<T>(ICollection<T> entitys) where T : class, IAggregateRoot;

        /// <summary>
        /// 
        /// </summary>
        bool Commit();

        /// <summary>
        /// 
        /// </summary>
        Task<bool> CommitAsync();
    }
}
