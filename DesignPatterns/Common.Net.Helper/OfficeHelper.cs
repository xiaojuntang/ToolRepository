using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;
using System.Collections;
using System.Reflection;
using NPOI.HSSF.Util;
using NPOI.SS.Util;

namespace Common.Tools
{
    /// <summary>
    /// 导出Excel类
    /// </summary>
    public class OfficeHelper
    {
        public OfficeHelper() { }

        #region ChengWei扩展

        int count = 0;
        ISheet sheet;

        /// <summary>
        /// 初始化Execl类
        /// </summary>
        /// <param name="num"></param>
        /// <param name="name">工作表Sheet名称</param>
        /// <param name="width">列宽</param>
        public OfficeHelper(int num, string name, int width = 15)
        {
            count = num;
            sheet = NpoiStyle.CreateWkSheet(name);
            sheet.DefaultColumnWidth = width;
        }

        /// <summary>
        /// 生成Excel文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Save(string path)
        {
            string result = "Success";
            NpoiStyle.Save(path, ref result);
            return result == "Success";
        }

        /// <summary>
        /// 创建head 黑体 
        /// </summary>
        /// <param name="str">要特殊显示的文字,多个内容用'|'分隔开</param>
        /// <param name="msg">显示的文字</param>
        /// <param name="row">哪一行</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="ha">居中/居左/居右</param>
        public void CreateHeadCell(string str, string msg, int row, short fontSize, HorizontalAlignment ha = HorizontalAlignment.Center)
        {
            var cell = NpoiStyle.MergedRegion(sheet, row, row, 0, count);
            var font = NpoiStyle.GetFont(fontSize, "黑体");
            var font2 = NpoiStyle.GetFont(fontSize, "黑体", 1);
            var style = NpoiStyle.GetCellStyle(font, ha);
            if (!string.IsNullOrEmpty(str))
            {
                var s = str.Split('|');
                var richText = NpoiStyle.RichText(s, msg, font, font2);
                cell.SetCellValue(richText);
            }
            else
            {
                cell.SetCellValue(msg);
            }
            cell.CellStyle = style;
        }

        /// <summary>
        /// 创建title 宋体 居左
        /// </summary>
        /// <param name="str">要特殊显示的文字</param>
        /// <param name="msg">显示的文字</param>
        /// <param name="row">哪一行</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="startCell">从哪列开始合并</param>
        public void CreateTitleCell(string str, string msg, int row, short fontSize, int startCell)
        {
            var cell = NpoiStyle.MergedRegion(sheet, row, row, startCell, count);
            var font = NpoiStyle.GetFont(fontSize, "宋体", 0, false, (short)FontBoldWeight.Bold);
            var font2 = NpoiStyle.GetFont(fontSize, "宋体", 1, false, (short)FontBoldWeight.Bold);
            var style = NpoiStyle.GetCellStyle(font, HorizontalAlignment.Left);
            if (!string.IsNullOrEmpty(str))
            {
                var s = str.Split('|');
                var richText = NpoiStyle.RichText(s, msg, font, font2);
                cell.SetCellValue(richText);
            }
            else
            {
                cell.SetCellValue(msg);
            }
            cell.CellStyle = style;
        }

        /// <summary>
        /// body文字
        /// </summary>
        /// <param name="str">要特殊显示的文字</param>
        /// <param name="msg">显示的文字</param>
        /// <param name="row">哪一行</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="isItalic">是否有下划线</param>
        /// <param name="b">是否有边框</param>
        public void CreateBodyCell(string str, string msg, int row, short fontSize, bool isItalic = false, bool b = false)
        {
            var cell = NpoiStyle.MergedRegion(sheet, row, row, 0, count);
            var font = NpoiStyle.GetFont(fontSize, "宋体");
            var font2 = NpoiStyle.GetFont(fontSize, "宋体", 1, isItalic);
            var style = NpoiStyle.GetCellStyle(font, HorizontalAlignment.Left, b);

            if (!string.IsNullOrEmpty(str))
            {
                var s = str.Split('|');
                var richText = NpoiStyle.RichText(s, msg, font, font2);
                cell.SetCellValue(richText);
            }
            else
            {
                cell.SetCellValue(msg);
            }
            cell.CellStyle = style;
        }
        public void CreateBodyCell2(string str, string msg, int startRow, int endRow, int startCell, int endCell, short fontSize, bool isItalic = false, bool b = false)
        {
            var cell = NpoiStyle.MergedRegion(sheet, startRow, endRow, startCell, endCell);
            var font = NpoiStyle.GetFont(fontSize, "宋体");
            var font2 = NpoiStyle.GetFont(fontSize, "宋体", 1, isItalic);
            var style = NpoiStyle.GetCellStyle(font, HorizontalAlignment.Left, b);

            if (!string.IsNullOrEmpty(str))
            {
                var s = str.Split('|');
                var richText = NpoiStyle.RichText(s, msg, font, font2);
                cell.SetCellValue(richText);
            }
            else
            {
                cell.SetCellValue(msg);
            }

            GetStyle(startRow, endRow, startCell, endCell, style);
        }
        public void CreateBodyCell3(int row, List<string> list, short fontSize)
        {
            for (var i = 0; i < list.Count; i++)
            {
                CreateBodyCell("", list[i], row + i, fontSize);
            }
        }

