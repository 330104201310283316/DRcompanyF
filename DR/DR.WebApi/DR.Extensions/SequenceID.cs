using System;
using System.Collections.Generic;
using System.Text;
using DR.Models;
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

        /// <summary>
        /// 补全时间段的所有天数
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public static Dictionary<DateTime, int> CompletionTime(DateTime StartTime, DateTime EndTime, List<TimeDto> ChangeTime)
        {
            Dictionary<DateTime, int> keyValue = new Dictionary<DateTime, int>();

            int count = 0;
            while (StartTime < EndTime)
            {
                DateTime Time = StartTime;
                foreach (var item in ChangeTime)
                {
                    if (StartTime.Day == item.DateTime)
                    {
                        Time = StartTime;
                        count = item.Count;
                        break;
                    }
                }
                keyValue.Add(Time, count);
                count = 0;
                StartTime = StartTime.AddDays(1);
            }
            return keyValue;
        }

    }
}
