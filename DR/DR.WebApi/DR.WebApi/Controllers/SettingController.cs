using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DR.EFCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DR.Models;
using DR.Configs;

namespace DR.WebApi.Controllers
{
    /// <summary>
    /// 配置文件
    /// </summary>
    [Route("api/dr/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private IHttpContextAccessor _httpContext;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="httpContext"></param>
        public SettingController(IHttpContextAccessor httpContext)
        {
            this._httpContext = httpContext;
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Settings")]
        public IActionResult Settings()
        {
            EFCoreContextWrite context = new EFCoreContextWrite();
            var settings = context.Settings.ToList();
            return Ok(new ApiResponse(settings)); ;
        }

        /// <summary>
        /// 设置配置文件
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateSettings")]
        public IActionResult UpdateSettings(UpdateSettingsDto body)
        {
            EFCoreContextWrite context = new EFCoreContextWrite();
            var settings = context.Settings.Single(x => x.Id == body.Id);
            settings.ConfigKey = body.ConfigKey;
            settings.ConfigValues = body.ConfigValues;
            context.SaveChanges();
            Configs.Configs.Init();
            return Ok(new ApiResponse()); ;
        }
    }
}