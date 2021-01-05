using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DR.EFCore;
using DR.Extensions;
using DR.Models;
using DR.MongoDB;
using DR.Redis;
using DR.Services;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using Serilog;

namespace DR.WebApi.Controllers
{
    /// <summary>
    /// test
    /// </summary>
    [Route("api/dr/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private ITestService _TestService;
        private IConfiguration _configuration;
        private IHttpContextAccessor _httpContext;
        private IBackgroundJobClient _backgroundJobs;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="TestService"></param>
        /// <param name="backgroundJobs"></param>
        /// <param name="configuration"></param>
        /// <param name="httpContext"></param>
        public TestController(ITestService TestService, IBackgroundJobClient backgroundJobs, IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            this._TestService = TestService;
            this._configuration = configuration;
            this._httpContext = httpContext;
            this._backgroundJobs = backgroundJobs;
        }

        /// <summary>
        /// test
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Test")]
        public IActionResult Test()
        {
            string port = _configuration["port"];
            return Ok(new ApiResponse(new { port }));
        }
        /// <summary>
        /// 测试hangfire
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Testhangfire")]
        public IActionResult Testhangfire()
        {
            hangFireText text = new hangFireText();
            _backgroundJobs.Enqueue(() => text.moreAdd(10000));
                return Ok(new ApiResponse(true));
        }
        /// <summary>
        /// 测试接入其他库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TestOther")]
        public IActionResult TestOther()
        {
            EFCoreContextDR contextdr = new EFCoreContextDR();
            var WordInfo = contextdr.WordInfo.First(x => x.Id != 0);
            return Ok(new ApiResponse(new { WordInfo.Id }));
        }


        /// <summary>
        /// 测试efcore和Redis
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TestEFCoreAndRedis")]
        public IActionResult TestEFCoreAndRedis()
        {

            List<Test> tests = _TestService.GetAll();
            return Ok(new ApiResponse(tests));
        }

        /// <summary>
        /// 测试efcore和Redis
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("TestEFCoreAndRedisAdd")]
        public IActionResult TestEFCoreAndRedisAdd([FromBody] CreateUpdateTestDto body)
        {

            Test test = _TestService.Add(body);

            return Ok(new ApiResponse(test));
        }

        /// <summary>
        /// 测试登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("TestLogin")]
        public IActionResult TestLogin()
        {
            try
            {
                using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();

                UserInfo userInfo = new UserInfo()
                {
                    id = 1,
                    Email = "Test",
                    AuthRole = new List<AuthRole>() { AuthRole.User }
                };
                string token = Guid.NewGuid().ToString();
                AuthRedis.SetToken(userInfo, token, LoginType.LimitWeb);
                return Ok(new ApiResponse(new { token }));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// 测试身份授权拦截器
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TestAuth")]
        [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult TestAuth()
        {
            string token = _httpContext.HttpContext.Request.Headers["Authorization"];
            if (AuthRedis.GetUserByToken(token, out UserInfo userInfo))
            {
                return Ok(new ApiResponse(userInfo));
            }

            return Ok(new ApiResponse(code: CodeAndMessage.UnKnownError));

        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="url">存储内容</param>
        /// <param name="pixel">像素大小</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetQRCode")]
        // [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public void GetQRCode(string url, int pixel)
        {
            Response.ContentType = "image/jpeg";
            var bitmap = _TestService.GetQRCode(url, pixel);
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);
            Response.Body.WriteAsync(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length));
            Response.Body.Close();
        }
    }
}
