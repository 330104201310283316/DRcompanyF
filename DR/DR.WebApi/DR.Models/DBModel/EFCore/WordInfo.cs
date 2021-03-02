using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// 上传文章详情表
    /// </summary>
    public class WordInfo : ModelBase
    {
        /// <summary>
        /// 用户表，做联级查询
        /// </summary>
        [ForeignKey("PictureID")]
        [Required]
        public PictureInfo PictureInfos { get; set; }
        /// <summary>
        /// 图片ID
        /// </summary>
        public long? PictureID { get; set; }
        /// <summary>
        /// 文章html内容
        /// </summary>
        [Required]
        public string HtmlContent { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        [Required]
        public string HtmlTitle { get; set; }
        /// <summary>
        /// 文章说明
        /// </summary>
        [Required]
        public string HtmlExplain { get; set; }
        /// <summary>
        /// 附加文件只能上传pdf
        /// </summary>
        public string AttachedPath { get; set; }
    }
}