        /// <summary>
        /// 创建一行
        /// </summary>
        /// <param name="str"></param>
        /// <param name="msg">内容文字</param>
        /// <param name="row">所在行</param>
        /// <param name="startCell">开始列</param>
        /// <param name="endCell">结束列</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="s">字体类型（默认粗体）</param>
        public void CreateTableCell(string msg, int row, int startCell, int endCell, short fontSize, short s = (short)FontBoldWeight.Bold)
        {
            var cell = NpoiStyle.MergedRegion(sheet, row, row, startCell, endCell);
            var font = NpoiStyle.GetFont(fontSize, "宋体", 0, false, s);
            var style = NpoiStyle.GetCellStyle(font, HorizontalAlignment.Center, true);
            cell.SetCellValue(msg);
            GetStyle(row, row, startCell, endCell, style);
        }

        /// <summary>
        /// 创建合并的单元格并赋值
        /// </summary>
        /// <param name="msg">单元格类型</param>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <param name="startCell"></param>
        /// <param name="endCell"></param>
        /// <param name="fontSize">字体</param>
        /// <param name="s">样式</param>
        public void CreateMergedRegionRowCell(string msg, int startRow, int endRow, int startCell, int endCell, short fontSize, short s = (short)FontBoldWeight.Bold)
        {
            var cc = NpoiStyle.MergedRegion(sheet, startRow, endRow, startCell, endCell);
            var font = NpoiStyle.GetFont(fontSize, "宋体", 0, false, s);
            var style = NpoiStyle.GetCellStyle(font, HorizontalAlignment.Center, true);
            var num = 0.0;
            if (double.TryParse(msg, out num))
            {
                cc.SetCellValue(num);
            }
            else
            {
                cc.SetCellValue(msg);
            }
            GetStyle(startRow, endRow, startCell, endCell, style);
        }

        public void CreateTable(List<string> str, int num, short s = (short)FontBoldWeight.Bold)
        {
            if (str[0] != "NoHander")
            {
                CreateTableCell(str[0], num, 0, 1, 12, s);
            }
            CreateTableCell(str[1], num, 2, 4, 12, s);
            CreateTableCell(str[2], num, 5, 7, 12, s);
            CreateTableCell(str[3], num, 8, 10, 12, s);
        }


        public void CreateTable2(List<string> str, int num, short s = (short)FontBoldWeight.Bold)
        {
            if (str[0] != "NoHander")
            {
                CreateTableCell(str[0], num, 0, 1, 12, s);
            }
            CreateTableCell(str[1], num, 2, 10, 12, s);
        }

        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="str">列头名称列表</param>
        /// <param name="row">第几行开始索引</param>
        /// <param name="cell">第几列开始索引</param>
        /// <param name="cellNum"></param>
        /// <param name="s">字体大小</param>
        public void CreateTable3(List<string> str, int row, int cell, int cellNum, short s = (short)FontBoldWeight.Normal)
        {
            var num = cell;
            cellNum = cellNum - 1;
            for (var i = 0; i < str.Count; i++)
            {
                CreateMergedRegionRowCell(str[i], row, row, num, num + cellNum, 12, s);
                num = num + cellNum + 1;
            }
        }
        public void CreateTable4(List<string> str, int row, int cell, int rowNum, short s = (short)FontBoldWeight.Normal)
        {
            //var num = cell;
            rowNum = rowNum - 1;
            for (var i = 0; i < str.Count; i++)
            {
                CreateMergedRegionRowCell(str[i], row + i, row + rowNum + i, cell, cell, 12, s);
                //num = num + rowNum + 1;
            }
        }
        public void CreateTable5(List<string> str, int row, int cell, int rowNum, short s = (short)FontBoldWeight.Normal)
        {
            rowNum = rowNum - 1;
            for (var i = 0; i < str.Count; i++)
            {
                CreateMergedRegionRowCell(str[i], row, row + rowNum, cell + i, cell + i, 12, s);
            }
        }

