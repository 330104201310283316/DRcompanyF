using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DR.WebApi
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 程序入口
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Warning()//报错等级（详情参见serilog官网或github）
                 .Enrich.FromLogContext()
                 .WriteTo.RollingFile(AppDomain.CurrentDomain.BaseDirectory + "logs/DR.log",//报错文件
                     fileSizeLimitBytes: int.MaxValue,//文件大小
                     shared: true,
                     flushToDiskInterval: TimeSpan.FromSeconds(5))
                 .CreateLogger();

            try
            {
                Log.Warning("Getting the motors running...");
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddCommandLine(args)//支持命令行参数
                    .Build();

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
                 webBuilder.UseSerilog();
             });
    }
}
