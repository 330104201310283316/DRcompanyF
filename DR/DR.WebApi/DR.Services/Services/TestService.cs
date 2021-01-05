using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DR.EFCore;
using DR.Extensions;
using DR.Models;
using DR.Redis;
using QRCoder;

namespace DR.Services
{
    public class TestService : ITestService
    {
        public Test Add(CreateUpdateTestDto body)
        {
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            Test test = new Test()
            {
                Id = SequenceID.GetSequenceID(),
                Name = body.TestName
            };
            context.Add(test);
            context.SaveChanges();
            TestRedis.Del();
            return test;
        }
        /// <summary>
        /// moreAdd
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool moreAdd(int i)
        {
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            Test test = new Test();
            for (int j=0;j<= i; j++)
            {
                test = new Test()
                {
                    Id = SequenceID.GetSequenceID(),
                    Name = "hangfire"
                };
                context.Add(test);
                context.SaveChanges();
            }
            TestRedis.Del();
            return true;
        }


        public List<Test> GetAll()
        {
            if (!TestRedis.GetAll(out List<Test> tests))
            {
                using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
                tests = context.Tests.Select(x => x).ToList();
                if (tests != null && tests.Count > 0)
                    TestRedis.SaveAll(tests);
            }
            return tests;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">存储内容</param>
        /// <param name="pixel">像素大小</param>
        /// <returns></returns>
        public Bitmap GetQRCode(string url, int pixel)
        {
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData codeData = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M, true);
            QRCoder.QRCode qrcode = new QRCoder.QRCode(codeData);

            Bitmap qrImage = qrcode.GetGraphic(pixel, Color.Black, Color.White, true);

            return qrImage;
        }
    }
}
