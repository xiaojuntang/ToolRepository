/***************************************************************************** 
*        filename :ExeclHelper 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   ExeclHelper 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Helper
*        文件名:             ExeclHelper 
*        创建系统时间:       2016/1/28 16:25:53 
*        创建年份:           2016 
/*****************************************************************************/
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Net.Helper
{
    public class ExeclHelper
    {
        /// <summary>
        /// 用于Web导出  将数据转换了Excel报表的扩展：ExcelUtility
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName)
        {
            ExportByWeb(dtSource, null, strHeaderText, strFileName, 3000);
        }
        /// <summary>
        /// 用于Web导出
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="listColumn"> </param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        public static void ExportByWeb(DataTable dtSource, List<Column> listColumn, string strHeaderText, string strFileName)
        {
            ExportByWeb(dtSource, listColumn, strHeaderText, strFileName, 3000);
        }

        /// <summary>
        /// 用于Web导出
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="listColumn"> </param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        /// <param name="defaultColumnWidth">默认列宽度 </param>
        public static void ExportByWeb(DataTable dtSource, List<Column> listColumn, string strHeaderText, string strFileName, int defaultColumnWidth)
        {
            HttpContext curContext = HttpContext.Current;
            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                                             "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));
            curContext.Response.BinaryWrite(Export(dtSource, listColumn, strHeaderText, defaultColumnWidth).GetBuffer());
            //curContext.Response.End();
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="listColumn"> </param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream Export(DataTable dtSource, List<Column> listColumn, string strHeaderText)
        {
            return Export(dtSource, listColumn, strHeaderText, 3000);
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="listColumn"> </param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="defaultColumnWidth">默认列宽度 </param>
        public static MemoryStream Export(DataTable dtSource, List<Column> listColumn, string strHeaderText, int defaultColumnWidth)
        {
            #region 门卫代码
            if (listColumn == null) listColumn = new List<Column>();
            #endregion

            var workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            #region 右击文件 属性信息

            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "云学时代";
                workbook.DocumentSummaryInformation = dsi;
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "云学时代"; //填加xls文件作者信息
                si.ApplicationName = strHeaderText; //填加xls文件创建程序信息
                si.LastAuthor = "云学时代"; //填加xls文件最后保存者信息
                si.Comments = "云学时代"; //填加xls文件作者信息
                si.Title = strHeaderText; //填加xls文件标题信息
                si.Subject = "云学时代" + strHeaderText; //填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }

            #endregion


            #region 标题样式
            ICellStyle titleCellStyle = workbook.CreateCellStyle();
            titleCellStyle.Alignment = HorizontalAlignment.Center;
            titleCellStyle.FillForegroundColor = HSSFColor.BlueGrey.Index;
            titleCellStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SparseDots;
            //titleCellStyle.FillBackgroundColor = HSSFColor.BLUE_GREY.index;
            titleCellStyle.FillBackgroundColor = HSSFColor.BlueGrey.Index;
            IFont titleFont = workbook.CreateFont();
            titleFont.FontHeightInPoints = 18;
            titleFont.Boldweight = 700;
            titleFont.FontName = "黑体";
            titleCellStyle.SetFont(titleFont);
            #endregion

            #region 表头样式
            ICellStyle headCellStyle = workbook.CreateCellStyle();
            headCellStyle.Alignment = HorizontalAlignment.Center;
            //headCellStyle.FillForegroundColor = HSSFColor.LIGHT_YELLOW.index;
            headCellStyle.FillForegroundColor = HSSFColor.LightYellow.Index;
            //headCellStyle.FillPattern = FillPatternType.SPARSE_DOTS;
            headCellStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SparseDots;
            //headCellStyle.FillBackgroundColor = HSSFColor.LIGHT_YELLOW.index;
            headCellStyle.FillBackgroundColor = HSSFColor.LightYellow.Index;
            IFont headfont = workbook.CreateFont();
            headfont.FontHeightInPoints = 10;
            headfont.Boldweight = 700;
            headfont.FontName = "宋体";
            headCellStyle.SetFont(headfont);
            #endregion

            #region 单元格样式
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            IFont cellfont = workbook.CreateFont();
            cellfont.FontHeightInPoints = 10;
            cellfont.Boldweight = 400;
            cellfont.FontName = "宋体";
            cellStyle.SetFont(cellfont);
            #endregion
            #region 日期样式
            ICellStyle dateStyle = workbook.CreateCellStyle();
            //dateStyle.Alignment=HorizontalAlignment.LEFT;
            dateStyle.Alignment = HorizontalAlignment.Left;
            IFont dateFont = workbook.CreateFont();
            dateFont.FontHeightInPoints = 10;
            dateFont.Boldweight = 400;
            dateFont.FontName = "宋体";
            dateStyle.SetFont(dateFont);
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            #endregion
            #region 取得列宽
            //var arrColWidth = new int[dtSource.Columns.Count];
            //foreach (DataColumn item in dtSource.Columns)
            //{
            //    arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName).Length;
            //}

            //for (int i = 0; i < dtSource.Rows.Count; i++)
            //{
            //    for (int j = 0; j < dtSource.Columns.Count; j++)
            //    {
            //        int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
            //        if (intTemp > arrColWidth[j])
            //        {
            //            arrColWidth[j] = intTemp;
            //        }
            //    }
            //}
            for (int i = 0; i < dtSource.Columns.Count; i++)
            {
                var firstOrDefault = listColumn.FirstOrDefault(a => a.Key == dtSource.Columns[i].ColumnName);
                if (firstOrDefault != null)
                {
                    sheet.SetColumnWidth(i, firstOrDefault.Width);
                }
                else
                {
                    sheet.SetColumnWidth(i, defaultColumnWidth);
                }
            }

            #endregion

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
                    IRow titleRow = sheet.CreateRow(0);
                    titleRow.HeightInPoints = 22;
                    titleRow.CreateCell(0).SetCellValue(strHeaderText);
                    titleRow.GetCell(0).CellStyle = titleCellStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                    #endregion

                    #region 列头及样式
                    IRow headerRow = sheet.CreateRow(1);
                    foreach (DataColumn column in dtSource.Columns)
                    {
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                        headerRow.GetCell(column.Ordinal).CellStyle = headCellStyle;
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
                    newCell.CellStyle = cellStyle; //格式化显示
                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime": //日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);
                            newCell.CellStyle = dateStyle; //格式化显示
                            break;
                        case "System.Boolean": //布尔型
                            bool boolV;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //空值处理
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
            //using (var ms = new MemoryStream())
            //{
            //    workbook.Write(ms);
            //    ms.Flush();
            //    ms.Position = 0;
            //    return ms;
            //}
            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;

        }

        /// <summary>
        /// 将DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">数据源</param>
        /// <param name="listColumn">列名称</param>
        /// <param name="tableName">Excel标题名称</param>
        public static void FileExport(DataTable dtSource, List<Column> listColumn, string tableName)
        {
            //List<Column> listColumn = new List<Column>
            //{
            //    new Column{Key = "年级", Width = 5000},
            //     new Column{Key = "时间", Width = 5000},
            //};
            MemoryStream memory = Export(dtSource, listColumn, tableName);
            FileStream fileStream = new FileStream(tableName + ".xls", FileMode.Create, FileAccess.ReadWrite);
            memory.WriteTo(fileStream);
            memory.Close();
            fileStream.Close();
        }
    }

    /// <summary>
    /// 列的宽度
    /// </summary>
    public class Column
    {
        private string _key;
        private int _width;

        /// <summary>
        /// 列的名称
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// 列的宽度
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value > 0 ? value : 0; }
        }
    }
}
