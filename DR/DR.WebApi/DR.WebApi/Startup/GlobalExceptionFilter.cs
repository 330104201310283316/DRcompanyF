using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DR.Extensions;
using DR.Models;
using DR.MongoDB;
using DR.Redis;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace DR.WebApi
{
    public class LogsFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            RequestLogs logs = new RequestLogs()
            {
                Id = SequenceID.GetSequenceID(),
                ApiName = context.HttpContext.Request.GetEncodedUrl(),
                IP = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                Headers = JsonConvert.SerializeObject(context.HttpContext.Request.Headers),
                QueryString = context.HttpContext.Request.QueryString.Value,
                StatusCode = context.HttpContext.Response.StatusCode,
                Result = JsonConvert.SerializeObject(context.Result)
            };
            string token = context.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token))
                if (AuthRedis.GetUserByToken(token, out UserInfo userInfo))
                    logs.UID = userInfo.id;
            DBRequestLogs _logs = new DBRequestLogs();
            _logs.Create(logs);
        }
    }
}