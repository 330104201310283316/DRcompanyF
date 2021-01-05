using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DR.MongoDB;
using DR.Services;
using DR.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DR.WebApi
{
    /// <summary>
    /// 业务逻辑层注入
    /// </summary>
    public static class ApiCoreServiceExtensions
    {
        /// <summary>
        /// 配置 业务逻辑层注入 Startup 辅助类
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApiCoreService(this IServiceCollection services)
        {
            /*
            *  业务逻辑层注入
            */
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<ISendService, SendService>();
            services.AddScoped<DBRequestLogs>();
            return services;
        }
    }
}
