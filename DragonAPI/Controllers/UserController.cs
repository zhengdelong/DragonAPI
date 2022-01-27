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
        [HttpPost("addUser")]
        public async Task<ActionResult> AddUser(User user)
        {
            if (user == null)
            {
                return new BadRequestObjectResult(user);
            }
            var res = await _userServices.AddUser(user);
            if (res)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestObjectResult(user);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("userPageResult")]
        public PageList<User> UserPageResult(int pageSize, int pageIndex, string userName)
        {
            return _userServices.UserPageList(pageSize, pageIndex, userName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("getUser")]
        public User GetUser(string userid)
        {
            return _userServices.User(userid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("updateUser")]
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