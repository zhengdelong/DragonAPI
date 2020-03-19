using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastMember;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MySql.Data.MySqlClient;

namespace Repositories.EntityFrameworkCore
{
    public static class BulkOpration
    {
        public static BulkModel GetBulkModels<T>(this DbContext dbContext, IEnumerable<T> dataList)
            where T : IAggregateRoot
        {
            var entityType = dbContext.Model.FindEntityType(typeof(T));
            if (entityType == null)
                throw new InvalidOperationException("DbContext does not contain EntitySet for Type: " + typeof(T).Name);
            var tableName = entityType.GetDefaultTableName();
            var properties = entityType.GetProperties().ToList();
            properties = properties.Where(s => !s.IsShadowProperty()&& s.GetValueConverter() == null).ToList();
            var sqlBuilder = new StringBuilder();
            var cloumNameList = properties.Select(s => new { columnName =s.GetColumnName(),s.Name}).ToList();
            string cloumnames = string.Join(',', cloumNameList.Select(s=>s.columnName));
            sqlBuilder.Append("INSERT INTO " + tableName + " (" + cloumnames + ") VALUES ");

            int flag = 0;
            var access = TypeAccessor.Create(typeof(T));
            LinkedList<MySqlParameter> mySqlParameters = new LinkedList<MySqlParameter>();

            foreach (var data in dataList)
            {
                var paramterList = new string[properties.Count()];

                for (int i = 0; i < cloumNameList.Count; i++)
                {
                    var property = cloumNameList[i].Name;
                    var value = access[data, property];
                    MySqlParameter mySqlParameter = new MySqlParameter($"@{cloumNameList[i].columnName + flag}", value);
                    mySqlParameters.AddLast(mySqlParameter);
                    paramterList[i] = ($"@{cloumNameList[i].columnName + flag}");
                }

                sqlBuilder.Append($"({string.Join(',', paramterList)}),");
                flag++;
            }

            var sql = sqlBuilder.ToString().TrimEnd(',');
            return new BulkModel(sql, mySqlParameters);
        }
    }
}
