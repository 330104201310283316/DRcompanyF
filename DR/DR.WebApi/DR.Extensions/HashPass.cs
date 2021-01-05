using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DR.Extensions
{
    /// <summary>
    /// 加密工具
    /// </summary>
    public static class HashPass
    {
        /// <summary>
        /// SHA,SHA1,MD5,SHA256,SHA256Managed,SHA-256,SHA384,SHA384Managed,SHA512,SHA512Managed,
        /// 新加密类
        /// </summary>
        /// <param name="inputString">需要加密的字符串</param>
        /// <param name="hashName">加密方式</param>
        /// <returns></returns>
        public static string HashString(string inputString, string hashName)
        {
            HashAlgorithm algorithm = HashAlgorithm.Create(hashName);
            if (algorithm == null)
            {
                throw new ArgumentException("Unrecognized hash name", "hashName");
            }
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// 文件加密工具
        /// </summary>
        /// <param name="fileStream">文件</param>
        /// <returns></returns>
        public static string HashFileMd5(Stream fileStream)
        {
            using (var md5 = MD5.Create())
            {

                byte[] bytes = md5.ComputeHash(fileStream);
                return BitConverter.ToString(md5.ComputeHash(bytes)).Replace("-", "").ToLower();
            }
        }
    }
}
