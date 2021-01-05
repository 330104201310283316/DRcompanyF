using System;
using System.Collections.Generic;
using System.Text;
using DR.Models;

namespace DR.Redis
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class ConfigRedis
    {
        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="Settings"></param>
        /// <returns></returns>
        public static bool Get(out Dictionary<string, string> Settings)
        {
            Settings = RedisHelper.Get<Dictionary<string, string>>("Settings");
            if (Settings != null && Settings.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 设置配置文件
        /// </summary>
        /// <param name="Settings"></param>
        /// <returns></returns>
        public static bool Set(Dictionary<string, string> Settings)
        {
            RedisHelper.Set("Settings", Settings);
            return true;
        }

        /// <summary>
        /// 删除配置文件
        /// </summary>
        /// <returns></returns>
        public static bool Del()
        {
            RedisHelper.Del("Settings");
            return true;
        }
    }
}