using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
     public class BulkModel
    {
        public string Sql { get; set; }

        public LinkedList<MySqlParameter> Parameters { get; set; }

        public BulkModel(string sql, LinkedList<MySqlParameter> parameters) 
        {
            Sql = sql;
            Parameters = parameters;
        }
    }
}
