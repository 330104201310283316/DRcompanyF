using System;
using System.Collections.Generic;
using System.Text;
using DR.Redis;

namespace DR.Extensions
{
    public static class SequenceID
    {
        public static long GetSequenceID()
        {
            try
            {
                long tick = ToTimestamp(DateTime.Now, 13);
                long id = IncrBy.GetSequenceID(tick);
                if (id < 10)
                {
                    tick = Convert.ToInt64(tick.ToString() + "0" + id.ToString());
                }
                else
                {
                    tick = Convert.ToInt64(tick.ToString() + id.ToString());
                }
                return tick;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static long ToTimestamp(DateTime dt, int length)
        {
            int x = 10000000;
            if (length == 13)
            {
                x = 10000;
            }
            return (dt.ToUniversalTime().Ticks - 626311977000000000) / x;
        }


    }
}
