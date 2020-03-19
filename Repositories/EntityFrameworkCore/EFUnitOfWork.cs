using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySql.Data.MySqlClient;
using Repositories.EntityFrameworkCore;

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
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        public void BulkInsert<T>([NotNull]IEnumerable<T> entitys) where T : class, IAggregateRoot
        {
            int insertcount = entitys.Count();
            int numresult = insertcount / 200;
            int remainder = insertcount % 200;

            if (numresult <= 0)
            {
                var bulkModel = _dragonDBContext.GetBulkModels<T>(entitys);
                bulkModels.AddLast(bulkModel);
            }
            else
            {
                for (int i = 1; i <= numresult; i++)
                {
                    var bulkModel = _dragonDBContext.GetBulkModels<T>(entitys.Skip((i - 1) * 200).Take(200));
                    bulkModels.AddLast(bulkModel);
                }
                if (remainder > 0)
                {
                    var bulkModel = _dragonDBContext.GetBulkModels<T>(entitys.TakeLast(remainder));
                    bulkModels.AddLast(bulkModel);
                }
            }

        }

        private LinkedList<BulkModel> bulkModels = new LinkedList<BulkModel>();

        public bool Commit()
        {
            using var tran = _dragonDBContext.Database.BeginTransaction();
            try
            {
                _dragonDBContext.SaveChanges();
                foreach (var bulk in bulkModels)
                {
                    _dragonDBContext.Database.ExecuteSqlRaw(bulk.Sql, bulk.Parameters);
                }
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
                bulkModels = null;
                tran.Dispose();
            }
        }
    }
}
