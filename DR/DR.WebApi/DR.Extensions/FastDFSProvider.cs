using FastDFS.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DR.Extensions
{
    public class FastDFSProvider
    {
        /// <summary>
        /// 提供程序名称
        /// </summary>
        public string ProviderName => "FastDFS";
        const string StorageLink = "https://file.dtgty.com/";
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fastDFSConfig">注入配置数据</param>
        public FastDFSProvider()
        {
            var trackerIPs = new List<IPEndPoint>();
            trackerIPs.Add(new IPEndPoint(IPAddress.Parse("114.215.136.245"), 22122));
            ConnectionManager.Initialize(trackerIPs);
        }

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="objectStream">文件流</param>
        /// <param name="objectName">对象名</param>
        /// <returns>文件路径</returns>
        public async Task<string> StoreObjectStreamAsync(Stream objectStream, string objectName)
        {
            StorageNode storageNode = FastDFSClient.GetStorageNodeAsync("group1").GetAwaiter().GetResult();
            var filePath = await FastDFSClient.UploadFileAsync(storageNode, objectStream, Path.GetExtension(objectName));
            return StorageLink + storageNode.GroupName + "/" + filePath;
        }

        private static void SyncTest()
        {
            StorageNode storageNode = FastDFSClient.GetStorageNodeAsync("group1").GetAwaiter().GetResult();
            string[] files = Directory.GetFiles("testimage", "*.jpg");
            string[] strArrays = files;
            for (int i = 0; i < strArrays.Length; i++)
            {
                string str1 = strArrays[i];
                var fileStream = new FileStream(str1, FileMode.Open);
                var binaryReader = new BinaryReader(fileStream);
                byte[] numArray;
                try
                {
                    numArray = binaryReader.ReadBytes((int)fileStream.Length);
                }
                finally
                {
                    binaryReader.Dispose();
                }
                var str = FastDFSClient.UploadFileAsync(storageNode, numArray, "jpg").GetAwaiter().GetResult();
                Console.WriteLine(StorageLink + str);
                FastDFSClient.RemoveFileAsync("group1", str).GetAwaiter().GetResult(); ;
                Console.WriteLine("FastDFSClient.RemoveFile" + str);
            }
        }


    }
}
