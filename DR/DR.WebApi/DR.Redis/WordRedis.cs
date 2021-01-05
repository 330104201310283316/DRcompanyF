using DR.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Redis
{
    public class WordRedis
    {
        public static bool GetAll(out List<WordInfo> Word)
        {
            Word = new List<WordInfo>();
            try
            {
                Word = RedisHelper.Get<List<WordInfo>>("WordAll");
                if (Word != null && Word.Count > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool SaveAll(List<WordInfo> Word)
        {
            try
            {
                RedisHelper.Set("WordAll", Word);
                RedisHelper.Expire("WordAll", 3 * 60 * 60);
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
                RedisHelper.Del("WordAll");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
