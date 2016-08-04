using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Common.Net.Excel
{
    /// <summary>
    /// Asp.Net导出Excel
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="headerText">表头文本</param>
        /// <Author>逆水寒龙修改自柳永法 2012-07-19 15:32</Author>
        public static void RenderToExcel(DataTable dtSource, string headerText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "百里登风";
                workbook.DocumentSummaryInformation = dsi;
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "百里登风"; //填加xls文件作者信息
                si.ApplicationName = "百里登风"; //填加xls文件创建程序信息
                si.LastAuthor = "craneexporter"; //填加xls文件最后保存者信息
                si.Comments = "百里登风"; //填加xls文件作者信息
                si.Title = "inquiry info of craneexporter"; //填加xls文件标题信息
                si.Subject = "inquiry infolist";//填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                si.RevNumber = "Beta V1.0";
                workbook.SummaryInformation = si;
            }
            #endregion

            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(headerText);
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        CellRangeAddress region = new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1);
                        //Region rg = new Region(0, 0, 0, dtSource.Columns.Count - 1);
                        sheet.AddMergedRegion(region);
                        //headerRow.Dispose();
                    }
                    #endregion

                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(1);
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);

                        }
                        //headerRow.Dispose();
                    }
                    #endregion
                    rowIndex = 2;
                }
                #endregion


                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }
                #endregion

                rowIndex++;
            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", HttpUtility.UrlEncode(headerText + "_" + DateTime.Now.ToString("yyyy-MM-dd"), System.Text.Encoding.UTF8)));
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();
            workbook = null;
            ms.Close();
            ms.Dispose();
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    workbook.Write(ms);
            //    ms.Flush();
            //    ms.Position = 0;
            //    //sheet.Dispose();
            //    //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet
            //    return ms;
            //}
        }

        /// <summary>
        /// Asp.Net导出Excel
        /// </summary>
        /// <typeparam name="T">导出对象</typeparam>
        /// <param name="datas">导出数据</param>
        /// <param name="columnInfo">表头字典</param>
        /// <param name="headerText">表头</param>
        public static void RenderToExcel<T>(List<T> datas, Dictionary<string, string> columnInfo, string headerText)
        {
            MemoryStream ms = new MemoryStream();
            //IWorkbook workbook = new HSSFWorkbook();
            //ISheet sheet = workbook.CreateSheet("导出数据");
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "百里登风";
                workbook.DocumentSummaryInformation = dsi;
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "百里登风"; //填加xls文件作者信息
                si.ApplicationName = "百里登风"; //填加xls文件创建程序信息
                si.LastAuthor = "craneexporter"; //填加xls文件最后保存者信息
                si.Comments = "百里登风"; //填加xls文件作者信息
                si.Title = "inquiry info of craneexporter"; //填加xls文件标题信息
                si.Subject = "inquiry infolist";//填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                si.RevNumber = "Beta V1.0";
                workbook.SummaryInformation = si;
            }
            #endregion

            #region 表头及样式
            {
                IRow headRow = sheet.CreateRow(0);
                headRow.HeightInPoints = 25;
                headRow.CreateCell(0).SetCellValue(headerText);
                ICellStyle headStyle = workbook.CreateCellStyle();
                headStyle.Alignment = HorizontalAlignment.Center;
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 16;
                font.Boldweight = 500;
                headStyle.SetFont(font);
                headRow.GetCell(0).CellStyle = headStyle;
                CellRangeAddress region = new CellRangeAddress(0, 0, 0, columnInfo.Count - 1);
                //Region rg = new Region(0, 0, 0, dtSource.Columns.Count - 1);
                sheet.AddMergedRegion(region);
                //headerRow.Dispose();
            }
            #endregion

            IRow headerRow = sheet.CreateRow(1);
            int rowIndex = 2, piIndex = 0;
            Type type = typeof(T);
            PropertyInfo[] pis = type.GetProperties();
            int pisLen = pis.Length;//减2是多了2个外键引用  
            PropertyInfo pi = null;

            #region 列头及样式
            {
                ICellStyle headStyle2 = workbook.CreateCellStyle();
                headStyle2.Alignment = HorizontalAlignment.Center;
                IFont font2 = workbook.CreateFont();
                font2.FontHeightInPoints = 10;
                font2.Boldweight = 700;
                headStyle2.SetFont(font2);
                while (piIndex < pisLen)
                {
                    pi = pis[piIndex];
                    var displayName = columnInfo[pi.Name];
                    if (!displayName.Equals(string.Empty))
                    {
                        //如果该属性指定了DisplayName，则输出  
                        try
                        {
                            headerRow.CreateCell(piIndex).SetCellValue(displayName);
                            headerRow.GetCell(piIndex).CellStyle = headStyle2;
                        }
                        catch (Exception)
                        {
                            headerRow.CreateCell(piIndex).SetCellValue("");
                        }
                    }
                    piIndex++;
                }
            }
            #endregion

            foreach (T data in datas)
            {
                piIndex = 0;
                IRow dataRow = sheet.CreateRow(rowIndex);
                while (piIndex < pisLen)
                {
                    pi = pis[piIndex];
                    try
                    {
                        dataRow.CreateCell(piIndex).SetCellValue(pi.GetValue(data, null).ToString());
                    }
                    catch (Exception)
                    {
                        dataRow.CreateCell(piIndex).SetCellValue("");
                    }
                    piIndex++;
                }
                rowIndex++;
            }
            workbook.Write(ms);
            var value = string.Format("attachment; filename={0}.xls",
                HttpUtility.UrlEncode(headerText + "_" + DateTime.Now.ToString("yyyy-MM-dd"), Encoding.UTF8));
            HttpContext.Current.Response.AddHeader("Content-Disposition", value);
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();
            workbook = null;
            ms.Close();
            ms.Dispose();
        }

        /// <summary> 
        /// 将一组对象导出成EXCEL 
        /// </summary> 
        /// <typeparam name="T">要导出对象的类型</typeparam> 
        /// <param name="datas">一组对象</param> 
        /// <param name="headerText">导出后的文件名</param> 
        /// <param name="columnInfo">列名信息</param> 
        public static void ExExcel<T>(List<T> datas, Dictionary<string, string> columnInfo, string headerText)
        {
            if (columnInfo.Count == 0) { return; }
            if (datas.Count == 0) { return; }
            //生成EXCEL的HTML 
            string excelStr = "";
            Type myType = datas[0].GetType();
            //根据反射从传递进来的属性名信息得到要显示的属性 
            List<PropertyInfo> myPro = new List<PropertyInfo>();
            foreach (string cName in columnInfo.Keys)
            {
                PropertyInfo p = myType.GetProperty(cName);
                if (p != null)
                {
                    myPro.Add(p);
                    excelStr += columnInfo[cName] + "\t";
                }
            }
            //如果没有找到可用的属性则结束 
            if (myPro.Count == 0) { return; }
            excelStr += "\n";
            foreach (T obj in datas)
            {
                foreach (PropertyInfo p in myPro)
                {
                    excelStr += p.GetValue(obj, null) + "\t";
                }
                excelStr += "\n";
            }
            //输出EXCEL 
            HttpResponse rs = System.Web.HttpContext.Current.Response;
            rs.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            rs.AppendHeader("Content-Disposition", "attachment;filename=" + headerText);
            rs.ContentType = "application/ms-excel";
            rs.Write(excelStr);
            rs.End();
        }
    }
}
