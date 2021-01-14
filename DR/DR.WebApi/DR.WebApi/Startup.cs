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
        /// ע�뷽������ȡconfig�ļ�
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var csredis = new CSRedis.CSRedisClient(Configuration["ConnectionStrings:Redis"]);
            //��ʼ�� RedisHelper
            RedisHelper.Initialization(csredis);
        }
        /// <summary>
        /// ��ȡcofig��·��
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// �˷���������ʱ���á�ʹ�ô˷����ɽ�������ӵ������С�
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // ��� Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("WriteDB"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero, // ��ҵ������ѯ�����Ĭ��ֵΪ15��
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));
            //���hangfire������
            services.AddHangfireServer();

            //ҵ���߼�ע��
            services.AddApiCoreService();
            //ע��API corers����
            services.AddApiCors();
            //ע��context
            services.AddHttpContextAccessor();
            services.AddControllers(options =>
            {
                //options.Filters.Add<LogsFilterAttribute>();
            }).AddNewtonsoftJson(options =>
            {
                //// ����ѭ������
                //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //// ��ʹ���շ�(��������ĸ��Сд)
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // ����ʱ���ʽ
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                // ���ֶ�Ϊnullֵ�����ֶβ��᷵�ص�ǰ��
                // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
            //Swagger��������ӵ������еķ��񼯺���
            services.AddSwaggerDocumentation();

            Log.Warning("Startup done running...");
        }

        /// <summary>
        /// �˷���������ʱ���á�ʹ�ô˷�������HTTP����ܵ���
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //ʹ��swagger
            app.UseSwaggerDocumentation();
            //ʹ�ÿ���
            app.UseCors("api");
            //��װ������Ϣ
            app.UseMiddleware<ErrorWrappingMiddleware>();
            app.UseHttpsRedirection();

            //ʹ��hangfireֻ�ܱ��ط���
            //app.UseHangfireDashboard();
            //�����Ǳ��̵ķ���·������Ȩ����
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
