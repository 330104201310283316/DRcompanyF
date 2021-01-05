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
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="TestService"></param>
        /// <param name="httpContext"></param>
        /// <param name="sendService"></param>
        public LoginController(ITestService TestService, IHttpContextAccessor httpContext, ISendService sendService)
        {
            this._TestService = TestService;
            this._httpContext = httpContext;
            this._SendService = sendService;
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
        
                using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
                Users users = new Users()
                {
                    Id = SequenceID.GetSequenceID(),
                    AuthRole = AuthRole.User,
                    CreateTime = DateTime.Now,
                    Disable = false,
                    Email = body.Email,
                    LastModifiedTime = DateTime.Now,
                    LoginType = LoginType.LimitWeb,
                    UserName = SequenceID.GetSequenceID().ToString(),
                    PassWord = HashPass.HashString("123456", "MD5")
                };
                context.Add(users);
                context.SaveChanges();
                _SendService.SendEmail(body.Email, users.UserName, "123456");
                return Ok(new ApiResponse(code: CodeAndMessage.注册成功));

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
                using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
                int count = context.User.Where(x => x.UserName == body.UserName).Count();
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
                context.Add(users);
                context.SaveChanges();
                return Ok(new ApiResponse(code: CodeAndMessage.注册成功));
           
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
                using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
                var User = context.User.Single(x => x.Id == body.ID && x.LoginType== LoginType.FreeWeb);
                User.Disable = true;
                context.SaveChanges();
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
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            var User = context.User.Where(x => x.UserName == body.UserName && x.PassWord == HashPass.HashString(body.PassWord, "MD5"));
            int Usercount = User.Count();
            if (Usercount > 0)
            {
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
                AuthRole AuthRole = userInfo.AuthRole.First();
                switch (AuthRole)
                {
                    case Models.AuthRole.Admin:
                        AuthRedis.GetUserById(userInfo.id);
                        AuthRedis.SetToken(userInfo, token, LoginType.FreeWeb);
                        break;
                    case Models.AuthRole.User:
                        AuthRedis.GetUserById(userInfo.id);
                        AuthRedis.SetToken(userInfo, token, LoginType.LimitWeb);
                        break;
                    default:
                        break;
                }
                return Ok(new ApiResponse(new { token, AuthRole }));
            }
            else
            {
                return Ok(new ApiResponse(code: CodeAndMessage.用户名或密码错误));
            }


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
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            var user = context.User.Where(x => x.Disable == false && x.AuthRole == AuthRole.User).ToList();
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
            Dictionary<string, string> dicColumns = new Dictionary<string, string>();
            dicColumns.Add("Email", "邮箱");
            dicColumns.Add("UserName", "用户名");
            dicColumns.Add("LoginType", "临时");
            dicColumns.Add("CreateTime", "创建时间");
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            var user = context.User.Where(x => x.Disable == false && x.AuthRole == AuthRole.User).ToList();
            if (user.Count <= 0)
            {
                return null;
            }
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("用户列表");//名称自定义
            IRow cellsColumn = null;
            IRow cellsData = null;
            //获取实体属性名
            PropertyInfo[] properties = user[0].GetType().GetProperties();
            int cellsIndex = 0;
            //标题
            if (!string.IsNullOrEmpty(title))
            {
                ICellStyle style = workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center;
                //设置字体
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "微软雅黑";
                style.SetFont(font);

                IRow cellsTitle = sheet.CreateRow(0);
                cellsTitle.CreateCell(0).SetCellValue(title);
                cellsTitle.RowStyle = style;
                //合并单元格
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 0, dicColumns.Count - 1));
                cellsIndex = 2;
            }
            //列名
            cellsColumn = sheet.CreateRow(cellsIndex);
            int index = 0;
            Dictionary<string, int> columns = new Dictionary<string, int>();
            foreach (var item in dicColumns)
            {
                cellsColumn.CreateCell(index).SetCellValue(item.Value);
                columns.Add(item.Value, index);
                index++;
            }
            cellsIndex += 1;
            //数据
            foreach (var item in user)
            {
                cellsData = sheet.CreateRow(cellsIndex);
                for (int i = 0; i < properties.Length; i++)
                {
                    if (!dicColumns.ContainsKey(properties[i].Name)) continue;
                    //这里可以也根据数据类型做不同的赋值，也可以根据不同的格式参考上面的ICellStyle设置不同的样式
                    object[] entityValues = new object[properties.Length];
                    entityValues[i] = properties[i].GetValue(item);
                    //获取对应列下标
                    index = columns[dicColumns[properties[i].Name]];
                    cellsData.CreateCell(index).SetCellValue(entityValues[i].ToString());
                }
                cellsIndex++;
            }

            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.GetBuffer();
                ms.Close();
            }

            string path = "用户" + DateTime.Now.ToString("yyyy-MM-dd-HH-m") + ".xlsx";
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        path);

        }
    }
}
