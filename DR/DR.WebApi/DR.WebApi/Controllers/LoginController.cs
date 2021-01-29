using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DR.EFCore;
using DR.Extensions;
using DR.Models;
using DR.MongoDB;
using DR.Redis;
using DR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Serilog;

namespace DR.WebApi.Controllers
{
    /// <summary>
    /// 用户API
    /// </summary>
    [Route("api/dr/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ITestService _TestService;
        private IHttpContextAccessor _httpContext;
        private ISendService _SendService;
        private IEFBaseService _BaseService;
        private IExcelService _excelService;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="TestService"></param>
        /// <param name="httpContext"></param>
        /// <param name="sendService"></param>
        /// <param name="BaseService"></param>
        /// <param name="excelService"></param>
        public LoginController(ITestService TestService, IHttpContextAccessor httpContext, ISendService sendService, IEFBaseService BaseService, IExcelService excelService)
        {
            this._TestService = TestService;
            this._httpContext = httpContext;
            this._SendService = sendService;
            this._BaseService = BaseService;
            this._excelService = excelService;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterDto body)
        {
            var userList = _BaseService.GetListWriteBy<Users>(x => x.Email == body.Email);
            int count = userList.Where(x => x.CreateTime.AddHours(2) > DateTime.Now).Count();
            if (count > 0)
            {
                return Ok(new ApiNResponse(code: CodeAndMessage.重复邮箱在俩小时内注册, message: "Repeat email to register within two hours"));
            }
            int userinfo = userList.Where(x => x.Email == body.Email && x.CreateTime.AddHours(2) < DateTime.Now).Count();
            if (userinfo > 0)
            {
                var userlist = userList.Single(x => x.Email == body.Email);

                userlist.CreateTime = DateTime.Now;
                userlist.Count = userlist.Count + 1;
                _BaseService.ModifyNo(userlist);
                _SendService.SendEmail(body.Email, body.Email, "123456");
            }
            else
            {
                Users users = new Users()
                {
                    Id = SequenceID.GetSequenceID(),
                    AuthRole = AuthRole.User,
                    CreateTime = DateTime.Now,
                    Disable = false,
                    Email = body.Email,
                    LastModifiedTime = DateTime.Now,
                    LoginType = LoginType.LimitWeb,
                    UserName = body.Email.ToString(),
                    ComPany = body.ComPany,
                    NickName = body.NickName,
                    Count = 1,
                    PassWord = HashPass.HashString("123456", "MD5")
                };
                _BaseService.Add(users);
                _SendService.SendEmail(body.Email, users.UserName, "123456");
            }
            return Ok(new ApiResponse());

        }

        /// <summary>
        /// 永久账号注册
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RegisterFree")]
        [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult RegisterFree(RegisterFreeDto body)
        {
            int count = _BaseService.GetListWriteBy<Users>(x => x.UserName == body.UserName).Count();
            if (count > 0)
                return Ok(new ApiResponse(code: CodeAndMessage.用户名重复));
            Users users = new Users()
            {
                Id = SequenceID.GetSequenceID(),
                AuthRole = AuthRole.User,
                CreateTime = DateTime.Now,
                Disable = false,
                Email = body.Email,
                LastModifiedTime = DateTime.Now,
                LoginType = LoginType.FreeWeb,
                UserName = body.UserName,
                PassWord = HashPass.HashString(body.PassWord, "MD5"),
            };
            _BaseService.Add(users);
            return Ok(new ApiResponse());

        }
        /// <summary>
        /// 删除永久账号
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FreeDel")]
        [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult FreeDel(FreeDto body)
        {
            var User = _BaseService.GetWriteBy<Users>(x => x.Id == body.ID && x.LoginType == LoginType.FreeWeb);
            User.Disable = true;
            _BaseService.ModifyNo(User);
            return Ok(new ApiResponse());
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AuthLogin")]
        public IActionResult AuthLogin(LoginDto body)
        {
            var User = _BaseService.GetListWriteBy<Users>(x => x.UserName == body.UserName);

            if (User.Count <= 0)
            {
                return Ok(new ApiNResponse(code: CodeAndMessage.用户名不存在, message: "The user name does not exist"));
            }
            if (User.Where(x => x.UserName == body.UserName && x.PassWord == HashPass.HashString(body.PassWord, "MD5")).Count() <= 0)
                return Ok(new ApiNResponse(code: CodeAndMessage.密码错误, message: "Password error"));

            if (User.Where(x => x.UserName == body.UserName && x.PassWord == HashPass.HashString(body.PassWord, "MD5") && x.CreateTime.AddHours(2) < DateTime.Now && x.LoginType == LoginType.LimitWeb).Count() > 0)
                return Ok(new ApiNResponse(code: CodeAndMessage.注册时间已经超过2小时, message: "The registration time has exceeded 2 hours. Please re-register"));

            UserInfo userInfo = new UserInfo();
            foreach (var item in User)
            {
                userInfo = new UserInfo()
                {
                    id = item.Id,
                    AuthRole = new List<AuthRole>() { item.AuthRole },
                    Email = item.Email,
                    LoginType = new List<LoginType>() { item.LoginType },
                    CreateTime = item.CreateTime
                };
            }
            string token = Guid.NewGuid().ToString();
            AuthRole AuthRoles = userInfo.AuthRole.First();
            switch (AuthRoles)
            {
                case Models.AuthRole.Admin:
                    AuthRedis.GetUserById(userInfo.id, LoginType.FreeWeb);
                    AuthRedis.SetToken(userInfo, token, LoginType.FreeWeb);
                    break;
                case Models.AuthRole.User:
                    AuthRedis.GetUserById(userInfo.id, LoginType.LimitWeb);
                    AuthRedis.SetToken(userInfo, token, LoginType.LimitWeb);
                    break;
                default:
                    break;
            }
            return Ok(new ApiResponse(new { token, AuthRoles }));

        }
        /// <summary>
        /// 用户查询列表
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UsersInfo")]
        [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult UsersInfo(int pageNum, int pageSize, string Email)
        {
            var user = _BaseService.GetListWriteBy<Users>(x => x.Disable == false && x.AuthRole == AuthRole.User).ToList();
            int total = user.Count();
            user = user.OrderByDescending(x => x.CreateTime).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            if (!string.IsNullOrEmpty(Email))
            {
                user = user.Where(x => x.Email == Email).ToList();
            }
            List<UserDto> UserDtos = new List<UserDto>();
            foreach (var item in user)
            {
                UserDto userDto = new UserDto()
                {
                    ID = item.Id,
                    Email = item.Email,
                    AuthRole = item.AuthRole,
                    CreateTime = item.CreateTime,
                    LoginType = item.LoginType,
                    UserName = item.UserName,
                    PassWord = item.PassWord
                };
                UserDtos.Add(userDto);
            }
            return Ok(new ApiResponse(UserDtos, total));

        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportExcel")]
        [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult ExportExcel()
        {
            string title = "用户列表";
            string sheetName = "用户列表";
            Dictionary<string, string> dicColumns = new Dictionary<string, string>();
            dicColumns.Add("Email", "邮箱");
            dicColumns.Add("UserName", "用户名");
            dicColumns.Add("LoginType", "用户类型");
            dicColumns.Add("CreateTime", "创建时间");
            var user = _BaseService.GetListWriteBy<Users>(x => x.Disable == false && x.AuthRole == AuthRole.User).ToList();
            byte[] buffer = _excelService.ExportExcel(user, title, sheetName, dicColumns);
            string path = "用户" + DateTime.Now.ToString("yyyy-MM-dd-HH-m") + ".xlsx";
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        path);

        }
    }
}
