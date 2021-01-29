using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// 图片
    /// </summary>
    public class PictureInfo : ModelBase
    {

        /// <summary>
        /// 用户表，做联级查询
        /// </summary>
        [Required]
        [ForeignKey("UserID")]
        public Users Users { get; set; }
        /// <summary>
        /// 管理员ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        [Required]
        public string PictureContent { get; set; }
        /// <summary>
        /// 图片标题
        /// </summary>
        [Required]
        public string PictureTitle { get; set; }
        /// <summary>
        /// 图片说明
        /// </summary>
        [Required]
        public string PictureExplain { get; set; }
        /// <summary>
        ///推荐指数
        /// </summary>
        [Required]
        public RecommendIndex RecommendIndex { get; set; }
        /// <summary>
        ///图片分类
        /// </summary>
        [Required]
        public PictureType PictureType { get; set; }
        /// <summary>
        ///产品图片类型
        /// </summary>
        [Required]
        public PhotoType PhotoType { get; set; }
    }
}
