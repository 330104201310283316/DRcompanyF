using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DR.EFCore;
using DR.Extensions;
using DR.Models;
using DR.Redis;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace DR.Services
{
    public class TestService : ITestService
    {
        private static IEFBaseService _EFBaseService;
        public Test Add(CreateUpdateTestDto body)
        {
            Test test = new Test()
            {
                Id = SequenceID.GetSequenceID(),
                Name = body.TestName
            };
            _EFBaseService.Add(test);
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
            Test test = new Test();
            for (int j = 0; j <= i; j++)
            {
                test = new Test()
                {
                    Id = SequenceID.GetSequenceID(),
                    Name = "hangfire"
                };
                _EFBaseService.Add(test);
            }
            TestRedis.Del();
            return true;
        }


        public List<Test> GetAll()
        {
            if (!TestRedis.GetAll(out List<Test> tests))
            {
                tests= _EFBaseService.GetListWriteBy<Test>(u=>u.Disable==false).ToList();
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
