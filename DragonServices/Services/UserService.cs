using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using System.Linq;

namespace Services
{
    public class UserService:IUserServices
    {
        public readonly IUserRepositories _userRepositories;
        public readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepositories userRepositories, IUnitOfWork unitOfWork)
        {
            _userRepositories = userRepositories;
            _unitOfWork = unitOfWork;
        }

        public User User(string userID)
        {
            return _userRepositories.Find(userID);
        }

        public bool AddUser(User user)
        {
            user.UserID = Guid.NewGuid().ToString();
            _unitOfWork.Add(user);
            return _unitOfWork.Commit();
        }
    }
}
