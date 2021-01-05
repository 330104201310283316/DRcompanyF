using DR.EFCore;
using DR.Models;
using DR.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Extensions
{
   public class hangFireText
    {

        public  bool moreAdd(int i)
        {
            using EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
            Test test = new Test();
            for (int j = 0; j <= i; j++)
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
    }
}
