using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DR.Models;
using DR.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DR.WebApi
{
    /// <summary>
    /// 自定义授权拦截器
    /// </summary>
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// 请求验证，当前验证部分不要抛出异常，ExceptionFilter不会处理
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(token))
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);

            if (!AuthRedis.GetUserByToken(token, out UserInfo userInfo))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
            else
            {
                //注册账号时间不能超过俩个小时
                if (DateTime.Now.Hour - userInfo.CreateTime.Hour > 2 && userInfo.LoginType.First() !=LoginType.FreeWeb)
                    context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }

        }
    }
}
