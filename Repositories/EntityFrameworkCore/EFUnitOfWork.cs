using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly DragonDBContext _dragonDBContext;
        public EFUnitOfWork(DragonDBContext dragonDBContext)
        {
            _dragonDBContext = dragonDBContext;
        }
        public void Add<T>(T entity) where T : class, IAggregateRoot
        {
            _dragonDBContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class, IAggregateRoot
        {
            if (entity == null) throw new Exception("t is null");
            _dragonDBContext.Set<T>().Attach(entity);
            _dragonDBContext.Set<T>().Remove(entity);
        }

        public void Update<T>(T entity) where T : class, IAggregateRoot
        {
            if (entity == null) throw new Exception("t is null");

            _dragonDBContext.Set<T>().Attach(entity);//将数据附加到上下文，支持实体修改和新实体，重置为UnChanged
            _dragonDBContext.Entry<T>(entity).State = EntityState.Modified;
        }

        public void Update<T>(List<T> models) where T : class, IAggregateRoot
        {
            foreach (var t in models)
            {
                _dragonDBContext.Set<T>().Attach(t);
                _dragonDBContext.Entry<T>(t).State = EntityState.Modified;
            }
        }

        public bool Commit()
        {
            using var tran = _dragonDBContext.Database.BeginTransaction();
            try
            {
                _dragonDBContext.SaveChanges();

                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                tran.Dispose();
            }
        }
    }
}
