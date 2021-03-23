using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Domain;
using Infrastructure;
using Kogel.Dapper.Extension;
using Kogel.Dapper.Extension.Model;
using Kogel.Dapper.Extension.MySql;
using MySqlConnector;

namespace Dapper.Repositories
{
    public class DapperUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MySqlConnection _mySqlConnection;
        private readonly MySqlTransaction _mySqlTransaction;
        public DapperUnitOfWork(MySqlConnection mySqlConnection)
        {
            _mySqlConnection = mySqlConnection;
            _mySqlConnection.Open();
            _mySqlTransaction = _mySqlConnection.BeginTransaction();
        }
        public bool Commit()
        {
            try
            {
                _mySqlTransaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _mySqlTransaction.Rollback();
                _mySqlConnection.Close();
                throw;
            }
            finally
            {
                _mySqlConnection.Close();
                _mySqlConnection.Dispose();
                //_mySqlTransaction.Dispose();
            }
        }

        void IUnitOfWork.Add<T>(T entity)
        {
            _mySqlConnection.CommandSet<T>(_mySqlTransaction)
                 .Insert(entity);
        }
        void IUnitOfWork.Delete<T>(Expression<Func<T, bool>> funcWhere)
        {
            _mySqlConnection.CommandSet<T>(_mySqlTransaction).Where(funcWhere).Delete();
        }

        void IUnitOfWork.Update<T>(Expression<Func<T, bool>> funcWhere, T entity)
        {
            _mySqlConnection.CommandSet<T>(_mySqlTransaction).Where(funcWhere).Update(entity);
        }

        void IUnitOfWork.Update<T>(Expression<Func<T, bool>> funcWhere, Expression<Func<T, T>> updateParamer)
        {
            _mySqlConnection.CommandSet<T>(_mySqlTransaction).Where(funcWhere).Update(updateParamer);
        }
        //[Obsolete]
        //async Task IUnitOfWork.BulkInsert<T>(IEnumerable<T> entitys)
        //{
        //    var entityObject = EntityCache.QueryEntity(typeof(T));
        //    var table = new DataTable();
        //    foreach (var item in entityObject.EntityFieldList)
        //    {
        //        var column = new DataColumn
        //        {
        //            ColumnName = item.FieldName,
        //            DataType = item.PropertyInfo.PropertyType
        //        };
        //        table.Columns.Add(column);
        //    }
        //    var models = entitys.ToList();
        //    int numresult = models.Count / 100;
        //    int remainder = models.Count % 100;
        //    if (numresult <= 0)
        //    {
        //        await InsertInner(entitys, table, entityObject);
        //    }
        //    else
        //    {
        //        for (int i = 0; i < numresult; i++)
        //        {
        //            await InsertInner(models.Skip((i - 1) * 50).Take(50), table, entityObject);
        //        }
        //        if (remainder > 0)
        //        {
        //            await InsertInner(models.TakeLast(remainder), table, entityObject);
        //        }
        //    }
        //}
        //[Obsolete]
        //private async Task InsertInner<T>(IEnumerable<T> entitys, DataTable table, EntityObject entityObject)
        //{
        //    foreach (var item in entitys)
        //    {
        //        var wrapped = ObjectAccessor.Create(item);
        //        var row = table.NewRow();
        //        foreach (var field in entityObject.EntityFieldList)
        //        {
        //            row[field.FieldName] = wrapped[field.PropertyInfo.Name];
        //        }
        //        table.Rows.Add(row);
        //    }
        //    var bulkCopy = new MySqlBulkCopy(_mySqlConnection, _mySqlTransaction)
        //    {
        //        DestinationTableName = entityObject.Name
        //    };
        //    await bulkCopy.WriteToServerAsync(table);
        //    table.Clear();
        //}
        public void Dispose() => _mySqlConnection.Dispose();

        async Task IUnitOfWork.BulkInsert<T>(ICollection<T> entitys)
        {
            var dataItems = entitys.ToList();
            var entityObject = EntityCache.Register(typeof(T));
            using var ms = new MemoryStream();
            using var writer = new StreamWriter(ms, Encoding.UTF8);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.Configuration.HasHeaderRecord = true;
            csv.Configuration.RegisterClassMap<AotuMapper<T>>();
            if (dataItems != null && dataItems.Count > 0)
            {
                csv.WriteRecords(dataItems);
            }
            writer.Flush();
            ms.Position = 0;
            //var mybyte = ms.ToArray();
            var fieldPairs = entityObject.FieldPairs;
            //File.WriteAllBytes("C:/Users/Public/Desktop/test.csv", mybyte);
            var bulkLoader = new MySqlBulkLoader(_mySqlConnection)
            {
                TableName = entityObject.Name,
                CharacterSet = "UTF8",
                NumberOfLinesToSkip = 1,
                FieldTerminator = ",",
                FieldQuotationCharacter = '"',
                FieldQuotationOptional = true,
                Local = true,
                SourceStream = ms
            };
            var bitPropertys = entityObject.Properties.Where(s => s.PropertyType == typeof(bool)).ToList();
            //int i = 1;
            //foreach (var item in bitPropertys)
            //{
            //    fieldPairs[item.Name] = $"@var{i}";
            //    bulkLoader.Expressions.Add($"{item.Name} = CAST(CONV(@var{i}, 2, 10) AS UNSIGNED)");
            //    i++;
            //}

            int i = 1;
            foreach (var item in bitPropertys)
            {
                fieldPairs[item.Name] = $"@var{i}";
                bulkLoader.Expressions.Add($"{item.Name} = CAST(CONV(@var{i}, 2, 10) AS UNSIGNED)");
                i++;
            }
            var datetimePropertyType = entityObject.Properties.Where(s => s.PropertyType == typeof(DateTime?)).ToList();
            foreach (var item in datetimePropertyType)
            {
                fieldPairs[item.Name] = $"@var{i}";
                bulkLoader.Expressions.Add($"{item.Name} = if(LENGTH(@var{i})=0,null,@var{i})");
                i++;
            }
            bulkLoader.Columns.AddRange(fieldPairs.Values);
            //bulkLoader.Expressions.Add("IsUsed = 0");
            //bulkLoader.Expressions.Add("Type = 2");
            await bulkLoader.LoadAsync();
        }
    }

}
