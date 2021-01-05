using System;
using System.Collections.Generic;
using System.Text;
using DR.Models;

namespace DR.Redis
{
    public class TestRedis
    {
        public static bool GetAll(out List<Test> tests)
        {
            tests = new List<Test>();
            try
            {
                tests = RedisHelper.Get<List<Test>>("TestAll");
                if (tests != null && tests.Count > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool SaveAll(List<Test> tests)
        {
            try
            {
                RedisHelper.Set("TestAll", tests);
                RedisHelper.Expire("TestAll", 3 * 60 * 60);
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
                RedisHelper.Del("TestAll");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
