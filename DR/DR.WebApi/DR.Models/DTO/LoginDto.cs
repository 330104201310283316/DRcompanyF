using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// 注册
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [RegularExpression(@"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", ErrorMessage = "请输入正确的邮箱")]
        public string Email { get; set; }
    }

    /// <summary>
    /// 永久账号注册
    /// </summary>
    public class RegisterFreeDto
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [RegularExpression(@"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", ErrorMessage = "请输入正确的邮箱")]
        public string Email { get; set; }
        /// <summary>
        ///  用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
    }
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
    }
    /// <summary>
    /// 删除永久账号
    /// </summary>
    public class FreeDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
    }


    /// <summary>
    /// 用户列表
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
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
        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }

}
