using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Domain;
using Kogel.Dapper.Extension.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace DragonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        [HttpPost("User")]
        public async Task<int> AddUser(User user) => await _userServices.AddUser(user);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserPageResult")]
        public PageList<User> UserPageResult(int pageSize, int pageIndex, string userName)
        {
            return _userServices.UserPageList(pageSize, pageIndex, userName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("User")]
        public  User User(string userid)
        {
            return _userServices.User(userid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("UpdateUser")]
        public bool UpdateUser(string userID, string name)
        {
            return _userServices.UpdateUser(userID, name);
        }

        [HttpPut("getstring")]
        public string GetString() 
        {
            return "";
        }
    }
}