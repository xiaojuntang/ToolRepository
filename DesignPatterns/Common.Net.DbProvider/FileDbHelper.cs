/************************************************************************************************************************
* 命名空间: Common.Net.DbProvider
* 项目描述:
* 版本名称: v1.0.0.0
* 作　　者: 唐晓军
* 所在区域: 北京
* 机器名称: DESKTOP-F6QRRBM
* 注册组织: 学科网（www.zxxk.com）
* 项目名称: 学易作业系统
* CLR版本:  4.0.30319.42000
* 创建时间: 2017/9/26 14:40:58
* 更新时间: 2017/9/26 14:40:58
* 
* 功 能： N/A
* 类 名： ExcelDBHelper
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────────────────────────────
* V0.01 2017/9/26 14:40:58 唐晓军 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．                                                  │
*│　版权所有：北京凤凰学易科技有限公司　　　　　　　　　　　　　                                                      │
*└──────────────────────────────────────────────────────────┘
************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.DbProvider
{
    /// <summary>
    /// 文件读取类库
    /// </summary>
    public class FileDbHelper
    {
        /// <summary>
        /// 读取Excel文件  [Sheet1$]
        /// 安装http://download.microsoft.com/download/7/0/3/703ffbcb-dc0c-4e19-b0da-1463960fdcdb/AccessDatabaseEngine.exe
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataSet GetExcelFile(string filePath, string tableName)
        {
            string connection = string.Empty;
            if (GetExtension(filePath) == ".xls")
                connection = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
            else
                connection = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
            OleDbConnection conn = new OleDbConnection(connection);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("未加载组件 AccessDatabaseEngine.exe");
            }

            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            //string strExcel = "select * from [Sheet1$]";
            string strExcel = string.Concat("select * from [", tableName, "$]");
            myCommand = new OleDbDataAdapter(strExcel, connection);
            ds = new DataSet();
            myCommand.Fill(ds, tableName);
            return ds;
        }


        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string GetExtension(string fileName)
        {
            int i = fileName.LastIndexOf(".") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }
    }
}
