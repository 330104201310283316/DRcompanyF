using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DR.WebApi.Controllers
{
    /// <summary>
    /// consul服务发现健康检查
    /// </summary>
    [Route("api/dr/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private IConfiguration _IConfiguration;
        /// <summary>
        /// 健康发现
        /// </summary>
        /// <param name="configuration"></param>
        public HealthController(IConfiguration configuration)
        {
            this._IConfiguration = configuration;
        }
        /// <summary>
        /// 心跳
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Console.WriteLine($"This Is HealthController {this._IConfiguration["port"]} Invoke");
            return Ok();
        }
    }
}
