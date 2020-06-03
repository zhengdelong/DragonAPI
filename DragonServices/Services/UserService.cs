using System;
using System.Collections.Generic;
using Domain;
using Microsoft.Extensions.Logging;
using Infrastructure.Encrypt;
using Infrastructure;
using Kogel.Dapper.Extension.Model;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
            return _userRepositories.QueryEntity(s => s.UserID == userID);
        }
        public async Task<int> AddUser(User user)
        {
            _ILogger.LogError("test");
            LinkedList<User> users = new LinkedList<User>();
            for (int i = 0; i < 50; i++)
            {
                var flag = false;
                if (i > 20)
                {
                    flag = true;
                }
                User user1 = new User
                {
                    UserID = SequentialGuidGenerator.Instance.Create().ToString(),
                    ClassId = 12,
                    CreateTime = DateTime.Now,
                    Money = 23.32M,
                    PassWord = MD5Encrypt.MD5Encrypt32(user.UserName + i),
                    Type = UserEnum.user,
                    UserName = $"test{i}",
                    IsUsed = flag
                };
                users.AddLast(user1);
            }
            await _unitOfWork.BulkInsert(users);
            _unitOfWork.Commit();
            return 1;
        }


        public PageList<User> UserPageList(int pageSize, int pageIndex, string userName)
        {
            //Expression<Func<User, bool>> expression = s => true;
            //if (!string.IsNullOrEmpty(userName))
            //{
            //    expression = expression.And(s => s.UserName == userName);
            //}
            var res = _userRepositories.QuerySet().Where(s => true);
            if (!string.IsNullOrEmpty(userName))
            {
                res = res.Where(s => s.UserName == userName);
            }

            return res.OrderBy(s => s.UserName).PageList(pageIndex, pageSize);
        }

        public bool UpdateUser(string userID, string name)
        {
            _unitOfWork.Update<User>(s => s.UserID == userID, u => new Domain.User() { UserName = name, CreateTime = DateTime.Now, Type = UserEnum.admin, IsUsed = false });
            return _unitOfWork.Commit();
        }
    }
}
