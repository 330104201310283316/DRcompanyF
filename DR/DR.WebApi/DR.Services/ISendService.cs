using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Services
{
    /// <summary>
    /// 发送
    /// </summary>
    public interface ISendService
    {
        /// <summary>
        /// 邮箱发送
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        bool SendEmail(string Email, string UserName, string PassWord);
    }
}
