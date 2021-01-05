using System;
using System.Collections.Generic;
using System.Text;
using DR.Models;

namespace DR.Redis
{
    public class AuthRedis
    {
        /// <summary>
        /// 设置token
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="token">Token</param>
        /// <param name="loginType">类型</param>
        /// <returns></returns>
        public static bool SetToken(UserInfo userInfo, string token, LoginType loginType)
        {
            RedisHelper.Set($"UserToken_{loginType}_{token}", userInfo);
            RedisHelper.Expire($"UserToken_{loginType}_{token}", 2  * 60 * 60);//2个小时过期单位是秒
            RedisHelper.Set($"UserInfo_{userInfo.id}", token);
            RedisHelper.Expire($"UserInfo_{userInfo.id}", 2 * 60 * 60);//2个小时过期单位是秒
            return true;
        }

        public static void GetUserById(long id)
        {
            string token = RedisHelper.Get<string>($"UserInfo_{id}");
            if (!string.IsNullOrEmpty(token))
                Del(LoginType.LimitWeb, token, id);
        }

        public static bool GetUserByToken(string token, out UserInfo userInfo)
        {
            userInfo = null;

            Type t = typeof(LoginType);
            Array arrays = Enum.GetValues(t);

            foreach (var item in arrays)
            {
                LoginType type = (LoginType)item;
                userInfo = RedisHelper.Get<UserInfo>($"UserToken_{type}_{token}");
                if (userInfo != null)
                    return true;
            }
            return false;
        }
        public static bool Del(LoginType type, string token, long ID)
        {
            try
            {
                RedisHelper.Del($"UserToken_{type}_{token}");
                RedisHelper.Del($"UserInfo_{ID}");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
