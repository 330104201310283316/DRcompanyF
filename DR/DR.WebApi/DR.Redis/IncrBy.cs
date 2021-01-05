using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Redis
{
    public static class IncrBy
    {
        public static long GetSequenceID(long tick)
        {
            long id = RedisHelper.IncrBy(tick.ToString());
            RedisHelper.Expire(tick.ToString(), 3);
            return id;
        }
    }
}