        /// <summary>
        /// 多行求和 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="indexRow">显示和的行数</param>
        /// <param name="startRow">页面上显示的行数，不是创建时显示的行数（如果操作该行是第2行，那这儿就填3）</param>
        /// <param name="endRow">页面上显示的行数，不是创建时显示的行数</param>
        public void GetRowSum(int count, int indexRow, int startRow, int endRow)
        {
            for (var i = 0; i < count; i++)
            {
                var row = sheet.GetRow(indexRow);
                var cell = row.GetCell(i);
                if (cell.StringCellValue == "") continue;
                cell.SetCellFormula("sum(" + cell.StringCellValue + "" + startRow + ":" + cell.StringCellValue + "" + endRow + ")");
            }
        }

        /// <summary>
        /// 给每行每列设置样式
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <param name="startCell"></param>
        /// <param name="endCell"></param>
        /// <param name="style"></param>
        public void GetStyle(int startRow, int endRow, int startCell, int endCell, ICellStyle style)
        {
            for (var i = startRow; i <= endRow; i++)
            {
                for (var j = startCell; j <= endCell; j++)
                {
                    var r = sheet.GetRow(i);
                    if (r == null)
                    {
                        r = sheet.CreateRow(i);
                    }

                    var c = r.GetCell(j);
                    if (c == null)
                        c = r.CreateCell(j);

                    c.CellStyle = style;
                }
            }
        }

        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="anchor"></param>
        /// <param name="bytes"></param>
        /// <param name="b"></param>
        public void CreatePicture(HSSFClientAnchor anchor, byte[] bytes, bool b)
        {
            NpoiStyle.Picture(sheet, bytes, anchor, b);
        }

