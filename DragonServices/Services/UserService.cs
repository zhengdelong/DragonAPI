using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Domain;
using Microsoft.Extensions.Logging;
using Infrastructure.Encrypt;
using Infrastructure;

namespace Services
{
    public class UserService : IUserServices
    {
        public readonly IUserRepositories _userRepositories;
        public readonly IUnitOfWork _unitOfWork;
        public readonly ILogger _ILogger;
        public UserService(IUserRepositories userRepositories, IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _userRepositories = userRepositories;
            _unitOfWork = unitOfWork;
            _ILogger = logger;
        }

        public User User(string userID)
        {
            return _userRepositories.Find(userID);
        }
        public bool AddUser(User user)
        {
            _ILogger.LogError("test");
            LinkedList<User> users = new LinkedList<User>();
            for (int i = 0; i < 100000; i++)
            {
                User user1 = new User();
                user1.UserID = SequentialGuidGenerator.Instance.Create().ToString();
                user1.ClassId = i;
                user1.CreateTime = DateTime.Now;
                user1.Money = i;
                user1.PassWord =MD5Encrypt.MD5Encrypt32(user.UserName+i);
                user1.Type = 1;
                user1.UserName = $"test{i}";
                users.AddLast(user1);
            }

            _unitOfWork.BulkInsert(users);
            return _unitOfWork.Commit();
        }
    }
}
