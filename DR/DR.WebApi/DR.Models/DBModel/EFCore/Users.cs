using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class Users : ModelBase
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        [Required]
        public string ComPany { get; set; }
        /// <summary>
        ///密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        ///注册次数
        /// </summary>
        public int Count { get; set; }
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
