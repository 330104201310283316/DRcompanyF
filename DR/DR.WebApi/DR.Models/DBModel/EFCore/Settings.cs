using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class Settings : ModelBase
    {
        /// <summary>
        /// key
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ConfigKey { get; set; }
        /// <summary>
        /// values
        /// </summary>
        [Required]
        public string ConfigValues { get; set; }
    }
}