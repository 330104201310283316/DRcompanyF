using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDTH.WebApi;
using DR.Extensions;
using DR.Models;
using DR.MongoDB;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace DR.WebApi
{
    /// <summary>
    /// 报错封装
    /// </summary>
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="next"></param>
        public ErrorWrappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                context.Response.StatusCode = 500;
                StringBuilder bf = new StringBuilder();
                ex.FlattenHierarchy().ToList().ForEach(x => bf.Append(x.Message));
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";

                    var response = new ApiResponse(code: CodeAndMessage.UnKnownError);

                    var json = JsonConvert.SerializeObject(response);

                    await context.Response.WriteAsync(json);
                }
            }
        }
    }
}
