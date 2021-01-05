using System;
using System.Collections.Generic;
using System.Text;
using DR.Models;
using Newtonsoft.Json;

namespace DR.Models
{
    /// <summary>
    /// 返回json包装工具
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// data
        /// </summary>
        public object Data { get; }
        /// <summary>
        /// 条数
        /// </summary>
        public int Total { get; }
        /// <summary>
        /// 报错码
        /// </summary>
        public int Code { get; }
        /// <summary>
        /// 报错信息
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// 返回格式封装
        /// </summary>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <param name="code"></param>
        public ApiResponse(object data = null, int total = 0, CodeAndMessage code = CodeAndMessage.Success)
        {
            if (data != null)
                Data = data;
            else
                Data = new { };
            Total = total;
            Code = (int)code;
            Message = code.ToString();
        }
    }
}
