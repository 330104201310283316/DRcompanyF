using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Consul;
using DR.WebApi;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DR.WebApi
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 注入方法，读取config文件
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var csredis = new CSRedis.CSRedisClient(Configuration["ConnectionStrings:Redis"]);
            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
        }
        /// <summary>
        /// 获取cofig的路径
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 此方法由运行时调用。使用此方法可将服务添加到容器中。
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加 Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("WriteDB"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero, // 作业队列轮询间隔。默认值为15秒
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));
            //添加hangfire服务器
            services.AddHangfireServer();

            //业务逻辑注入
            services.AddApiCoreService();
            //注入API corers跨域
            services.AddApiCors();
            //注入context
            services.AddHttpContextAccessor();
            services.AddControllers(options =>
            {
                //options.Filters.Add<LogsFilterAttribute>();
            }).AddNewtonsoftJson(options =>
            {
                //// 忽略循环引用
                //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //// 不使用驼峰(属性首字母会小写)
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // 设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                // 如字段为null值，该字段不会返回到前端
                // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
            //Swagger生成器添加到方法中的服务集合中
            services.AddSwaggerDocumentation();

            Log.Warning("Startup done running...");
        }

        /// <summary>
        /// 此方法由运行时调用。使用此方法配置HTTP请求管道。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //使用swagger
            app.UseSwaggerDocumentation();
            //使用跨域
            app.UseCors("api");
            //封装报错信息
            app.UseMiddleware<ErrorWrappingMiddleware>();
            app.UseHttpsRedirection();

            //使用hangfire只能本地访问
            //app.UseHangfireDashboard();
            //控制仪表盘的访问路径和授权配置
            app.UseHangfireDashboard("/hangfire", new Hangfire.DashboardOptions
            {
                Authorization = new[] { new MyDashboardAuthorizationFilter() }
            });
            app.UseRouting();

            app.UseAuthorization();
            //app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseStaticFiles();
           // this.Configuration.ConsulRegist();
        }
    }
}
