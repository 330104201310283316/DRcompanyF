using DR.EFCore;
using DR.Extensions;
using DR.Models;
using DR.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DR.WebApi.Controllers
{
    /// <summary>
    /// 图片Api
    /// </summary>
    [Route("api/dr/[controller]")]
    [ApiController]
    public class PictureUpLoadController : ControllerBase
    {

        private IConfiguration _configuration;
        private IHttpContextAccessor _httpContext;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="httpContext"></param>
        public PictureUpLoadController(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            this._configuration = configuration;
            this._httpContext = httpContext;
        }
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PictureUpload")]
        public IActionResult PictureUpload(PictureDto body)
        {
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            if (!string.IsNullOrEmpty(body.PictureTitle) && !string.IsNullOrEmpty(body.PictureExplain))
            {
                string token = _httpContext.HttpContext.Request.Headers["Authorization"];

                AuthRedis.GetUserByToken(token, out UserInfo userInfo);
                PictureInfo PictureInfos = new PictureInfo()
                {
                    Id = SequenceID.GetSequenceID(),
                    CreateTime = DateTime.Now,
                    Disable = false,
                    PictureContent = body.url,
                    UserID = userInfo.id,
                    LastModifiedTime = DateTime.Now,
                    PictureExplain = body.PictureExplain,
                    PictureTitle = body.PictureTitle,
                    RecommendIndex = body.Index,
                    PictureType = body.PictureType,
                    PhotoType = body.PhotoType
                };
                context.Add(PictureInfos);
                context.SaveChanges();
                PictureRedis.Del();
            }
            return Ok(new ApiResponse());


        }

        /// <summary>
        /// 图片列表
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PictureInfo")]
        [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult PictureInfo(PictureSelectDto body)
        {
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            int total = 0;
            List<PictureListDto> PictureListDtos = new List<PictureListDto>();
            if (!PictureRedis.GetAll(out List<PictureInfo> Picture))
            {
                Picture = context.PictureInfo.Where(x => x.Disable == false).Include(x => x.Users).ToList();
                total = Picture.Count();
                if (Picture != null && Picture.Count > 0)
                    PictureRedis.SaveAll(Picture);

                if (body.StartTime != null)
                {
                    Picture = Picture.Where(x => x.CreateTime >= body.StartTime).ToList();
                }
                if (body.EndTime != null)
                {
                    Picture = Picture.Where(x => x.CreateTime <= body.EndTime).ToList();
                }
                if (body.PictureType != null)
                {
                    Picture = Picture.Where(x => x.PictureType == body.PictureType).ToList();
                }

                Picture = Picture.OrderByDescending(x => x.RecommendIndex).Skip(body.pageSize * (body.pageNum - 1)).Take(body.pageSize).ToList();

                foreach (var item in Picture)
                {
                    PictureListDto PictureListDto = new PictureListDto()
                    {
                        ID = item.Id,
                        CreateTime = item.CreateTime,
                        Index = item.RecommendIndex,
                        UserName = item.Users.UserName,
                        PictureExplain = item.PictureExplain,
                        PictureTitle = item.PictureTitle,
                        PictureUrl = item.PictureContent,
                        PictureType = item.PictureType

                    };
                    PictureListDtos.Add(PictureListDto);
                }
            }
            else
            {
                total = Picture.Count();
                if (body.StartTime != null)
                {
                    Picture = Picture.Where(x => x.CreateTime >= body.StartTime).ToList();
                }
                if (body.EndTime != null)
                {
                    Picture = Picture.Where(x => x.CreateTime <= body.EndTime).ToList();
                }
                if (body.PictureType != null)
                {
                    Picture = Picture.Where(x => x.PictureType == body.PictureType).ToList();
                }

                Picture = Picture.OrderByDescending(x => x.RecommendIndex).Skip(body.pageSize * (body.pageNum - 1)).Take(body.pageSize).ToList();

                foreach (var item in Picture)
                {
                    PictureListDto PictureListDto = new PictureListDto()
                    {
                        ID = item.Id,
                        CreateTime = item.CreateTime,
                        Index = item.RecommendIndex,
                        UserName = item.Users.UserName,
                        PictureExplain = item.PictureExplain,
                        PictureTitle = item.PictureTitle,
                        PictureUrl = item.PictureContent,
                        PictureType = item.PictureType
                    };
                    PictureListDtos.Add(PictureListDto);
                }
            }
            return Ok(new ApiResponse(PictureListDtos, total));
        }

        /// <summary>
        /// 前端图片列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("PictureList")]
        //[AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult PictureList()
        {
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            int total = 0;
            List<PictureListDto> PictureListDtos = new List<PictureListDto>();
            if (!PictureRedis.GetAll(out List<PictureInfo> Picture))
            {
                Picture = context.PictureInfo.Where(x => x.Disable == false).OrderByDescending(x => x.RecommendIndex).Include(x => x.Users).ToList();
                total = Picture.Count();
                if (Picture != null && Picture.Count > 0)
                    PictureRedis.SaveAll(Picture);
                string token = _httpContext.HttpContext.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(token))
                {
                    Picture = Picture.Where(x => x.PictureType == PictureType.News).OrderByDescending(x => x.RecommendIndex).Skip(0).Take(4).ToList();
                }
                else
                {
                    if (!AuthRedis.GetUserByToken(token, out UserInfo userInfo))
                    {
                        Picture = Picture.Where(x => x.PictureType == PictureType.News).OrderByDescending(x => x.RecommendIndex).Skip(0).Take(4).ToList();
                    }
                    else
                    {
                        //注册账号时间不能超过俩个小时
                        if (DateTime.Now.Hour - userInfo.CreateTime.Hour > 2 && userInfo.LoginType.First() != LoginType.FreeWeb)
                            Picture = Picture.Where(x => x.PictureType == PictureType.News).OrderByDescending(x => x.RecommendIndex).Skip(0).Take(4).ToList();
                    }
                    Picture = Picture.Where(x => x.Disable == false && x.PictureType == PictureType.News).ToList();
                }
                foreach (var item in Picture)
                {
                    PictureListDto PictureListDto = new PictureListDto()
                    {
                        ID = item.Id,
                        CreateTime = item.CreateTime,
                        Index = item.RecommendIndex,
                        UserName = item.Users.UserName,
                        PictureExplain = item.PictureExplain,
                        PictureTitle = item.PictureTitle,
                        PictureUrl = item.PictureContent,
                        PictureType = item.PictureType

                    };
                    PictureListDtos.Add(PictureListDto);
                }
            }
            else
            {
                total = Picture.Count();

                string token = _httpContext.HttpContext.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(token))
                {

                    Picture = Picture.Where(x => x.PictureType == PictureType.News).OrderByDescending(x => x.RecommendIndex).Skip(0).Take(4).ToList();
                }
                else
                {
                    if (!AuthRedis.GetUserByToken(token, out UserInfo userInfo))
                    {
                        Picture = Picture.Where(x => x.PictureType == PictureType.News).OrderByDescending(x => x.RecommendIndex).Skip(0).Take(4).ToList();
                    }
                    else
                    {
                        //注册账号时间不能超过俩个小时
                        if (DateTime.Now.Hour - userInfo.CreateTime.Hour > 2 && userInfo.LoginType.First() != LoginType.FreeWeb)
                            Picture = Picture.Where(x => x.PictureType == PictureType.News).OrderByDescending(x => x.RecommendIndex).Skip(0).Take(4).ToList();
                    }
                    Picture = Picture.Where(x => x.Disable == false && x.PictureType == PictureType.News).OrderByDescending(x => x.RecommendIndex).ToList();
                }
                foreach (var item in Picture)
                {
                    PictureListDto PictureListDto = new PictureListDto()
                    {
                        ID = item.Id,
                        CreateTime = item.CreateTime,
                        Index = item.RecommendIndex,
                        UserName = item.Users.UserName,
                        PictureExplain = item.PictureExplain,
                        PictureTitle = item.PictureTitle,
                        PictureUrl = item.PictureContent,
                        PictureType = item.PictureType
                    };
                    PictureListDtos.Add(PictureListDto);
                }
            }
            return Ok(new ApiResponse(PictureListDtos, total));
        }



        /// <summary>
        /// 产品图片类型(YOUTH/ELEGANT)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("PhotoList")]
        //[AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult PhotoList(PhotoType photoType)
        {
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            int total = 0;
            List<PictureListDto> PictureListDtos = new List<PictureListDto>();
            if (!PictureRedis.GetAll(out List<PictureInfo> Picture))
            {
                Picture = context.PictureInfo.Where(x => x.Disable == false).Include(x => x.Users).ToList();
                total = Picture.Count();
                if (Picture != null && Picture.Count > 0)
                    PictureRedis.SaveAll(Picture);
                Picture = Picture.Where(x => x.Disable == false && x.PictureType == PictureType.Product && x.PhotoType == photoType).OrderByDescending(x => x.RecommendIndex).ToList();

                foreach (var item in Picture)
                {
                    PictureListDto PictureListDto = new PictureListDto()
                    {
                        ID = item.Id,
                        CreateTime = item.CreateTime,
                        Index = item.RecommendIndex,
                        UserName = item.Users.UserName,
                        PictureExplain = item.PictureExplain,
                        PictureTitle = item.PictureTitle,
                        PictureUrl = item.PictureContent,
                        PictureType = item.PictureType

                    };
                    PictureListDtos.Add(PictureListDto);
                }
            }
            else
            {
                total = Picture.Count();
                Picture = Picture.Where(x => x.Disable == false && x.PictureType == PictureType.Product && x.PhotoType == photoType).OrderByDescending(x => x.RecommendIndex).ToList();

                foreach (var item in Picture)
                {
                    PictureListDto PictureListDto = new PictureListDto()
                    {
                        ID = item.Id,
                        CreateTime = item.CreateTime,
                        Index = item.RecommendIndex,
                        UserName = item.Users.UserName,
                        PictureExplain = item.PictureExplain,
                        PictureTitle = item.PictureTitle,
                        PictureUrl = item.PictureContent,
                        PictureType = item.PictureType
                    };
                    PictureListDtos.Add(PictureListDto);
                }
            }
            return Ok(new ApiResponse(PictureListDtos, total));
        }
        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PictureDetails")]
        [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult PictureDetails(long ID)
        {
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            var PictureList = context.PictureInfo.Single(x => x.Disable == false && x.Id == ID);

            PictureLessDto PictureDto = new PictureLessDto()
            {
                CreateTime = PictureList.CreateTime,
                PictureUrl = PictureList.PictureContent,
                PictureExplain = PictureList.PictureExplain,
                PictureTitle = PictureList.PictureTitle,
                Index = PictureList.RecommendIndex,
                UserName = PictureList.Users.UserName,
                PictureType = PictureList.PictureType
            };
            return Ok(new ApiResponse(PictureDto));
        }


        /// <summary>
        /// 查看图片资讯文章详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PictureInfo")]
        //[AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult PictureInfo(long ID)
        {
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            var WordLists = context.WordInfo.Single(x => x.Disable == false && x.PictureID == ID);

            WordInfoDto WordList = new WordInfoDto()
            {
                CreateTime = WordLists.CreateTime,
                HtmlContent = WordLists.HtmlContent,
                HtmlExplain = WordLists.HtmlExplain,
                HtmlTitle = WordLists.HtmlTitle,
                AttachedPath = WordLists.AttachedPath
            };
            return Ok(new ApiResponse(WordList));
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>说
        /// <returns></returns>
        [HttpGet]
        [Route("PictureDel")]
        [AuthFilter]//身份认证，不带token或者token错误会被拦截器拦截进不来这个接口
        public IActionResult PictureDel(long id)
        {
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            var PictureInfo = context.PictureInfo.Single(x => x.Id == id);
            var WordInfo = context.WordInfo.Where(x => x.PictureID == id);
            PictureInfo.Disable = true;
            foreach (var item in WordInfo)
            {
                item.Disable = true;
            }
            context.SaveChanges();
            WordRedis.Del();
            PictureRedis.Del();

            return Ok(new ApiResponse());
        }
    }
}
