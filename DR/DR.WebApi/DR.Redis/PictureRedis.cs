using DR.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Redis
{
   public class PictureRedis
    {
        public static bool GetAll(out List<PictureInfo> Picture)
        {
            Picture = new List<PictureInfo>();
            try
            {
                Picture = RedisHelper.Get<List<PictureInfo>>("PictureAll");
                if (Picture != null && Picture.Count > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool SaveAll(List<PictureInfo> Picture)
        {
            try
            {
                RedisHelper.Set("PictureAll", Picture);
                RedisHelper.Expire("PictureAll", 3 * 60 * 60);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool Del()
        {
            try
            {
                RedisHelper.Del("PictureAll");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
