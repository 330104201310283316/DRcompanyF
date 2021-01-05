using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// 测试mongodb
    /// </summary>
    public class RequestLogs : ModelBase
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UID { get; set; }
        /// <summary>
        /// 调了哪个接口
        /// </summary>
        public string ApiName { get; set; }
        /// <summary>
        /// 请求来源
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 请求头
        /// </summary>
        public string Headers { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string QueryString { get; set; }
        /// <summary>
        /// StatusCode
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 请求反回结果
        /// </summary>
        public string Result { get; set; }
    }
}
