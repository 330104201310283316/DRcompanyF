using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DR.EFCore;
using DR.Models;
using DR.Redis;
using Newtonsoft.Json;

namespace DR.Configs
{
    public static class Configs
    {
        //private static Dictionary<string, string> _Settings;
        public static Dictionary<string, string> Settings
        {
            get
            {
                if (ConfigRedis.Get(out Dictionary<string, string> settings))
                    return settings;
                return GetSettings(); //Redis没有去数据拿
            }
        }

        #region Consul设置

        private static ConsulSettings _ConsulSettings;
        public static ConsulSettings Consul
        {
            get
            {
                if (Settings.TryGetValue("Consul", out string value))
                return JsonConvert.DeserializeObject<ConsulSettings>(value);
                return null;
            }
        }
        public static ConsulSettings NewConsul
        {
            get
            {
                if (Settings.TryGetValue("NewConsul", out string value))
                    return JsonConvert.DeserializeObject<ConsulSettings>(value);
                return null;
            }
        }
        #endregion

        #region ExcelPath
        public static string ExcelPath
        {
            get
            {
                if (Settings.TryGetValue("ExeclPath", out string value))
                    return value;
                return "";
            }
        }
        #endregion

        private static Dictionary<string, string> GetSettings()
        {
            using EFCoreContextWrite context = new EFCoreContextWrite();
            List<Settings> settings = context.Settings.ToList();
            if (settings != null && settings.Count() > 0)
            {
                Dictionary<string, string> _settings = new Dictionary<string, string>();
                foreach (var setting in settings)
                {
                    _settings.Add(setting.ConfigKey, setting.ConfigValues);
                }
                ConfigRedis.Set(_settings);
                return _settings;
            }
            return null;
        }

        public static void Init()
        {
            //ConfigRedis.Del();
            GetSettings();
        }
    }
}
