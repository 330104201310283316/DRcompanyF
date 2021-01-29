using DR.EFCore;
using DR.Extensions;
using DR.Models;
using DR.Redis;
using DR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DR.WebApi.Controllers
{
    /// <summary>
    /// word文档API
    /// </summary>
    [Route("api/dr/[controller]")]
    [ApiController]
    public class UpLoadWordController : ControllerBase
    {
        private ITestService _TestService;
        private IConfiguration _configuration;
        private IHttpContextAccessor _httpContext;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="TestService"></param>
        /// <param name="configuration"></param>
        /// <param name="httpContext"></param>
        public UpLoadWordController(ITestService TestService, IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            this._TestService = TestService;
            this._configuration = configuration;
            this._httpContext = httpContext;
        }


        /// <summary>
        /// Word文档上传
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("WordsUpLoad")]
        [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult WordsUpLoad(WordDto body)
        {

            string token = _httpContext.HttpContext.Request.Headers["Authorization"];
            AuthRedis.GetUserByToken(token, out UserInfo userInfo);
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            if (body.Eid == null)
            {
                int count = context.WordInfo.Where(x => x.PictureID == body.id).Count();
                if (count > 0)
                    return Ok(new ApiResponse(code: CodeAndMessage.已存在对应的资讯文档));
                WordInfo WordInfos = new WordInfo()
                {
                    Id = SequenceID.GetSequenceID(),
                    CreateTime = DateTime.Now,
                    Disable = false,
                    HtmlContent = body.HtmlContent,
                    PictureID = body.id,
                    LastModifiedTime = DateTime.Now,
                    HtmlExplain = body.HtmlExplain,
                    HtmlTitle = body.HtmlTitle
                };
                context.Add(WordInfos);
                context.SaveChanges();
                WordRedis.Del();
            }
            else
            {
                var WordInfo = context.WordInfo.Single(x => x.Id == body.Eid);
                WordInfo.LastModifiedTime = DateTime.Now;
                WordInfo.HtmlContent = body.HtmlContent;
                WordInfo.HtmlExplain = body.HtmlExplain;
                WordInfo.HtmlTitle = body.HtmlTitle;
                context.SaveChanges();
                WordRedis.Del();
            }
            return Ok(new ApiResponse());

        }
        /// <summary>
        /// Word列表
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("WordInfo")]
        [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult WordInfo(WordSelectDto body)
        {
            int total = 0;
            List<WordListDto> wordListDtos = new List<WordListDto>();
            if (!WordRedis.GetAll(out List<WordInfo> Word))
            {
                using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
                Word = context.WordInfo.Where(x => x.Disable == false).Include(x => x.PictureInfos).Include(x => x.PictureInfos.Users).ToList();
                total = Word.Count();
                if (Word != null && Word.Count > 0)
                    WordRedis.SaveAll(Word);
                if (body.StartTime != null)
                {
                    Word = Word.Where(x => x.CreateTime >= body.StartTime).ToList();
                }
                if (body.EndTime != null)
                {
                    Word = Word.Where(x => x.CreateTime <= body.EndTime).ToList();
                }

                Word = Word.OrderByDescending(x => x.CreateTime).Skip(body.pageSize * (body.pageNum - 1)).Take(body.pageSize).ToList();

                foreach (var item in Word)
                {
                    WordListDto WordInfos = new WordListDto()
                    {
                        ID = item.Id,
                        CreateTime = item.CreateTime,
                        HtmlContent = item.HtmlContent,
                        HtmlExplain = item.HtmlExplain,
                        UserName = item.PictureInfos.Users.UserName,
                        HtmlTitle = item.HtmlTitle,
                        PictureTitle = item.PictureInfos.PictureTitle
                    };
                    wordListDtos.Add(WordInfos);
                }
                return Ok(new ApiResponse(wordListDtos, total));
            }
            else
            {
                total = Word.Count();
                Word = Word.OrderByDescending(x => x.CreateTime).Skip(body.pageSize * (body.pageNum - 1)).Take(body.pageSize).ToList();

                foreach (var item in Word)
                {
                    WordListDto WordInfos = new WordListDto()
                    {
                        ID = item.Id,
                        CreateTime = item.CreateTime,
                        HtmlContent = item.HtmlContent,
                        HtmlExplain = item.HtmlExplain,
                        UserName = item.PictureInfos.Users.UserName,
                        HtmlTitle = item.HtmlTitle,
                        PictureTitle = item.PictureInfos.PictureTitle
                    };
                    wordListDtos.Add(WordInfos);
                }
                return Ok(new ApiResponse(wordListDtos, total));
            }

        }

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("WordDetails")]
        [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult WordDetails(long ID)
        {
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            var wordList = context.WordInfo.Single(x => x.Disable == false && x.Id == ID);
            WordLessDto WordInfos = new WordLessDto()
            {
                CreateTime = wordList.CreateTime,
                HtmlContent = wordList.HtmlContent,
                HtmlExplain = wordList.HtmlExplain,
                HtmlTitle = wordList.HtmlTitle
            };
            return Ok(new ApiResponse(WordInfos));
        }
        /// <summary>
        /// 删除文档内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("WordDel")]
        [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult WordDel(long id)
        {
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            var wordinfo = context.WordInfo.Single(x => x.Id == id);
            wordinfo.Disable = true;
            context.SaveChanges();
            return Ok(new ApiResponse());
        }


        /// <summary>
        /// 文件上传服务
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFileAsync")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            if (file.Length == 0)
                return Ok(new ApiResponse(code: CodeAndMessage.UnKnownError));
            string SuffixName = System.IO.Path.GetExtension(file.FileName);
            string fileName = DateTime.Now.Ticks.ToString() + "_ID1" + SuffixName;
            FastDFSProvider fastDFSProvider = new FastDFSProvider();
            string url = await fastDFSProvider.StoreObjectStreamAsync(file.OpenReadStream(), fileName);
            return Ok(new ApiResponse(new { url }));
        }

        /// <summary>
        /// 多文件上传服务
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFileMore")]
        public async Task<IActionResult> UploadFileMore(List<IFormFile> files)
        {
            if (files.Count == 0)
                return Ok(new ApiResponse(code: CodeAndMessage.UnKnownError));
            Dictionary<string, string> UrlList = new Dictionary<string, string>();
            int i = 0;
            foreach (var item in files)
            {
                string SuffixName = System.IO.Path.GetExtension(item.FileName);
                string fileName = DateTime.Now.Ticks.ToString() + "_ID1" + SuffixName;
                FastDFSProvider fastDFSProvider = new FastDFSProvider();
                string url = await fastDFSProvider.StoreObjectStreamAsync(item.OpenReadStream(), fileName);
                UrlList.Add("File" + i++, url);
            }
            return Ok(new ApiResponse(UrlList));
        }
    }
}
