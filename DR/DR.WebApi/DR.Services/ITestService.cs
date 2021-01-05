using System;
using System.Collections.Generic;
using System.Drawing;
using DR.Models;

namespace DR.Services
{
    public interface ITestService
    {
        List<Test> GetAll();
        Test Add(CreateUpdateTestDto body);
        Bitmap GetQRCode(string url, int pixel);
        bool moreAdd(int i);
    }
}
