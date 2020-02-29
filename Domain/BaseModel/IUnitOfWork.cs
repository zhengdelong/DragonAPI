using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface IUnitOfWork
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
        /// 
        /// </summary>
        bool Commit();
    }
}
