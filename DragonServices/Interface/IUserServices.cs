using Domain;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public interface IUserServices:IService
    {
        User User(string userID);

        bool AddUser(User user);
    }
}
