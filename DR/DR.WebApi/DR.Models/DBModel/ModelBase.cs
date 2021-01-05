using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// 数据库表base模型
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// 表主键
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Description("Id")]
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Description("最后修改时间")]
        public DateTime LastModifiedTime { get; set; }
        /// <summary>
        /// 是否禁用（假删除）
        /// </summary>
        [Description("是否禁用（假删除）")]
        [Required]
        public bool Disable { get; set; }
        public ModelBase()
        {
            CreateTime = DateTime.UtcNow.AddHours(8);
            LastModifiedTime = DateTime.UtcNow.AddHours(8);
            Disable = false;
        }
    }
}
