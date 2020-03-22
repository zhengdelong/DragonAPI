using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FastMember;
using MySql.Data.MySqlClient;

namespace Repositories.EntityFrameworkCore
{
    public static class BulkOpration
    {
        public static BulkModel GetBulkModels<T>(IEnumerable<T> dataList, BulkConfig bulkConfig)
            where T : IAggregateRoot
        {
            var cloumNameList = bulkConfig.ColumMapping;
            var tableName = bulkConfig.TableName;
            string cloumnames = string.Join(',', cloumNameList.Keys);
            var access = bulkConfig.Accessor;
            bulkConfig.Sqlbuilder.Append("INSERT INTO " + tableName + " (" + cloumnames + ") VALUES ");


            LinkedList<MySqlParameter> mySqlParameters = new LinkedList<MySqlParameter>();

            //foreach (var data in dataList)
            //{
            var datas = dataList.ToList();
            for (int j = 0; j < datas.Count - 1; j++)
            {
                var paramterList = new string[bulkConfig.ColumMapping.Count];
                int i = 0;
                foreach (var item in cloumNameList)
                {
                    var property = item.Value;
                    var value = access[datas[j], property];
                    MySqlParameter mySqlParameter = new MySqlParameter($"@{item.Key + j}", value);
                    mySqlParameters.AddLast(mySqlParameter);
                    paramterList[i] = ($"@{item.Key + j}");
                    i++;
                }
                bulkConfig.Sqlbuilder.Append($"({string.Join(',', paramterList)}),");
            }

            //}

            var sql = bulkConfig.Sqlbuilder.ToString().TrimEnd(',');
            bulkConfig.Sqlbuilder.Clear();
            return new BulkModel(sql, mySqlParameters);
        }

        public static BulkConfig BulkConfig<T>(this DbContext dbContext)
        {
            var access = TypeAccessor.Create(typeof(T));
            var entityType = dbContext.Model.FindEntityType(typeof(T));
            if (entityType == null)
                throw new InvalidOperationException("DbContext does not contain EntitySet for Type: " + typeof(T).Name);
            var tableName = entityType.GetDefaultTableName();
            var properties = entityType.GetProperties().ToList();
            properties = properties.Where(s => !s.IsShadowProperty() && s.GetValueConverter() == null).ToList();
            var sqlBuilder = new StringBuilder();
            var cloumNameList = properties.Select(s => new { columnName = s.GetColumnName(), s.Name }).ToDictionary(s => s.columnName, s => s.Name);
            return new BulkConfig() { ColumMapping = cloumNameList, TableName = tableName, Accessor = access, Sqlbuilder = sqlBuilder };
        }
    }
}
