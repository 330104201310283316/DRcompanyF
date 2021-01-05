using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DR.Models;
using Consul;
using Microsoft.Extensions.Configuration;

namespace DR.WebApi
{
    /// <summary>
    /// 服务发现帮助类
    /// </summary>
    public static class ConsulHelper
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <param name="configuration"></param>
        public static void ConsulRegist(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri(Configs.Configs.Consul.ConsulUrl);
                c.Datacenter = "DR";
            });

            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);

            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = $"service{ip}:{port}",
                Name = "DR",
                Address = ip,
                Port = port,
                Tags = new string[] { weight.ToString() },
                //健康检查
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(Configs.Configs.Consul.ConsulInterval),//间隔多少秒检查一次服务健康状态
                    HTTP = $"http://{ip}:{port}/api/dr/Health/Index",
                    Timeout = TimeSpan.FromSeconds(5),//等待多久（超时）
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(Configs.Configs.Consul.ConsulTimeout)//失败后多久移除
                }
            });
            Console.WriteLine($"{ip}:{port}-*-weight:{weight}");
        }
    }
}
