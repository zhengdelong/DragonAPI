using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public interface IUserServices
    {
        User User(string userID);
    }
}
