using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Configuration;

namespace BDTH.Docs
{
    /// <summary>
    /// 服务发现帮助类
    /// </summary>
    public static class ConsulHelper
    {
        public static void ConsulRegist(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri(configuration["Settings:Consul"]);
                c.Datacenter = "dcl";
            });

            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);

            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = $"service{ip}:{port}",
                Name = "Docs",
                Address = ip,
                Port = port,
                Tags = new string[] { weight.ToString() },
                //健康检查
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(int.Parse(configuration["Settings:ConsulInterval"])),//间隔多少秒检查一次服务健康状态
                    HTTP = $"http://{ip}:{port}/Health",
                    Timeout = TimeSpan.FromSeconds(5),//等待多久（超时）
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(int.Parse(configuration["Settings:ConsulTimeout"]))//失败后多久移除
                }
            });
            Console.WriteLine($"{ip}:{port}-*-weight:{weight}");
        }
    }
}
