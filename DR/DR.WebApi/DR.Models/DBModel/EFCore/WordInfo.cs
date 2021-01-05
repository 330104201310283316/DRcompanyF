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
        [ForeignKey("UserID")]
        [Required]
        public Users Users { get; set; }
        /// <summary>
        /// 管理员ID
        /// </summary>
        public long UserID { get; set; }
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
        /// 文章类型
        /// </summary>
        public ArticleType ArticleType { get; set; }
    }
}
