using Domain;
using Infrastructure;
using Kogel.Dapper.Extension.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserServices : IService
    {
        User User(string userID);

        Task<int> AddUser(User user);

        PageList<User> UserPageList(int pageSize, int pageIndex, string userName);
        bool UpdateUser(string userID, string name);
    }
}
