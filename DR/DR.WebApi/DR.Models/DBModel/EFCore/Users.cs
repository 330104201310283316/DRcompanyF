using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class Users : ModelBase
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        ///密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        ///登录类型（0:限制账号，1:不限制账号）
        /// </summary>
        public LoginType LoginType { get; set; }
        /// <summary>
        ///用户权限
        /// </summary>
        public AuthRole AuthRole { get; set; }
    }
}
