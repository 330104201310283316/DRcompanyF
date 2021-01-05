using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DR.WebApi
{
    /// <summary>
    /// 跨域
    /// </summary>
    public static class ApiCorsExtensions
    {
        /// <summary>
        /// 配置 api Cors 跨域 Startup 辅助类
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApiCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("api", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });


            return services;
        }
    }
}
