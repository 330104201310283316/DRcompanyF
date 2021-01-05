using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Configs
{
    /// <summary>
    /// consul配置文件
    /// </summary>
    public class ConsulSettings
    {

        /// <summary>
        /// consul地址
        /// </summary>
        public string ConsulUrl { get; set; }
        /// <summary>
        /// 心跳周期（单位秒）
        /// </summary>
        public int ConsulInterval { get; set; }
        /// <summary>
        /// 超时移除（单位秒）
        /// </summary>
        public int ConsulTimeout { get; set; }
    }
}
