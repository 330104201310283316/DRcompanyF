using DR.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        ///登录类型（0:限制账号，1:不限制账号）
        /// </summary>
        public List<LoginType> LoginType { get; set; }
        /// <summary>
        ///用户权限
        /// </summary>
        public List<AuthRole> AuthRole { get; set; }
        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
