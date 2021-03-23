using Domain;
using Infrastructure;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Repositories
{
    public class UserRepositorie : DapperBase<User>, IUserRepositories, IBaseRepository
    {
        public UserRepositorie(MySqlConnection connection) : base(connection)
        {

        }
    }
}
