using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Services
{
   public interface IExcelService
    {
        /// <summary>
        /// 导出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model"></param>
        /// <param name="title"></param>
        /// <param name="sheetName"></param>
        /// <param name="dicColumns"></param>
        /// <returns></returns>
        byte[] ExportExcel<T>(List<T> Model, string title, string sheetName, Dictionary<string, string> dicColumns);
    }
}
