using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using System.Linq;

namespace Services
{
    public class UserService
    {
        public readonly IUserRepositories _userRepositories;
        public readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepositories userRepositories,IUnitOfWork unitOfWork) 
        {
            _userRepositories = userRepositories;
            _unitOfWork = unitOfWork;
        }

        public User User(string userID) 
        {
            return _userRepositories.Find(userID);
        }

        public void AddUser(User user) 
        {
            _unitOfWork.Add(user);
            _unitOfWork.Commit();
        }
    }
}
