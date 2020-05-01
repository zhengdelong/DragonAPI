using System;
using Kogel.Dapper.Extension.Attributes;

namespace Domain
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class User : AggregateRoot
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Identity(IsIncrease = false)]
        [Display(Rename = "ID")]
        public string UserID { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        public DateTime CreateTime { get; set; }
        public UserEnum Type { get; set; }

        public int ClassId { get; set; }
        public decimal Money { get; set; }
        public bool IsUsed { get; set; }

    }

    public enum UserEnum
    {
        nomal = 0,
        admin = 1,
        user = 2

    }
}
