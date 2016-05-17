using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZxxkConsole
{
    /// <summary>
    /// 腾讯作业表
    /// </summary>
    public class Tencent_HomeWork
    {
        /// <summary>
        /// 作业ID
        /// </summary>
        public int HomeWorkID { get; set; }
        /// <summary>
        /// QQ ID
        /// </summary>
        public string OpenID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 其它
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public string HomeWorkName { get; set; }
        /// <summary>
        /// 初中语文   学段科目ID
        /// </summary>
        public int BankID { get; set; }
        /// <summary>
        /// HW_ZujuanNodes表中的NodeID
        /// </summary>
        public int TeachMaterialID { get; set; }
        /// <summary>
        /// 初中数学人教版
        /// </summary>
        public string TeachMaterialName { get; set; }
        /// <summary>
        /// 2,18,19,20,39  XYTF_QuesType表
        /// </summary>
        public string QuesTypeIDs { get; set; }
        /// <summary>
        /// 选择题,填空题,计算题,解答题,判断题
        /// </summary>
        public string QuesTypeNames { get; set; }
        /// <summary>
        /// 声音的产生,声音的传播条件,影响声速的因素,声音的相关计算,声音产生和传播的综合运用
        /// </summary>
        public string CategoryNames { get; set; }
        /// <summary>
        /// 是否删除 0正常 1删除
        /// </summary>
        public int Deleted { get; set; }
        /// <summary>
        /// HW_ZujuanNodes表中的NodeID
        /// </summary>
        public int GradeID { get; set; }
        /// <summary>
        /// 七年级上
        /// </summary>
        public string GradeName { get; set; }
        /// <summary>
        /// 第一章 有理数
        /// </summary>
        public string ChapterName { get; set; }
        /// <summary>
        /// 20 腾讯项目
        /// </summary>
        public int TRType { get; set; }
    }

    /// <summary>
    /// 作业试题对应表
    /// </summary>
    public class Tencent_HomeworkQuestion
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int HQID { get; set; }
        /// <summary>
        /// 作业ID
        /// </summary>
        public int HomeWorkID { get; set; }
        /// <summary>
        /// 试题ID
        /// </summary>
        public int QuesID { get; set; }
        /// <summary>
        /// 试题序号
        /// </summary>
        public int OrderNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Point { get; set; }
    }

    /// <summary>
    /// 作答记录表
    /// </summary>
    public class Tencent_Answer
    {
        /// <summary>
        /// 自增
        /// </summary>
        public Int64 AnswerID { get; set; }
        /// <summary>
        /// 作业ID
        /// </summary>
        public int HomeWorkID { get; set; }
        /// <summary>
        /// QQ ID
        /// </summary>
        public int OpenID { get; set; }
        /// <summary>
        /// 试题ID
        /// </summary>
        public int QuesID { get; set; }
        /// <summary>
        /// 题型ID
        /// </summary>
        public int QuesTypeID { get; set; }
        /// <summary>
        /// 正确答案
        /// </summary>
        public int QuesAnswer { get; set; }
        /// <summary>
        /// 学生答案
        /// </summary>
        public int StudentAnswer { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public int CommitDate { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// 质点概念的理解
        /// </summary>
        public int CategoryName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Point { get; set; }
        /// <summary>
        /// 父试题ID
        /// </summary>
        public int ParentQuesID { get; set; }
        /// <summary>
        /// 试题数量
        /// </summary>
        public int QuesNumber { get; set; }
    }

    public class Dal
    {
        public void InserHomeWork(Tencent_HomeWork model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@OpenID", SqlDbType.VarChar,32){ Direction=ParameterDirection.InputOutput, Value=model.OpenID},
                new SqlParameter("@Comment", SqlDbType.VarChar,100){ Direction=ParameterDirection.InputOutput, Value=model.Comment},
                new SqlParameter("@HomeWorkName", SqlDbType.VarChar,200){ Direction=ParameterDirection.InputOutput, Value=model.HomeWorkName},
                new SqlParameter("@BankID", model.BankID),
                new SqlParameter("@TeachMaterialID", model.TeachMaterialID),
                new SqlParameter("@TeachMaterialName", model.TeachMaterialName),
                new SqlParameter("@QuesTypeIDs", model.QuesTypeIDs),
                new SqlParameter("@QuesTypeNames", model.QuesTypeNames),
                new SqlParameter("@CategoryNames", model.CategoryNames),
                new SqlParameter("@GradeID", model.GradeID),
                new SqlParameter("@GradeName", model.GradeName),
                new SqlParameter("@ChapterName", model.ChapterName)
            };
            StringBuilder Sql = new StringBuilder(1000);
            Sql.Append(@"INSERT INTO [Tencent_HomeWork]
([OpenID],[Comment],[HomeWorkName],[BankID],[TeachMaterialID],[TeachMaterialName],[QuesTypeIDs],[QuesTypeNames],[CategoryNames],[GradeID],[GradeName],[ChapterName])
VALUES (@OpenID,@Comment,@HomeWorkName,@BankID,@TeachMaterialID,@TeachMaterialName,@QuesTypeIDs,@QuesTypeNames,@CategoryNames,@GradeID,@GradeName,@ChapterName);SELECT IDENT_CURRENT('Tencent_HomeWork')");


            Sql.Append(@"INSERT INTO [Tencent_HomeworkQuestion]([HomeWorkID],[QuesID],[OrderNumber],[Point]) VALUES (@HomeWorkID,@QuesID,@OrderNumber,@Point);");


            //List<String> SQLStringList, List< MySqlParameter[] > SqlParameterList



            Sql.Append(@"INSERT INTO [dbo].[Tencent_Answer]
		([HomeWorkID],[OpenID],[QuesID],[QuesTypeID],[QuesAnswer],[StudentAnswer],[CommitDate],[CategoryID],[CategoryName],[Point],[ParentQuesID],[QuesNumber])
	VALUES
		(@HomeWorkID,@OpenID,@QuesID,@QuesTypeID,@QuesAnswer,@StudentAnswer,@CommitDate,@CategoryID,@CategoryName,@Point,@ParentQuesID,@QuesNumber);");
        }

       
    }
}
