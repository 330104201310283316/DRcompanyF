using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DR.Services
{
    /// <summary>
    /// Excel操作
    /// </summary>
    public class ExcelService:IExcelService
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
        public byte[] ExportExcel<T>(List<T> Model,string title,string sheetName, Dictionary<string, string> dicColumns)
        {
            if (Model.Count <= 0)
            {
                return null;
            }
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);//名称自定义
            IRow cellsColumn = null;
            IRow cellsData = null;
            //获取实体属性名
            PropertyInfo[] properties = Model[0].GetType().GetProperties();
            int cellsIndex = 0;
            //标题
            if (!string.IsNullOrEmpty(title))
            {
                ICellStyle style = workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center;
                //设置字体
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "微软雅黑";
                style.SetFont(font);

                IRow cellsTitle = sheet.CreateRow(0);
                cellsTitle.CreateCell(0).SetCellValue(title);
                cellsTitle.RowStyle = style;
                //合并单元格
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 0, dicColumns.Count - 1));
                cellsIndex = 2;
            }
            //列名
            cellsColumn = sheet.CreateRow(cellsIndex);
            int index = 0;
            Dictionary<string, int> columns = new Dictionary<string, int>();
            foreach (var item in dicColumns)
            {
                cellsColumn.CreateCell(index).SetCellValue(item.Value);
                columns.Add(item.Value, index);
                index++;
            }
            cellsIndex += 1;
            //数据
            foreach (var item in Model)
            {
                cellsData = sheet.CreateRow(cellsIndex);
                for (int i = 0; i < properties.Length; i++)
                {
                    if (!dicColumns.ContainsKey(properties[i].Name)) continue;
                    //这里可以也根据数据类型做不同的赋值，也可以根据不同的格式参考上面的ICellStyle设置不同的样式
                    object[] entityValues = new object[properties.Length];
                    entityValues[i] = properties[i].GetValue(item);
                    //获取对应列下标
                    index = columns[dicColumns[properties[i].Name]];
                    cellsData.CreateCell(index).SetCellValue(entityValues[i].ToString());
                }
                cellsIndex++;
            }

            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.GetBuffer();
                ms.Close();
            }
            return buffer;

        }
    }
}
