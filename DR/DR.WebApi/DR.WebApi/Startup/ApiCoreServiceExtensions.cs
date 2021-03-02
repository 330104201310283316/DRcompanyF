using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DR.MongoDB;
using DR.Services;
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
            *  业务逻辑层服务注入
            */
            //services.AddTransient<ITestService, TestService>();Transient(瞬时的)每次请求时都会创建的瞬时生命周期服务。这个生命周期最适合轻量级，无状态的服务。
            //services.AddScoped<ITestService, TestService>();在同作用域,服务每个请求只创建一次。
            //services.AddSingleton<ITestService, TestService>();全局只创建一次,第一次被请求的时候被创建,然后就一直使用这一个.



            services.AddScoped<ITestService, TestService>();
            services.AddScoped<ISendService, SendService>();
            services.AddScoped<IEFBaseService, EFBaseService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<DBRequestLogs>();
            return services;
        }
    }
}
