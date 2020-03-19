using Infrastructure;
using System.Collections.Generic;

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
        void Update<T>(T entity) where T : class, IAggregateRoot;

        /// <summary>
        /// 修改所有实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        void Update<T>(List<T> models) where T : class, IAggregateRoot;

        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Delete<T>(T entity) where T : class, IAggregateRoot;
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        void BulkInsert<T>(IEnumerable<T> entitys) where T : class, IAggregateRoot;

        /// <summary>
        /// 
        /// </summary>
        bool Commit();
    }
}
