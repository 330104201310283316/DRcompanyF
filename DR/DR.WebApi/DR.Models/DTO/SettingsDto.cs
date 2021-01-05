using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// 修改配置
    /// </summary>
    public class UpdateSettingsDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Key
        /// </summary>
        public string ConfigKey { get; set; }
        /// <summary>
        /// Values
        /// </summary>
        public string ConfigValues { get; set; }
    }
}