using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Services
{
    public class SendService:ISendService
    {
        /// <summary>
        /// 邮箱发送
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public bool SendEmail(string Email, string UserName, string PassWord)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "smtp.163.com";//使用163的SMTP服务器发送邮件
            client.UseDefaultCredentials = true;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("15824405586", "BTHSVBTDJRABAUOJ");//163的SMTP服务器需要用163邮箱的用户名和密码作认证，如果没有需要去163申请个,

            System.Net.Mail.MailMessage Message = new System.Net.Mail.MailMessage();
            Message.From = new System.Net.Mail.MailAddress("deng1838376555@163.com");//这里需要注意，163似乎有规定发信人的邮箱地址必须是163的，而且发信人的邮箱用户名必须和上面SMTP服务器认证时的用户名相同

            Message.To.Add(Email);//将邮件发送给QQ邮箱
            Message.Subject = "注册账号密码";
            Message.Body = "account number:" + UserName + ",password：" + PassWord + "";
            Message.SubjectEncoding = System.Text.Encoding.UTF8;
            Message.BodyEncoding = System.Text.Encoding.UTF8;
            Message.Priority = System.Net.Mail.MailPriority.High;
            Message.IsBodyHtml = true;
            client.Send(Message);
            return true;
        }
    }
}
