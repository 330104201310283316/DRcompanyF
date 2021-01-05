using System;
using System.Collections.Generic;
using System.Text;
using DR.Models;

namespace DR.MongoDB
{
    public class DBRequestLogs : MongoDBBaseService<RequestLogs>
    {
        public DBRequestLogs() : base(nameof(RequestLogs))
        {

        }
    }
}
