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
                    RecommendIndex = body.Index
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
                        PictureUrl = item.PictureContent
                    };
                    PictureListDtos.Add(PictureListDto);
                }
            }
            else
            {
                total = Picture.Count();
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
                        PictureUrl = item.PictureContent
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
                UserName = PictureList.Users.UserName
            };
            return Ok(new ApiResponse(PictureDto));
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
            PictureInfo.Disable = true;
            context.SaveChanges();
            return Ok(new ApiResponse());
        }
    }
}