        /// <summary>
        /// 导出DataTable到EXCEL
        /// </summary>
        /// <param name="table">DataTable表</param>
        /// <param name="fileName">要保存的excel文件名</param>
        /// <param name="headerName">excel文件中的标题名</param>
        public static string DataTableToExcel(DataTable table, string fileName, string headerName, string path)
        {
            string result = "ok";
            #region   验证可操作性

            #region 对话框属性
            //申明保存对话框    
            //SaveFileDialog dlg = new SaveFileDialog();
            //设置另存为的文件名
            //dlg.FileName = fileName;
            //默然文件后缀    
            //dlg.DefaultExt = "xls ";
            //文件后缀列表    
            //dlg.Filter = "EXCEL FILE(*.XLS)|*.xls ";
            //默然路径是系统当前路径    
            //dlg.InitialDirectory = Directory.GetCurrentDirectory();
            //打开保存对话框    
            //if (dlg.ShowDialog() == DialogResult.Cancel) return;
            //返回文件路径    
            //string fileNameString = dlg.FileName; 
            #endregion


            string fileNameString = path;
            //验证strFileName是否为空或值无效    
            if (string.IsNullOrEmpty(fileNameString)) { return "文件保存路径不能空！"; }

            //如果没设置标题名，就默认是和文件名同名
            if (string.IsNullOrEmpty(headerName))
            {
                headerName = fileName;
            }
            //定义表格内数据的行数和列数
            int rowscount = table.Rows.Count;
            int colscount = table.Columns.Count;
            //行数必须大于0
            if (rowscount <= 0)
            {
                result = "导出数据源行无记录!";
            }

            //列数必须大于0    
            if (colscount <= 0)
            {
                result = "导出数据源列无记录!";
            }

            //行数不可以大于65536    
            if (rowscount > 65536)
            {
                result = "Execl的Sheet最多记录65,536条记录";
            }

            //列数不可以大于255    
            if (colscount > 255)
            {
                result = "数据源列数据不能大于255列";
            }

            FileInfo file = new FileInfo(fileNameString);
            if (file.Exists)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception error)
                {
                    result = error.Message;
                }
            }
            #endregion
            try
            {
                FileStream fs = new FileStream(fileNameString, FileMode.Create, FileAccess.ReadWrite);
                using (table)
                {
                    IWorkbook workbook = new HSSFWorkbook();
                    {
                        ISheet sheet = workbook.CreateSheet();
                        {
                            IRow headerRow = sheet.CreateRow(0);

                            // handling header.
                            foreach (DataColumn column in table.Columns)
                                headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
                            // handling value.
                            int rowIndex = 1;
                            foreach (DataRow row in table.Rows)
                            {
                                IRow dataRow = sheet.CreateRow(rowIndex);

                                foreach (DataColumn column in table.Columns)
                                {
                                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                                }
                                rowIndex++;
                            }
                            workbook.Write(fs);
                            fs.Flush();
                            fs.Close();
                        }
                    }
                }
            }
            catch (Exception error)
            {
                result = error.Message;
            }
            finally
            {

            }
            result = fileNameString + "\n\n数据导出成功! ";
            return result;
        }
        #endregion


        #region Txj扩展
        /// <summary>
        /// Excle导入到数据集
        /// Excle中的工作表对应DataSet中的Table，工作表名和列名分别对应Table中的表名和列名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataSet ExcelToDataSet(string path)
        {
            DataSet ds = new DataSet();
            IWorkbook wb = WorkbookFactory.Create(path);
            for (int sheetIndex = 0; sheetIndex < wb.NumberOfSheets; sheetIndex++)
            {
                ISheet sheet = wb.GetSheetAt(sheetIndex);
                DataTable dt = new DataTable(sheet.SheetName);

                //添加列
                int columnCount = sheet.GetRow(0).PhysicalNumberOfCells;
                for (int i = 0; i < columnCount; i++)
                    dt.Columns.Add(sheet.GetRow(0).GetCell(i).StringCellValue);

                //添加行,从索引为1的行开始
                int rowsCount = sheet.PhysicalNumberOfRows;
                for (int i = 1; i < rowsCount; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                        //dr.SetField(j, sheet.GetRow(i).GetCell(j).StringCellValue);
                        dr.SetField(j, sheet.GetRow(i).GetCell(j).ToString());
                    dt.Rows.Add(dr);
                }
                ds.Tables.Add(dt);
            }
            return ds;
        }

        /// <summary>
        /// 将集合中的数据导入到excle中，不同的集合对应excel中的不同的工作表
        /// </summary>
        /// <param name="lists">不同对象的集合,集合中的对象可以通过设置特性来关联列名</param>
        /// <param name="fileName">保存的文件名，后缀名为.xls或.xlsx</param>
        //public void ListToExcel(IList[] lists, string fileName) {
        //    DataSetToExcel(ConvertToDataSet(lists), fileName);
        //}

        /// <summary>
        /// 将数据集中的数据导入到excel中，多个table对应的导入到excel对应多个工作表
        /// </summary>
        /// <param name="ds">要导出到excle中的数据集，数据集中表名和字段名在excel中对应工作表名和标题名称</param>
        /// <param name="fileName">保存的文件名，后缀名为.xls或.xlsx</param>
        //public void DataSetToExcel(DataSet ds, string fileName) {
        //    if (ds != null) {
        //        IWorkbook wb = CreateSheet(fileName);
        //        foreach (DataTable dt in ds.Tables) {
        //            ImportToWorkbook(dt, ref wb);
        //        }
        //        DownHelper downHelper = new DownHelper();
        //        downHelper.DownloadByOutputStreamBlock(
        //            new MemoryStream(ToByte(wb)), fileName);
        //    }
        //}

        //public void DownFile(string fileName, IWorkbook workbook) {
        //    DownHelper downHelper = new DownHelper();
        //    downHelper.DownloadByOutputStreamBlock(new MemoryStream(ToByte(workbook)), fileName);
        //}

        /// <summary>
        /// 将数据导入到excel中
        /// </summary>
        /// <param name="dt">要导出到excle中的数据表，表名和字段名在excel中对应工作表名和标题名称</param>
        /// <param name="fileName">保存的文件名，后缀名为.xls或.xlsx</param>
        //public void DataTableToExcel(DataTable dt, string fileName) {
        //    IWorkbook wb = CreateSheet(fileName);
        //    ImportToWorkbook(dt, ref wb);
        //    DownHelper downHelper = new DownHelper();
        //    downHelper.DownloadByOutputStreamBlock(
        //        new MemoryStream(ToByte(wb)), fileName);
        //}

        public DataSet ConvertToDataSet(IList[] lists)
        {
            DataSet ds = new DataSet();

            foreach (IList list in lists)
            {
                if (list != null && list.Count > 0)
                {
                    string tableName = list[0].GetType().Name;
                    object[] classInfos = list[0].GetType().
                        GetCustomAttributes(typeof(EntityMappingAttribute), true);

                    if (classInfos.Length > 0)
                        tableName = ((EntityMappingAttribute)classInfos[0]).Name;

                    DataTable dt = new DataTable(tableName);
                    object obj = list[0];
                    PropertyInfo[] propertyInfos = obj.GetType().
                        GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        object[] infos = propertyInfo.
                            GetCustomAttributes(typeof(EntityMappingAttribute), true);
                        if (infos.Length > 0)
                            dt.Columns.Add(((EntityMappingAttribute)infos[0]).Name);
                        else
                            dt.Columns.Add(propertyInfo.Name);
                    }

                    //添加数据
                    for (int i = 0; i < list.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        object objTemp = list[i];
                        PropertyInfo[] propertyInfosTemp = objTemp.GetType().
                            GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        for (int j = 0; j < propertyInfosTemp.Count(); j++)
                        {
                            dr.SetField(j, propertyInfosTemp[j].GetValue(obj, null));
                        }
                        dt.Rows.Add(dr);
                    }

                    ds.Tables.Add(dt);
                }
                else
                {
                    ds.Tables.Add(new DataTable(list.GetType().Name));
                }
            }

            return ds;
        }

        public void ImportToWorkbook(DataTable dt, ref IWorkbook wb)
        {
            string sheetName = dt.TableName ?? "Sheet1";
            //创建工作表
            ISheet sheet = wb.CreateSheet(sheetName);
            //添加标题
            IRow titleRow = sheet.CreateRow(0);
            SetRow(titleRow,
                GetCloumnNames(dt),
                GetCellStyle(sheet.Workbook, FontBoldWeight.Bold));

            //添加数据行
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow dataRow = sheet.CreateRow(i + 1);
                SetRow(
                    dataRow,
                    GetRowValues(dt.Rows[i]),
                    GetCellStyle(sheet.Workbook));
            }

            //设置表格自适应宽度
            AutoSizeColumn(sheet);
        }

        private byte[] ToByte(IWorkbook wb)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //XSSFWorkbook即读取.xlsx文件返回的MemoryStream是关闭
                //但是可以ToArray(),这是NPOI的bug
                wb.Write(ms);
                return ms.ToArray();
            }
        }

        public IWorkbook CreateSheet(string path)
        {
            IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook(); ;
            string extension = System.IO.Path.GetExtension(path).ToLower();
            if (File.Exists(path))
            {
                Stream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                if (extension == ".xls")
                    wb = new HSSFWorkbook(fs);
                else if (extension == ".xlsx")
                    wb = new HSSFWorkbook(fs);
                return wb;
            }
            return null;
        }

        public int GetWidth(DataTable dt, int columnIndex)
        {
            IList<int> lengths = new List<int>();
            foreach (DataRow dr in dt.Rows)
                lengths.Add(Convert.ToString(dr[columnIndex]).Length * 256);
            return lengths.Max();
        }

        private IList<string> GetRowValues(DataRow dr)
        {
            List<string> rowValues = new List<string>();

            for (int i = 0; i < dr.Table.Columns.Count; i++)
                rowValues.Add(Convert.ToString(dr[i]));

            return rowValues;
        }

        private IList<string> GetCloumnNames(DataTable dt)
        {
            List<string> columnNames = new List<string>();

            foreach (DataColumn dc in dt.Columns)
                columnNames.Add(dc.ColumnName);

            return columnNames;
        }

        private void SetRow(IRow row, IList<string> values)
        {
            SetRow(row, values, null);
        }

        private void SetRow(IRow row, IList<string> values, ICellStyle cellStyle)
        {
            for (int i = 0; i < values.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(values[i]);
                if (cellStyle != null)
                    cell.CellStyle = cellStyle;
            }
        }

        private ICellStyle GetCellStyle(IWorkbook wb)
        {
            return GetCellStyle(wb, FontBoldWeight.None);
        }

        private ICellStyle GetCellStyle(IWorkbook wb, FontBoldWeight boldweight)
        {
            ICellStyle cellStyle = wb.CreateCellStyle();

            //字体样式
            IFont font = wb.CreateFont();
            font.FontHeightInPoints = 10;
            font.FontName = "微软雅黑";
            font.Color = (short)FontColor.Normal;
            font.Boldweight = (short)boldweight;

            cellStyle.SetFont(font);

            //对齐方式
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            //边框样式
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;

            //设置背景色
            cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            cellStyle.FillPattern = FillPattern.SolidForeground;


            //是否自动换行
            cellStyle.WrapText = false;

            //缩进
            cellStyle.Indention = 0;

            return cellStyle;
        }

        private void AutoSizeColumn(ISheet sheet)
        {
            //获取当前列的宽度，然后对比本列的长度，取最大值
            for (int columnNum = 0; columnNum <= sheet.PhysicalNumberOfRows; columnNum++)
                AutoSizeColumn(sheet, columnNum);
        }

        private void AutoSizeColumn(ISheet sheet, int columnNum)
        {
            int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
            for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
            {
                IRow currentRow = sheet.GetRow(rowNum) == null ?
                    sheet.CreateRow(rowNum) : sheet.GetRow(rowNum);
                if (currentRow.GetCell(columnNum) != null)
                {
                    ICell currentCell = currentRow.GetCell(columnNum);
                    int length = System.Text.Encoding.Default.GetBytes(currentCell.ToString()).Length;
                    if (columnWidth < length)
                        columnWidth = length;
                }
            }
            sheet.SetColumnWidth(columnNum, columnWidth * 256);
        }
        #endregion
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class EntityMappingAttribute : Attribute
    {
        public string Name { get; set; }
    }

    public class NpoiStyle
    {
        public NpoiStyle()
        {
            //创建工作薄
            wb = new HSSFWorkbook();
        }

        static HSSFWorkbook wb;
        /// <summary>
        /// 字体样式--列表
        /// </summary>
        static List<IFont> fontList;
        /// <summary>
        /// 列样式--列表
        /// </summary>
        static List<ICellStyle> cellStyleList;

        /// <summary>
        /// 创建工作表
        /// </summary>
        /// <param name="name">工作表名称</param>
        /// <returns></returns>
        public static ISheet CreateWkSheet(string name)
        {
            if (wb == null)
                wb = new HSSFWorkbook();
            var sheet1 = wb.GetSheet(name);
            if (sheet1 == null)
            {
                fontList = new List<IFont>();
                cellStyleList = new List<ICellStyle>();
                return wb.CreateSheet(name);
            }
            else
            {
                return sheet1;
            }
        }

        /// <summary>
        /// 是否存在该--字体样式
        /// </summary>
        /// <param name="size">大小</param>
        /// <param name="fontName">字体名称</param>
        /// <param name="underline">下划线</param>
        /// <param name="isItalic">是否斜体</param>
        /// <param name="num">字体加粗值</param>
        /// <returns></returns>
        private static IFont IsHasFont(short size, string fontName, byte underline, bool isItalic, short num)
        {
            foreach (var fontBody in fontList)
            {
                if (fontBody.FontHeightInPoints == size
                    && fontBody.Boldweight == num
                    && fontBody.FontName == fontName
                    && fontBody.IsItalic == isItalic
                    && fontBody.Underline == (FontUnderlineType)underline)
                {
                    return fontBody;
                }
            }
            return null;
        }

        /// <summary>
        /// 是否存在-列样式
        /// </summary>
        /// <param name="font">字体样式</param>
        /// <param name="alignment">单元格水平对齐方式</param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static ICellStyle IsHasCellStyle(IFont font, HorizontalAlignment alignment, bool b)
        {
            if (wb == null)
                wb = new HSSFWorkbook();
            foreach (var cellStyle in cellStyleList)
            {
                if (cellStyle.Alignment == alignment && cellStyle.GetFont(wb) == font)
                {
                    if (
                        (cellStyle.TopBorderColor == HSSFColor.Black.Index && b)
                        || (cellStyle.TopBorderColor != HSSFColor.Black.Index && !b)
                        )
                    {
                        return cellStyle;
                    }
                    break;
                }
            }
            return null;
        }

        /// <summary>
        /// 返回字体
        /// </summary>
        /// <param name="size"></param>
        /// <param name="fontName"></param>
        /// <param name="underline"></param>
        /// <param name="isItalic"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static IFont GetFont(short size, string fontName = "宋体", byte underline = 0, bool isItalic = false, short num = (short)FontBoldWeight.Normal)
        {
            var f = IsHasFont(size, fontName, underline, isItalic, num);
            if (f != null) return f;
            if (wb == null)
                wb = new HSSFWorkbook();
            IFont fontBody = wb.CreateFont();
            fontBody.FontHeightInPoints = size;
            fontBody.Boldweight = num;
            fontBody.FontName = fontName;
            fontBody.IsItalic = isItalic;//倾斜
            fontBody.Underline = (FontUnderlineType)underline; //下划线
            fontList.Add(fontBody);
            return fontBody;
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="tb">工作薄</param>
        /// <param name="startRow">开始行</param>
        /// <param name="endRow">结束行</param>
        /// <param name="startCol">开始列</param>
        /// <param name="endCol">结束列</param>
        /// <returns></returns>
        public static ICell MergedRegion(ISheet tb, int startRow, int endRow, int startCol, int endCol)
        {
            IRow row = tb.GetRow(startRow) == null
                ? tb.CreateRow(startRow)
                : tb.GetRow(startRow);
            ICell cell = row.GetCell(startCol) == null
                ? row.CreateCell(startCol)
                : row.GetCell(startCol);
            row.Height = 30 * 20;
            if (startRow != endRow || startCol != endCol)
            {
                var i = tb.AddMergedRegion(new CellRangeAddress(startRow, endRow, startCol, endCol));
            }
            return cell;
        }

        /// <summary>
        /// 保存Excel到Path
        /// </summary>
        /// <param name="path">存储的路径</param>
        /// <param name="error">异常信息</param>
        /// <returns></returns>
        public static bool Save(string path, ref string error)
        {
            try
            {
                //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
                using (FileStream fs = File.OpenWrite(path))
                {
                    if (wb == null)
                    {
                        error = "WorkBool IS NULL";
                        return false;
                    }
                    wb.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
                    return true;
                }
            }
            catch (Exception ee)
            {
                error = ee.Message;
                return false;
            }
        }

        /// <summary>
        /// 返回列的样式并存储到样式列表
        /// </summary>
        /// <param name="font"></param>
        /// <param name="alignment"></param>
        /// <param name="b">是否加边框</param>
        /// <returns></returns>
        public static ICellStyle GetCellStyle(IFont font, HorizontalAlignment alignment, bool b = false)
        {
            var cs = IsHasCellStyle(font, alignment, b);
            if (cs != null)
                return cs;
            ICellStyle cellStyle = wb.CreateCellStyle();
            //水平对齐  
            cellStyle.Alignment = alignment;
            //垂直对齐  
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //自动换行  
            cellStyle.WrapText = true;
            //缩进;当设置为1时，前面留的空白太大了。或者是我设置的不对  
            cellStyle.Indention = 0;
            //设置字体样式
            cellStyle.SetFont(font);
            if (b)
            {
                //边框  
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderTop = BorderStyle.Thin;
                //边框颜色  
                cellStyle.BottomBorderColor = HSSFColor.Black.Index;
                cellStyle.TopBorderColor = HSSFColor.Black.Index;
            }
            cellStyleList.Add(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// 返回富文本框
        /// </summary>
        /// <param name="list">需要特别显示的字段</param>
        /// <param name="msg">整个字段</param>
        /// <param name="font">特别显示的字体</param>
        /// <returns></returns>
        public static HSSFRichTextString RichText(string[] list, string msg, IFont font, IFont font2)
        {
            HSSFRichTextString richtext = new HSSFRichTextString(msg);

            int index = 0;
            int length;
            int startNum = 0;
            foreach (var str in list)
            {
                if (string.IsNullOrEmpty(str))
                    continue;
                index = msg.IndexOf(str, startNum);
                length = str.Length + index;
                richtext.ApplyFont(index, length, font2);

                if (startNum != 0 && startNum != index)
                {
                    richtext.ApplyFont(startNum, index, font);
                }

                startNum = length;
            }
            if (startNum != 0 && startNum != msg.Length)
            {
                richtext.ApplyFont(startNum, msg.Length - 1, font);
            }
            return richtext;
        }

        public static HSSFRichTextString RichText(string str, string msg, IFont font, IFont font2)
        {
            HSSFRichTextString richtext = new HSSFRichTextString(msg);
            var index = msg.IndexOf(str, 0);

            var length = str.Length + index;
            richtext.ApplyFont(index, length, font2);

            if (length < msg.Length)
            {
                richtext.ApplyFont(length + 1, msg.Length - 1, font);
            }

            return richtext;
        }

        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="wk"></param>
        /// <param name="tb"></param>
        /// <param name="imgBytes">图片的byte数组</param>
        /// <param name="anchor">图片插入的大小和位置</param>
        /// <param name="isResize">是否自动调节大小</param>
        public static void Picture(ISheet tb, byte[] imgBytes, HSSFClientAnchor anchor, bool isResize)
        {
            //byte[] bytes = File.ReadAllBytes("D://1.png");
            int pictureIdx = wb.AddPicture(imgBytes, PictureType.PNG);
            var patriarch = tb.CreateDrawingPatriarch();
            //var anchor = new HSSFClientAnchor(0, 0, 1023, 0, 0, 44, 10, 86);
            var pict = patriarch.CreatePicture(anchor, pictureIdx);
            if (isResize)
            {
                pict.Resize();
            }
        }

        /// <summary>
        /// 返回excel内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="list"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Dictionary<string, DataTable> ExamUpload(string filePath, List<string> list, out string error)
        {
            error = "";
            Dictionary<string, System.Data.DataTable> dic = new Dictionary<string, DataTable>();
            try
            {
                using (FileStream fs = File.OpenRead(filePath))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(fs);
                    for (var k = 0; k < workbook.NumberOfSheets; k++)
                    {
                        var sheet1 = workbook.GetSheetAt(k);

                        DataTable table = new DataTable();
                        //设置表头
                        var headerRow = sheet1.GetRow(0);
                        for (int i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
                        {
                            DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                            table.Columns.Add(column);
                        }

                        //设置内容
                        for (int i = (sheet1.FirstRowNum + 1); i <= sheet1.LastRowNum; i++)
                        {
                            var row = sheet1.GetRow(i);
                            DataRow dataRow = table.NewRow();
                            for (int j = headerRow.FirstCellNum; j < headerRow.LastCellNum; j++)
                            {
                                if (row.GetCell(j) != null)
                                    dataRow[j] = row.GetCell(j).ToString();
                            }
                            table.Rows.Add(dataRow);
                        }
                        dic.Add(sheet1.SheetName, table);
                    }
                }
            }
            catch (Exception ee)
            {
                error = ee.Message;
            }
            return dic;

        }

        /// <summary>
        /// Excel导入DataTable
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        //private static DataTable ExcelToDataTable(OleDbConnection conn, string tableName)
        //{
        //    var strExcel = string.Format("select * from [{0}$]", tableName);
        //    var ds = new DataSet();

        //    var adapter = new OleDbDataAdapter(strExcel, conn);
        //    adapter.Fill(ds, tableName);

        //    //消除空行
        //    ds.Tables[tableName].Rows.RemoveAt(0);

        //    var newTable = ds.Tables[tableName].Clone();
        //    var columnCount = ds.Tables[tableName].Columns.Count;
        //    foreach (DataRow dr in ds.Tables[tableName].Rows)
        //    {
        //        var i = 0;
        //        for (; i < columnCount; i++)
        //        {
        //            if (!Convert.IsDBNull(dr[i]))
        //                break;
        //        }
        //        if (i == columnCount)
        //            break;

        //        newTable.Rows.Add(dr.ItemArray);
        //    }
        //    return newTable;
        //}
    }
}
