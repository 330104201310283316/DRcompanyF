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
        private IEFBaseService _baseService;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="TestService"></param>
        /// <param name="backgroundJobs"></param>
        /// <param name="configuration"></param>
        /// <param name="httpContext"></param>
        /// <param name="efbaseservice"></param>
        public TestController(ITestService TestService, IBackgroundJobClient backgroundJobs, IConfiguration configuration, IHttpContextAccessor httpContext, IEFBaseService efbaseservice)
        {
            this._TestService = TestService;
            this._configuration = configuration;
            this._httpContext = httpContext;
            this._backgroundJobs = backgroundJobs;
            this._baseService = efbaseservice;
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
            List<Test> tests = _baseService.GetListWriteBy<Test>(x => x.Disable == false);
            return Ok(new ApiResponse(tests));
        }
        /// <summary>
        /// 测试efcore修改
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TestEFCoreModify")]
        public IActionResult TestEFCoreModify()
        {
            //假删除
            var tests = _baseService.GetWriteBy<Test>(x => x.Disable == false);
            tests.Disable = true;
            int test = _baseService.ModifyNo(tests);
            if (test <= 0)
                return Ok(new ApiResponse(code: CodeAndMessage.修改失败));
            return Ok(new ApiResponse(test));
        }

        /// <summary>
        /// 测试efcore和Redis
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("TestEFCoreAndRedisAdd")]
        public IActionResult TestEFCoreAndRedisAdd([FromBody] CreateUpdateTestDto body)
        {
            Test test = new Test()
            {
                Id = SequenceID.GetSequenceID(),
                Name = body.TestName
            };
            var tests = _baseService.Add(test);
            if (tests <= 0)
                return Ok(new ApiResponse(code: CodeAndMessage.新增失败));
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
        /// <summary>
        /// 获取MongoDB数据
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Skip"></param>
        /// <param name="limit"></param>
        [HttpGet]
        [Route("GetMongoDB")]
        // [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult GetMongoDB(DateTime StartTime, DateTime EndTime, int Skip, int limit)
        {
            DBRequestLogs _logs = new DBRequestLogs();
            var list = _logs.Get(StartTime, EndTime, Skip, limit).GroupBy(x => x.ApiName.Split("?")[0]);
            Dictionary<string, Dictionary<DateTime, int>> Time = new Dictionary<string, Dictionary<DateTime, int>>();

            foreach (var item in list)
            {
                foreach (var Citem in item.GroupBy(x => x.CreateTime.Day))
                {
                    TimeDto timeDto = new TimeDto() { DateTime = Citem.Key, Count = Citem.Count() };
                    List<TimeDto> ChangeTiem = new List<TimeDto>();
                    ChangeTiem.Add(timeDto);
                    Time.Add(item.Key+ Citem.Key, SequenceID.CompletionTime(StartTime, EndTime, ChangeTiem));
                }
            }
            return Ok(new ApiResponse(Time, Time.Count()));

        }



    }
}
