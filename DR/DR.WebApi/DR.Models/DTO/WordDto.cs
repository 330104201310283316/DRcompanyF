using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class WordDto
    {
        /// <summary>
        /// Eid
        /// </summary>
        public long? Eid { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public long? id { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string HtmlTitle { get; set; }
        /// <summary>
        /// 文章说明
        /// </summary>
        public string HtmlExplain { get; set; }
        /// <summary>
        /// 文本内容
        /// </summary>
        public string HtmlContent { get; set; }
    }
    /// <summary>
    /// word修改
    /// </summary>
    public class WordUpdateDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 文本内容
        /// </summary>
        public string HtmlContent { get; set; }
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    public class WordSelectDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }


        /// <summary>
        /// 条数
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "pageNum必须是大于等于1的整数")]
        public int pageNum { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "pageSize必须是大于0的整数")]
        public int pageSize { get; set; }
    }

    /// <summary>
    /// 文档列表
    /// </summary>
    public class WordListDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 添加人姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 文章html内容
        /// </summary>
        public string HtmlContent { get; set; }
        /// <summary>
        /// 资讯标题
        /// </summary>
        public string PictureTitle { get; set; }
        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string HtmlTitle { get; set; }
        /// <summary>
        /// 文章说明
        /// </summary>
        public string HtmlExplain { get; set; }
    }
    /// <summary>
    /// 文档详情
    /// </summary>
    public class WordLessDto
    {

        /// <summary>
        /// 文章html内容
        /// </summary>
        public string HtmlContent { get; set; }
        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string HtmlTitle { get; set; }
        /// <summary>
        /// 文章说明
        /// </summary>
        public string HtmlExplain { get; set; }
    }
    /// <summary>
    /// 图片列表
    /// </summary>
    public class PictureDto
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string url { get; set; }
        /// <summary>
        ///标题
        /// </summary>
        public string PictureTitle { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string PictureExplain { get; set; }
        /// <summary>
        /// 推荐指数
        /// </summary>
        public RecommendIndex Index { get; set; }
        /// <summary>
        /// 图片分类
        /// </summary>
        public PictureType PictureType { get; set; }
        /// <summary>
        /// 产品图片分类
        /// </summary>
        public PhotoType PhotoType { get; set; }
    }
    /// <summary>
    /// 图片查询
    /// </summary>
    public class PictureSelectDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public PictureType? PictureType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }


        /// <summary>
        /// 条数
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "pageNum必须是大于等于1的整数")]
        public int pageNum { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "pageSize必须是大于0的整数")]
        public int pageSize { get; set; }
    }
    /// <summary>
    /// 图片查询
    /// </summary>
    public class PicturePcDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public PictureType? PictureType { get; set; }
        /// <summary>
        /// 条数
        /// </summary>
        public int pageNum { get; set; } = 4;
        /// <summary>
        /// 页数
        /// </summary>
        public int pageSize { get; set; } = 1;
    }
    /// <summary>
    /// 图片上传
    /// </summary>
    public class PictureListDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 添加人姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string PictureUrl { get; set; }
        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 图片标题
        /// </summary>
        public string PictureTitle { get; set; }
        /// <summary>
        /// 图片说明
        /// </summary>
        public string PictureExplain { get; set; }
        /// <summary>
        /// 推荐指数
        /// </summary>
        public RecommendIndex Index { get; set; }
        /// <summary>
        /// 图片类型
        /// </summary>
        public PictureType PictureType { get; set; }

    }
    /// <summary>
    /// 图片详情
    /// </summary>
    public class PictureLessDto
    {
        /// <summary>
        /// 添加人姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string PictureUrl { get; set; }
        /// <summary>
        /// 图片标题
        /// </summary>
        public string PictureTitle { get; set; }
        /// <summary>
        /// 图片说明
        /// </summary>
        public string PictureExplain { get; set; }
        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 推荐指数
        /// </summary>
        public RecommendIndex Index { get; set; }
        /// <summary>
        /// 图片类型
        /// </summary>
        public PictureType PictureType { get; set; }
    }
    /// <summary>
    /// 文档详情
    /// </summary>
    public class WordInfoDto
    {

        /// <summary>
        /// 文章html内容
        /// </summary>
        public string HtmlContent { get; set; }
        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string HtmlTitle { get; set; }
        /// <summary>
        /// 文章说明
        /// </summary>
        public string HtmlExplain { get; set; }
    }

}
