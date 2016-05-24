/***************************************************************************** 
*        filename :MySQLHepler用法实例 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   MySQLHepler用法实例 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       DataPatterns 
*        文件名:             MySQLHepler用法实例 
*        创建系统时间:       2016/2/16 10:58:07 
*        创建年份:           2016 
/*****************************************************************************/
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPatterns
{
    class MySQLHepler用法实例
    {
    }

    /// <summary>
    /// MySQLHelper使用过程
    /// </summary>
    public class Test
    {

        //public static bool UpdateQuestion(Dictionary<string, object> SetParam, string Subject, int QuestionId)
        //{
        //    string conn = ConfigurationManager.ConnectionStrings["CResource"].ToString();//200.103
        //    using (MySqlDbHelper _mySqlDbHelper = new MySqlDbHelper(conn))
        //    {
        //        var mySqlParams = new List<IDbDataParameter>
        //        {
        //           _mySqlDbHelper.AddParameterWithValue("@f_subject",Subject),
        //           _mySqlDbHelper.AddParameterWithValue("@f_questionid",QuestionId)
        //        };
        //        if (SetParam.Count > 1)
        //        {

        //        }
        //        string setClause = ConvertSql(SetParam, mySqlParams);
        //        string sqlText = string.Format("UPDATE mfg_t_questionmanage SET {0} WHERE f_subject=@f_subject AND f_questionid=@f_questionid", setClause);
        //        return true;//_mySqlDbHelper.ExecSql(sqlText, mySqlParams.ToArray())>0;
        //    }
        //}

        //private static MySqlHelper dbHelper;

        public UserInfoModel GetUserInfo(int accountnumber, int orgid)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"SELECT a.orgid,a.acastru,a.artsciences from mfg_base_userinfo a WHERE a.accountnumber=@accountnumber AND a.orgid=@orgid;");
            List<MySqlParameter> parameters = new List<MySqlParameter>() {
                 new MySqlParameter("@accountnumber", MySqlDbType.Int32,11) { Direction = ParameterDirection.Input, Value = accountnumber },
                 new MySqlParameter("@orgid", MySqlDbType.Int32,11) { Direction = ParameterDirection.Input, Value = orgid }
            };
            return MySQLHelper.FindList<UserInfoModel>(sql.ToString(), (a) =>
            {
                return new UserInfoModel()
                {
                    accountnumber = accountnumber,
                    orgid = a.GetInt32(0),
                    acastru = a.GetInt32(1),
                    artsciences = a.GetInt32(2)
                };
            }, parameters).FirstOrDefault();
        }

        public QuestionNopagResult GetUserInfos(int accountnumber, int orgid)
        {
            QuestionNopagResult r = new QuestionNopagResult();

            StringBuilder sql = new StringBuilder();
            sql.Append(@"select SubjectID,GradeID,StageID,ExamName from mfg_exam_subject_template where ExamID=@ExamID;");

            List<MySqlParameter> parameters = new List<MySqlParameter>() {
             new MySqlParameter("@ExamID", MySqlDbType.Int32,11){ Direction=ParameterDirection.InputOutput, Value=accountnumber},
            };

            MySQLHelper.FindList(sql.ToString(), (a) =>
            {
                if (a.HasRows)
                {
                    while (a.Read())
                    {

                    }
                }
            }, parameters);

            return r;
        }
    }

    /// <summary>
    /// 包含List
    /// </summary>
    public class QuestionNopagResult
    {
        public List<int> Result { get; set; }
    }

    /// <summary>
    /// 教师信息
    /// </summary>
    public class UserInfoModel : BaseModel
    {
        /// <summary>
        /// 登录者账户
        /// </summary>
        public int accountnumber { get; set; }

        public int accounttype { get; set; }

        public int orgid { get; set; }

        /// <summary>
        /// 学制,0六三制 ;1五四制;2 未选中
        /// </summary>
        public int acastru { get; set; }

        /// <summary>
        /// 文理,0:理科，1:文科，2:不分文理 3 未选中
        /// </summary>
        public int artsciences { get; set; }
    }

    public class BaseModel : MarshalByRefObject
    {
    }

    /// <summary>
    /// 具体用法
    /// </summary>
    public class SubjectExamDAL
    {
        //        private static MySqlDbHelper dbHelper_Source;

        //        public UserInfoModel GetUserInfo(int accountnumber, int orgid)
        //        {
        //            StringBuilder sql = new StringBuilder();
        //            sql.Append(@"SELECT a.orgid,a.acastru,a.artsciences from mfg_base_userinfo a 
        //WHERE a.accountnumber=@accountnumber AND a.orgid=@orgid;");
        //            List<MySqlParameter> parameters = new List<MySqlParameter>() {
        //                 new MySqlParameter("@accountnumber", MySqlDbType.Int32,11) { Direction = ParameterDirection.Input, Value = accountnumber },
        //                 new MySqlParameter("@orgid", MySqlDbType.Int32,11) { Direction = ParameterDirection.Input, Value = orgid }
        //            };
        //            return MySQLHelper.ExecuteStatement<UserInfoModel>(sql.ToString(), (a) =>
        //            {
        //                return new UserInfoModel()
        //                {
        //                    accountnumber = accountnumber,
        //                    orgid = a.GetInt32(0),
        //                    acastru = a.GetInt32(1),
        //                    artsciences = a.GetInt32(2)
        //                };
        //            }, parameters).FirstOrDefault();
        //        }

        //        public bool SaveItems(ItemsSubjectPara para)
        //        {
        //            List<MySqlParameter> parameters = new List<MySqlParameter>()
        //            {
        //                new MySqlParameter("@accountnumber", MySqlDbType.Int32,11) { Direction = ParameterDirection.Input, Value = para.para.accountnumber },
        //                new MySqlParameter("@orgid", MySqlDbType.Int32,11) { Direction = ParameterDirection.Input, Value = para.para.orgid },
        //                new MySqlParameter("@PaperID",MySqlDbType.Int64,20) {  Direction=ParameterDirection.Input, Value=para.PaperID},
        //                //new MySqlParameter("@ExamID", MySqlDbType.Int32,11){ Direction=ParameterDirection.Input, Value=para.para.ExamID},
        //                new MySqlParameter("@TempID", MySqlDbType.Int32,11){ Direction=ParameterDirection.Input, Value=para.TempID}
        //            };

        //            var sql0 = new StringBuilder();//答题表
        //            StringBuilder sqlPot = new StringBuilder();//知识掌握度分析
        //            StringBuilder sqlDiff = new StringBuilder();//试卷难度分析
        //            StringBuilder sqlSub = new StringBuilder();//更新属性
        //            var i = 1;
        //            foreach (var item in para.data)
        //            {
        //                if (i == 1)
        //                {
        //                    sql0.AppendFormat(@" SELECT {0} as Accuracy,'{1}' as Answer,{2} as AnswerTime,{3} as ItemID ",
        //                        item.Accuracy, item.Answer, item.AnswerTime, item.ItemID);
        //                }
        //                else
        //                {
        //                    sql0.AppendFormat(@" UNION SELECT {0} as Accuracy,'{1}' as Answer,{2} as AnswerTime,{3} as ItemID ",
        //                       item.Accuracy, item.Answer, item.AnswerTime, item.ItemID);
        //                }
        //                i++;
        //            }


        //            #region 答题
        //            StringBuilder sql = new StringBuilder();
        //            sql.AppendFormat(@"INSERT INTO mfg_exam_subject_answer
        //(PaperItemID, TempID, Accuracy, Answer, AnswerTime, ItemSource, CreateTime, DelFlag, Remark)
        //SELECT a.PaperItemID,@TempID,b.Accuracy,b.Answer,b.AnswerTime,a.ItemSource,NOW(),0,'' FROM mfg_exam_subject_paper_item a 
        //LEFT JOIN ({0}) b on a.ItemID=b.ItemID WHERE a.PaperID=@PaperID;", sql0.ToString());
        //            #endregion

        //            #region 知识掌握分析
        //            i = 0;
        //            var pot = para.data.GroupBy(a => new { a.ParentPointID, a.PointID, a.f_keyordiff, a.f_percent, a.f_freq })
        //                .Select(b => new
        //                {
        //                    PointName = "",
        //                    ParentPointName = "",
        //                    ParentPointID = b.Key.ParentPointID,
        //                    PointID = b.Key.PointID,
        //                    KeyOrDiff = b.Key.f_keyordiff,
        //                    Percent = b.Key.f_percent,
        //                    Freq = b.Key.f_freq,
        //                    Count = b.Count(),//合计多少条数据
        //                    Sum = b.Sum(c => c.Accuracy)//答对多少题
        //                });

        //            var calresult = Math.Ceiling(para.data.Sum(x => x.Accuracy) * 100.0 / para.data.Count);//正确率
        //            var ResultLevel = "";
        //            if (calresult >= 90 && calresult <= 100)
        //            {
        //                ResultLevel = "A";
        //            }
        //            else if (calresult >= 80 && calresult < 90)
        //            {
        //                ResultLevel = "B";
        //            }
        //            else if (calresult >= 70 && calresult < 80)
        //            {
        //                ResultLevel = "C";
        //            }
        //            else if (calresult >= 60 && calresult < 70)
        //            {
        //                ResultLevel = "D";
        //            }
        //            else if (calresult < 60)
        //            {
        //                ResultLevel = "E";
        //            }

        //            sqlPot.Append(@"INSERT INTO mfg_exam_subject_grasp( PaperID, TempID, ParentPointID, ParentPointName, PointID, PointName, PTotalCount, PRightCount, Degree, EstimateValue, PointLevel, CreateTime) VALUES ");
        //            foreach (var item in pot)
        //            {
        //                if (i == 0)
        //                {
        //                    sqlPot.AppendFormat(@"(@PaperID,@TempID,'{0}','{1}','{2}','{3}',{4},{5},'{6}',{7},'{8}','{9}')", item.ParentPointID, item.ParentPointName, item.PointID, item.PointName, item.Count, item.Sum, item.KeyOrDiff, item.Percent, item.Freq, DateTime.Now);
        //                }
        //                else
        //                {
        //                    sqlPot.AppendFormat(@",(@PaperID,@TempID,'{0}','{1}','{2}','{3}',{4},{5},'{6}',{7},'{8}','{9}')", item.ParentPointID, item.ParentPointName, item.PointID, item.PointName, item.Count, item.Sum, item.KeyOrDiff, item.Percent, item.Freq, DateTime.Now);
        //                }
        //                i++;
        //            }

        //            sqlPot.Append(@";");//知识掌握度分析 
        //            #endregion

        //            #region 试卷难度分析

        //            i = 0;
        //            var Diff = para.data.GroupBy(a => new { a.f_difficulty, a.f_difficultyDesc }).
        //              Select(b => new
        //              {
        //                  Difficty = b.Key.f_difficulty,
        //                  DiffictyName = b.Key.f_difficultyDesc,
        //                  TotalCount = b.Count(),//一共多少试题
        //                  RightCount = b.Sum(c => c.Accuracy),//答对试题数量
        //                  AnswerTime = b.Sum(c => c.AnswerTime),//答题时间
        //                  RightRate = Math.Ceiling(b.Sum(c => c.Accuracy) * 1.0 / b.Count()),//答对试题占比例
        //                  ExpectRate = CommonTools.GetExpectRate(b.Key.f_difficulty),//预期正确率率
        //              });
        //            sqlDiff.Append(@"INSERT INTO mfg_exam_subject_diff(TempID, PaperID, DiffictyCode, DiffictyName, TotalCount, RightCount, AnswerTime, RightRate, ExpectRate, UpdateExpectRate, IsUpdate, CreateTime) VALUES");
        //            foreach (var item in Diff)
        //            {
        //                if (i == 0)
        //                    sqlDiff.AppendFormat(@"(@TempID,@PaperID,{0},'{1}',{2},{3},{4},'{5}','{6}',0,0,'{7}')", item.Difficty, item.DiffictyName, item.TotalCount, item.RightCount, item.AnswerTime, item.RightRate, item.ExpectRate, DateTime.Now);
        //                else
        //                    sqlDiff.AppendFormat(@",(@TempID,@PaperID,{0},'{1}',{2},{3},{4},'{5}','{6}',0,0,'{7}')", item.Difficty, item.DiffictyName, item.TotalCount, item.RightCount, item.AnswerTime, item.RightRate, item.ExpectRate, DateTime.Now);
        //                i++;
        //            }

        //            sqlDiff.Append(@";");//试卷难度分析 
        //            #endregion

        //            #region 更新属性

        //            sqlSub.AppendFormat(@"UPDATE mfg_exam_student a 
        //INNER JOIN (SELECT b.PaperID, SUM(b.ClassHour) as ClassHour from mfg_exam_subject_paper_point b WHERE b.PaperID=@PaperID GROUP BY b.PaperID) c
        //on a.PaperID=c.PaperID
        //set a.TotalHour=c.ClassHour,a.MeasureVersion=10,a.ResultLevel='{0}',a.IsEffect=1,a.LastUpdateUser=@accountnumber,a.EditTime=NOW()
        //WHERE a.TempID=@TempID and a.PaperID=@PaperID;", ResultLevel);//更新属性

        //            sqlSub.Append(@"UPDATE mfg_exam_subject_grasp a 
        //INNER JOIN (
        //SELECT DISTINCT n.PaperID,n.PointName as ParentPointName,m.PointID,m.PointName from mfg_exam_subject_paper_point n
        //INNER JOIN mfg_exam_subject_paper_item m on m.PaperID=n.PaperID AND m.ExamKnowID=n.ExamKnowID
        //WHERE n.IsUse=1 AND n.PaperID=@PaperID
        //) as b on a.PaperID=b.PaperID AND a.PointID=b.PointID
        //set a.ParentPointName=b.ParentPointName, a.PointName=b.PointName
        //WHERE a.PaperID=@PaperID;");//更新属性

        //            #endregion

        //            var t = sql.ToString() + sqlPot.ToString() + sqlDiff.ToString() + sqlSub.ToString();

        //            var r = false;

        //            MySQLHelper.ExecuteStatementList(t, (a) => { r = true; }, parameters);

        //            return r;
        //        }

        //        public AnswerTestModel WorkInit(long paperID)
        //        {
        //            AnswerTestModel r = new AnswerTestModel();
        //            StringBuilder sql = new StringBuilder();
        //            sql.Append(@"SELECT a.IsEffect from mfg_exam_student a WHERE a.PaperID=@PaperID;
        //set @Num:=0;
        //SELECT a.ParentPointID,a.ItemID,a.DiffNum,@Num:=@Num+1 as ItemIndex,a.PointID from mfg_exam_subject_paper_item a WHERE a.ItemSource=0 AND a.PaperID=@PaperID ORDER BY a.ItemIndex;");
        //            List<MySqlParameter> parameters = new List<MySqlParameter>() {
        //                new MySqlParameter("@paperID", MySqlDbType.Int64,20){ Direction=ParameterDirection.Input, Value=paperID}
        //            };
        //            MySQLHelper.ExecuteStatementList(sql.ToString(), (a) =>
        //            {
        //                if (a.HasRows)
        //                {
        //                    //是否答题
        //                    if (a.HasRows)
        //                    {
        //                        while (a.Read())
        //                        {
        //                            r.IsEffect = a.GetBoolean(0);
        //                        }
        //                    }

        //                    //答题列表
        //                    if (a.NextResult())
        //                    {
        //                        List<AnswerAssessmentModel> list = new List<AnswerAssessmentModel>();
        //                        while (a.Read())
        //                        {
        //                            AnswerAssessmentModel dto = new AnswerAssessmentModel();
        //                            dto.ParentPointID = a.GetString(0);
        //                            dto.ItemID = a.GetInt32(1);
        //                            dto.DiffNum = a.GetInt32(2);
        //                            if (dto.DiffNum <= 20)
        //                                dto.DiffNum = 1;
        //                            else if (dto.DiffNum <= 40)
        //                                dto.DiffNum = 2;
        //                            else if (dto.DiffNum <= 60)
        //                                dto.DiffNum = 3;
        //                            else if (dto.DiffNum <= 80)
        //                                dto.DiffNum = 4;
        //                            else dto.DiffNum = 5;
        //                            dto.ItemIndex = a.GetInt32(3);
        //                            dto.PointID = Convert.ToInt32(a.IsDBNull(4) ? (0).ToString() : a.GetString(4));
        //                            list.Add(dto);
        //                        }
        //                        r.list = list;
        //                    }
        //                }
        //            }, parameters);
        //            return r;
        //        }

        //        public SubjectInit_Model InitExamSubject(int examid)
        //        {
        //            SubjectInit_Model r = new SubjectInit_Model();
        //            List<MySqlParameter> parameters = new List<MySqlParameter>() {
        //                 new MySqlParameter("@ExamID", MySqlDbType.Int32,11) { Direction = ParameterDirection.Input, Value = examid },
        //            };

        //            StringBuilder sql = new StringBuilder();
        //            sql.Append(@"SELECT a.PointID,a.PointName,a.IsUse,a.DefaultHour,a.ClassHour FROM mfg_exam_subject_point a 
        //WHERE a.ExamID=@ExamID ORDER BY a.PointIndex;
        //SELECT a.ExamID,a.PointID,a.PointName,b.ItemID,b.DiffNum,b.ItemIndex,b.ItemSource,b.PointID,b.PointName
        //FROM mfg_exam_subject_point a 
        //INNER JOIN mfg_exam_subject_item b on a.ExamID=b.ExamID AND a.PointID=b.ParentPointID
        //WHERE a.ExamID=@ExamID;");

        //            MySQLHelper.ExecuteStatementList(sql.ToString(), (a) =>
        //            {
        //                if (a.HasRows)
        //                {
        //                    List<SubjectPointsInit_Model> points = new List<SubjectPointsInit_Model>();
        //                    while (a.Read())
        //                    {
        //                        SubjectPointsInit_Model dto = new SubjectPointsInit_Model();
        //                        dto.ParentPointID = a.GetString(0);
        //                        dto.ParentPointName = a.GetString(1);
        //                        dto.IsUse = a.GetBoolean(2);
        //                        dto.DefaultHour = a.GetInt32(3);
        //                        dto.time = a.GetInt32(4);
        //                        points.Add(dto);
        //                    }
        //                    r.points = points;
        //                }
        //                if (a.NextResult())
        //                {
        //                    if (a.HasRows)
        //                    {
        //                        List<SubjectItemsInit_Model> items = new List<SubjectItemsInit_Model>();
        //                        while (a.Read())
        //                        {
        //                            SubjectItemsInit_Model dto = new SubjectItemsInit_Model();
        //                            dto.ExamID = a.GetInt32(0);
        //                            dto.ParentPointID = a.GetString(1);
        //                            dto.ParentPointName = a.GetString(2);
        //                            dto.ItemID = a.GetInt32(3);
        //                            dto.DiffNum = a.GetInt32(4);
        //                            dto.SequenceID = a.GetInt32(5);
        //                            dto.ItemSource = a.GetByte(6);
        //                            dto.PointID = a.GetString(7);
        //                            dto.PointName = a.GetString(8);
        //                            items.Add(dto);
        //                        }
        //                        r.items = items;
        //                    }
        //                }

        //            }, parameters);

        //            return r;
        //        }

        //        public bool SaveSubjectExam(ExamSubjectPara para)
        //        {
        //            List<MySqlParameter> parameters = new List<MySqlParameter>() {
        //                 new MySqlParameter("@accountnumber", MySqlDbType.Int32,11) { Direction = ParameterDirection.Input, Value =para.para.accountnumber },
        //                 new MySqlParameter("@ExamID", MySqlDbType.Int32,11) { Direction = ParameterDirection.Input, Value = para.para.ExamID },
        //                 new MySqlParameter("@ScheduledTime", MySqlDbType.Float) { Direction = ParameterDirection.Input, Value = para.ScheduledTime },
        //                 new MySqlParameter("@ExamName", MySqlDbType.VarChar,200) { Direction = ParameterDirection.Input, Value = para.TestName }
        //            };

        //            StringBuilder sql = new StringBuilder();

        //            sql.Append(@"DELETE FROM mfg_exam_subject_item WHERE ExamID=@ExamID;");

        //            sql.Append(@"DELETE FROM mfg_exam_subject_point WHERE ExamID=@ExamID;");

        //            if (para.points.Count > 0)
        //            {
        //                var t = new StringBuilder();
        //                var i = 1;
        //                foreach (var item in para.points)
        //                {
        //                    if (i == 1)
        //                    {
        //                        t.AppendFormat(@"(@ExamID, '{0}', '{1}', {2}, {3}, {4}, {5}, {6},now())",
        //                            item.ParentPointID, para.datas.Where(a => a.ParentPointID == item.ParentPointID).Select(b => b.ParentPointName).FirstOrDefault(),
        //                            item.ClassHour, item.DefaultHour, 0, 1, i);
        //                    }
        //                    else
        //                    {
        //                        t.AppendFormat(@",(@ExamID, '{0}', '{1}', {2}, {3}, {4}, {5}, {6},now())",
        //                           item.ParentPointID, para.datas.Where(a => a.ParentPointID == item.ParentPointID).Select(b => b.ParentPointName).FirstOrDefault(),
        //                           item.ClassHour, item.DefaultHour, 0, 1, i);
        //                    }
        //                    i++;
        //                }

        //                sql.AppendFormat(@"INSERT INTO mfg_exam_subject_point( ExamID, PointID, PointName, ClassHour, DefaultHour, DiffNum, IsUse, PointIndex,CreateTime)
        //VALUES {0};", t.ToString());

        //            }

        //            if (para.datas.Count > 0)
        //            {
        //                var t = new StringBuilder();
        //                var i = 1;
        //                foreach (var item in para.datas)
        //                {
        //                    if (i == 1)
        //                    {
        //                        t.AppendFormat(@"SELECT {0} as ExamID, '{1}' as ItemID,{2} as DiffNum,{3} as ItemIndex,'{4}' as PointID, '{5}' as PointName,'{6}' as ParentPointID",
        //                            item.ExamID, item.ItemID, item.DiffNum, i, item.PointID, item.PointName, item.ParentPointID);
        //                    }
        //                    else
        //                    {
        //                        t.AppendFormat(@" UNION SELECT {0} as ExamID, '{1}' as ItemID,{2} as DiffNum,{3} as ItemIndex,'{4}' as PointID, '{5}' as PointName,'{6}' as ParentPointID",
        //                           item.ExamID, item.ItemID, item.DiffNum, i, item.PointID, item.PointName, item.ParentPointID);
        //                    }
        //                    i++;
        //                }
        //                sql.AppendFormat(@"INSERT INTO mfg_exam_subject_item
        //( ExamID, ExamKnowID, ParentPointID, ItemID, DiffNum, ItemIndex, ItemSource, PointID, PointName, CreateTime)
        //SELECT @ExamID,t.ExamKnowID,t.PointID,p.ItemID,p.DiffNum,p.ItemIndex,0,p.PointID,p.PointName,NOW() 
        //from mfg_exam_subject_point t LEFT JOIN ({0}) as p on t.ExamID=p.ExamID AND t.PointID=p.ParentPointID
        //WHERE t.ExamID=@ExamID;", t.ToString());
        //            }

        //            sql.Append(@"UPDATE mfg_exam_subject_template set ExamName=@ExamName, ScheduledTime=@ScheduledTime,IsEnable=1, LastUpdateTime=NOW(),LastTID=@accountnumber
        //WHERE ExamID=@ExamID;");

        //            return MySQLHelper.ExecuteStatement(sql.ToString(), parameters) > 1 ? true : false;
        //        }

        //        public MessageModel SaveExamInfo(UserInfoParameterModel dto)
        //        {
        //            MessageModel r = new MessageModel();
        //            StringBuilder sql = new StringBuilder();
        //            sql.Append(@"INSERT INTO mfg_exam_subject_template
        //( ExamName, TID, LastTID, StageID, GradeID, ExamTerm, SubjectID, ScheduledTime, IsEnable, IsDelete, CreateTime, LastUpdateTime, MaterialID, Mversion, ExamVersion, Remarks, SourceID)
        //VALUES(@ExamName, @accountnumber, @accountnumber, @StageID, @GradeID, 2, @SubjectID, 0, 0, 0, NOW(), NOW(), '', '', 0, '', 0);");
        //            sql.Append(@"SELECT LAST_INSERT_ID();");
        //            List<MySqlParameter> parameters = new List<MySqlParameter>() {
        //                 new MySqlParameter("@accountnumber", MySqlDbType.Int32,11) { Direction = ParameterDirection.Input, Value = dto.TID },
        //                 new MySqlParameter("@orgid", MySqlDbType.Int32,11) { Direction = ParameterDirection.Input, Value = dto.OrgID },
        //                 new MySqlParameter("@StageID", MySqlDbType.Byte,1) { Direction = ParameterDirection.Input, Value = dto.StageID },
        //                 new MySqlParameter("@GradeID", MySqlDbType.Byte,1) { Direction = ParameterDirection.Input, Value = dto.GradeID },
        //                 new MySqlParameter("@SubjectID", MySqlDbType.Byte,1) { Direction = ParameterDirection.Input, Value = dto.SubjectID },
        //                 new MySqlParameter("@ExamName", MySqlDbType.VarChar,200) { Direction = ParameterDirection.Input, Value = dto.Name }
        //            };
        //            r.ID = MySQLHelper.ExecuteStatement<Int32>(sql.ToString(), (a) =>
        //            {
        //                return a.GetInt32(0);
        //            }, parameters).FirstOrDefault();
        //            return r;
        //        }

        //        public QuestionNopageIndex GetExam(ExamParameterModel para)
        //        {
        //            QuestionNopageIndex r = new QuestionNopageIndex();

        //            StringBuilder sql = new StringBuilder();
        //            sql.Append(@"select SubjectID,GradeID,StageID,ExamName from mfg_exam_subject_template where ExamID=@ExamID;");

        //            List<MySqlParameter> parameters = new List<MySqlParameter>() {
        //             new MySqlParameter("@ExamID", MySqlDbType.Int32,11){ Direction=ParameterDirection.InputOutput, Value=para.ExamID},
        //            };

        //            MySQLHelper.ExecuteStatementList(sql.ToString(), (a) =>
        //            {
        //                if (a.HasRows)
        //                {
        //                    while (a.Read())
        //                    {
        //                        mfg_exam_subject_template dto = new mfg_exam_subject_template();
        //                        dto.SubjectID = a.GetByte(0);
        //                        dto.GradeID = a.GetByte(1);
        //                        dto.StageID = a.GetByte(2);
        //                        dto.ExamName = a.GetString(3);
        //                        r.template = dto;
        //                    }
        //                }
        //            }, parameters);

        //            return r;
        //        }


        //        public QuestionNopageIndex GePreView(ExamParameterModel para)
        //        {
        //            QuestionNopageIndex r = new QuestionNopageIndex();

        //            StringBuilder sql = new StringBuilder();
        //            sql.Append(@"select SubjectID,GradeID,StageID,ExamName from mfg_exam_subject_template where ExamID=@ExamID;");
        //            sql.Append(@"SELECT ExamItemID,ItemID,ItemIndex,PointName FROM mfg_exam_subject_item WHERE ExamID=@ExamID ORDER BY ItemIndex;");

        //            List<MySqlParameter> parameters = new List<MySqlParameter>() {
        //             new MySqlParameter("@ExamID", MySqlDbType.Int32,11){ Direction=ParameterDirection.InputOutput, Value=para.ExamID},
        //            };

        //            MySQLHelper.ExecuteStatementList(sql.ToString(), (a) =>
        //            {
        //                if (a.HasRows)
        //                {
        //                    while (a.Read())
        //                    {
        //                        mfg_exam_subject_template dto = new mfg_exam_subject_template();
        //                        dto.SubjectID = a.GetByte(0);
        //                        dto.GradeID = a.GetByte(1);
        //                        dto.StageID = a.GetByte(2);
        //                        dto.ExamName = a.GetString(3);
        //                        r.template = dto;
        //                    }
        //                }
        //                if (a.NextResult())
        //                {
        //                    if (a.HasRows)
        //                    {
        //                        r.Data = new List<mfg_exam_subject_item>();
        //                        while (a.NextResult())
        //                        {
        //                            mfg_exam_subject_item dto = new mfg_exam_subject_item();
        //                            dto.ExamItemID = a.GetInt32(0);
        //                            dto.ItemID = a.GetInt32(1);
        //                            dto.ItemIndex = a.GetInt32(2);
        //                            dto.PointName = a.GetString(3);
        //                            r.Data.Add(dto);
        //                        }
        //                    }
        //                }
        //            }, parameters);

        //            return r;
        //        }

        //        /// <summary>
        //        /// 设置试卷模板删除状态
        //        /// </summary>
        //        /// <param name="ExamID"></param>
        //        /// <returns></returns>
        //        public int DelTemplatePaper(int ExamID)
        //        {
        //            //1.设置不启用0 3.设置删除标示1 2.修改最后修改时间

        //            string sql = @"UPDATE mfg_exam_subject_template set IsEnable=0,IsDelete=1,LastUpdateTime=NOW()
        //                         where ExamID = @ExamID ";
        //            MySqlParameter[] parameters = {
        //                  new MySqlParameter("@ExamID", MySqlDbType.Int64,20) { Value= ExamID  }
        //            };

        //            return MySQLHelper.ExecuteSql(sql, parameters) > 0 ? 1 : -1;


        //        }


        //        /// <summary>
        //        /// 保存用户考试信息--学科
        //        /// </summary>
        //        /// <param name="model"></param>
        //        /// <returns>TempID</returns>
        //        public int SaveSubjectExamStuInfo(ExamStudent model)
        //        {
        //            //添加试卷,获取paperid
        //            //插入用户信息(paperid)
        //            //插入知识点(paperid),获取知识点id
        //            //插入试题(知识点id)


        //            StringBuilder strSql = new StringBuilder();
        //            strSql.Append(@"
        //
        //         insert into mfg_exam_subject_paper(ExamID,ExamName,TID,LastTID,StageID,GradeID,ExamTerm,SubjectID,ScheduledTime,IsEnable,IsDelete,CreateTime,
        //         LastUpdateTime,MaterialID,Mversion,ExamVersion,Remarks,SourceID)
        //         select  ExamID,ExamName,TID,LastTID,StageID,GradeID,ExamTerm,@SubjectID :=SubjectID as SubjectID,ScheduledTime,IsEnable,IsDelete,CreateTime,LastUpdateTime,
        //               MaterialID,Mversion,ExamVersion,Remarks,SourceID from mfg_exam_subject_template
        //        where ExamID=@ExamID and IsDelete=0 and IsEnable=1;
        //         
        //set @paperid:= LAST_INSERT_ID();  
        //
        //insert into mfg_exam_student(  PaperID,TID,TempName,Phone,DelFlag,Remark,MFGID,EditTime,IsEffect,IsFile,GradeID,StageID,Gender,Age,
        //          School,Adddress,GroupVersion,ExamType,MeasureVersion,ResultLevel,LastUpdateUser,TotalHour,MaterialID,Mversion,ScheduledTime,CreateTime,SourceID,SubjectID )
        //          select
        //           @paperid,@TID,@TempName,@Phone,0,@Remark,@MFGID,NOW(),0,0,GradeID,StageID,@Gender,@Age,@School,
        //         @Adddress,0,@ExamType,10,'',@TID,0,MaterialID,Mversion,ScheduledTime,NOW(),SourceID ,@SubjectID
        //          from mfg_exam_subject_template 
        //        where ExamID=@ExamID;
        //set @TempID=LAST_INSERT_ID();
        //          insert into mfg_exam_subject_paper_point( PaperID,ExamKnowID,ExamID,PointID,PointName,ClassHour,DefaultHour,DiffNum,IsUse,PointIndex,CreateTime )
        //         select @paperid, ExamKnowID,ExamID,PointID,PointName,ClassHour,DefaultHour,DiffNum,IsUse,PointIndex,CreateTime from mfg_exam_subject_point 
        //       where  ExamID=@ExamID and IsUse=1;
        //        
        //       insert into mfg_exam_subject_paper_item( PaperID,ExamKnowID,ExamItemID,ExamID,ParentPointID,ItemID,DiffNum,ItemIndex,ItemSource,PointID,PointName,CreateTime )
        //      select @paperid,ExamKnowID,ExamItemID,ExamID,ParentPointID,ItemID,DiffNum,ItemIndex,ItemSource,PointID,PointName,CreateTime 
        //      from mfg_exam_subject_item
        //      where  ExamID=@ExamID;
        //
        //select @TempID;
        //");

        //            MySqlParameter[] parameters = {

        //                        new MySqlParameter("@ExamID", MySqlDbType.Int64,20) { Value= model.f_paperid  },        //传入的是 ExamID 选择的试卷模板, 写入用户表的是PaperID用户选中的试卷从paper表自增得到
        //                        new MySqlParameter("@TID", MySqlDbType.Int32,11) { Value= model.f_tid  },
        //                        new MySqlParameter("@TempName", MySqlDbType.VarChar,50) { Value= model.f_tempname  },
        //                        new MySqlParameter("@Phone", MySqlDbType.VarChar,20) { Value= model.f_phone  },
        //                        new MySqlParameter("@Remark", MySqlDbType.VarChar,50) { Value=string.IsNullOrEmpty(model.f_remark)?"":model.f_remark  },
        //                        new MySqlParameter("@MFGID", MySqlDbType.VarChar,40) { Value= string.IsNullOrEmpty(model.f_mfgid)?"":model.f_mfgid  },
        //                        new MySqlParameter("@Gender", MySqlDbType.Int32,11) { Value= model.f_gender  },
        //                        new MySqlParameter("@Age", MySqlDbType.Int32,11) { Value= model.f_age  },
        //                        new MySqlParameter("@School", MySqlDbType.VarChar,255) { Value= model.f_school  },
        //                        new MySqlParameter("@Adddress", MySqlDbType.VarChar,255) { Value= model.f_adddress  },
        //                        new MySqlParameter("@ExamType", MySqlDbType.Int16,1) { Value= model.f_examtype  }


        //            };
        //            //ar ss=    MySQLHelper.ExecuteSql(strSql.ToString(), parameters);
        //            var ss = MySQLHelper.ExecuteSql(strSql.ToString(), parameters);
        //            using (dbHelper_Source = new MySqlDbHelper(ConnectionConfigHelper.DBConnetionStr))
        //            {
        //                object res = dbHelper_Source.GetScalarFile(strSql.ToString(), parameters);
        //                if (res == null)
        //                    return -1;

        //                return int.Parse(res.ToString());


        //            }




        //        }

        //        /// <summary>
        //        ///  获取学生答题时间
        //        /// </summary>
        //        /// <param name="TempID"></param>
        //        /// <returns></returns>
        //        public int GetStudentExamTime(int TempID)
        //        {
        //            string sql = "SELECT stu.ScheduledTime from mfg_exam_student stu where stu.TempID=@TempID";
        //            MySqlParameter[] parameters = { new MySqlParameter("@TempID", TempID) };

        //            using (dbHelper_Source = new MySqlDbHelper(ConnectionConfigHelper.DBConnetionStr))
        //            {
        //                object o1 = dbHelper_Source.GetScalarFile(sql.ToString(), parameters);
        //                if (o1 == null)
        //                    return -1;

        //                return int.Parse(o1.ToString());

        //            }
        //        }


        //        /// <summary>
        //        /// 获取学生信息
        //        /// </summary>
        //        /// <param name="TempID"></param>
        //        /// <returns></returns>
        //        public ExamStudent GetModel(int TempID)
        //        {

        //            string strSql = @"select TempID, PaperID, TID, TempName, Phone, DelFlag, Remark, MFGID, EditTime, IsEffect,
        //                             IsFile, GradeID, StageID, Gender, Age, School, Adddress, GroupVersion, ExamType, MeasureVersion, 
        //                           ResultLevel, LastUpdateUser, TotalHour, MaterialID, Mversion, ScheduledTime, CreateTime, SourceID, SubjectID 
        //                          from mfg_exam_student  where TempID=@TempID ";
        //            MySqlParameter[] parameters = {
        //                    new MySqlParameter("@TempID", MySqlDbType.Int32,11)         };
        //            parameters[0].Value = TempID;


        //            ExamStudent model = new ExamStudent();


        //            using (dbHelper_Source = new MySqlDbHelper(ConnectionConfigHelper.DBConnetionStr))
        //            {
        //                DataSet ds = dbHelper_Source.GetDataSet(strSql.ToString(), parameters);
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    if (ds.Tables[0].Rows[0]["TempID"].ToString() != "")
        //                    {
        //                        model.f_tempid = int.Parse(ds.Tables[0].Rows[0]["TempID"].ToString());
        //                    }
        //                    if (ds.Tables[0].Rows[0]["PaperID"].ToString() != "")
        //                    {
        //                        model.f_paperid = long.Parse(ds.Tables[0].Rows[0]["PaperID"].ToString());
        //                    }
        //                    if (ds.Tables[0].Rows[0]["TID"].ToString() != "")
        //                    {
        //                        model.f_tid = int.Parse(ds.Tables[0].Rows[0]["TID"].ToString());
        //                    }
        //                    model.f_tempname = ds.Tables[0].Rows[0]["TempName"].ToString();
        //                    model.f_phone = ds.Tables[0].Rows[0]["Phone"].ToString();
        //                    if (ds.Tables[0].Rows[0]["DelFlag"].ToString() != "")
        //                    {
        //                        model.f_delflag = int.Parse(ds.Tables[0].Rows[0]["DelFlag"].ToString());
        //                    }
        //                    model.f_remark = ds.Tables[0].Rows[0]["Remark"].ToString();
        //                    model.f_mfgid = ds.Tables[0].Rows[0]["MFGID"].ToString();
        //                    if (ds.Tables[0].Rows[0]["EditTime"].ToString() != "")
        //                    {
        //                        model.f_edittime = DateTime.Parse(ds.Tables[0].Rows[0]["EditTime"].ToString());
        //                    }
        //                    if (ds.Tables[0].Rows[0]["IsEffect"].ToString() != "")
        //                    {
        //                        if ((ds.Tables[0].Rows[0]["IsEffect"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsEffect"].ToString().ToLower() == "true"))
        //                        {
        //                            model.f_iseffect = true;
        //                        }
        //                        else
        //                        {
        //                            model.f_iseffect = false;
        //                        }
        //                    }
        //                    if (ds.Tables[0].Rows[0]["IsFile"].ToString() != "")
        //                    {
        //                        if ((ds.Tables[0].Rows[0]["IsFile"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsFile"].ToString().ToLower() == "true"))
        //                        {
        //                            model.f_isfile = true;
        //                        }
        //                        else
        //                        {
        //                            model.f_isfile = false;
        //                        }
        //                    }
        //                    if (ds.Tables[0].Rows[0]["GradeID"].ToString() != "")
        //                    {
        //                        model.f_gradeid = int.Parse(ds.Tables[0].Rows[0]["GradeID"].ToString());
        //                    }
        //                    if (ds.Tables[0].Rows[0]["StageID"].ToString() != "")
        //                    {
        //                        model.f_stageid = int.Parse(ds.Tables[0].Rows[0]["StageID"].ToString());
        //                    }
        //                    if (ds.Tables[0].Rows[0]["Gender"].ToString() != "")
        //                    {
        //                        model.f_gender = int.Parse(ds.Tables[0].Rows[0]["Gender"].ToString());
        //                    }
        //                    if (ds.Tables[0].Rows[0]["Age"].ToString() != "")
        //                    {
        //                        model.f_age = int.Parse(ds.Tables[0].Rows[0]["Age"].ToString());
        //                    }
        //                    model.f_school = ds.Tables[0].Rows[0]["School"].ToString();
        //                    model.f_adddress = ds.Tables[0].Rows[0]["Adddress"].ToString();
        //                    if (ds.Tables[0].Rows[0]["GroupVersion"].ToString() != "")
        //                    {

        //                        bool res = false;
        //                        if (bool.TryParse(ds.Tables[0].Rows[0]["GroupVersion"].ToString(), out res))
        //                        {
        //                            model.f_groupversion = res ? 1 : 0;
        //                        }
        //                        else
        //                        {
        //                            model.f_groupversion = int.Parse(ds.Tables[0].Rows[0]["GroupVersion"].ToString());
        //                        }



        //                    }
        //                    if (ds.Tables[0].Rows[0]["ExamType"].ToString() != "")
        //                    {
        //                        //  model.f_examtype = int.Parse(ds.Tables[0].Rows[0]["ExamType"].ToString());



        //                        bool res = false;
        //                        if (bool.TryParse(ds.Tables[0].Rows[0]["ExamType"].ToString(), out res))
        //                        {
        //                            model.f_examtype = res ? 1 : 0;
        //                        }
        //                        else
        //                        {
        //                            model.f_examtype = int.Parse(ds.Tables[0].Rows[0]["ExamType"].ToString());
        //                        }

        //                    }
        //                    if (ds.Tables[0].Rows[0]["MeasureVersion"].ToString() != "")
        //                    {
        //                        //  model.f_measureversion = int.Parse(ds.Tables[0].Rows[0]["MeasureVersion"].ToString());

        //                        bool res = false;
        //                        if (bool.TryParse(ds.Tables[0].Rows[0]["MeasureVersion"].ToString(), out res))
        //                        {
        //                            model.f_measureversion = res ? 1 : 0;
        //                        }
        //                        else
        //                        {
        //                            model.f_measureversion = int.Parse(ds.Tables[0].Rows[0]["MeasureVersion"].ToString());
        //                        }

        //                    }
        //                    model.f_resultlevel = ds.Tables[0].Rows[0]["ResultLevel"].ToString();
        //                    if (ds.Tables[0].Rows[0]["LastUpdateUser"].ToString() != "")
        //                    {
        //                        model.f_lastupdateuser = int.Parse(ds.Tables[0].Rows[0]["LastUpdateUser"].ToString());
        //                    }
        //                    if (ds.Tables[0].Rows[0]["TotalHour"].ToString() != "")
        //                    {
        //                        model.f_totalhour = int.Parse(ds.Tables[0].Rows[0]["TotalHour"].ToString());
        //                    }
        //                    model.f_materialid = ds.Tables[0].Rows[0]["MaterialID"].ToString();
        //                    model.f_mversion = ds.Tables[0].Rows[0]["Mversion"].ToString();
        //                    if (ds.Tables[0].Rows[0]["ScheduledTime"].ToString() != "")
        //                    {
        //                        model.f_scheduledtime = decimal.Parse(ds.Tables[0].Rows[0]["ScheduledTime"].ToString());
        //                    }
        //                    if (ds.Tables[0].Rows[0]["CreateTime"].ToString() != "")
        //                    {
        //                        model.f_createtime = DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString());
        //                    }
        //                    if (ds.Tables[0].Rows[0]["SourceID"].ToString() != "")
        //                    {
        //                        model.f_sourceid = int.Parse(ds.Tables[0].Rows[0]["SourceID"].ToString());
        //                    }
        //                    if (ds.Tables[0].Rows[0]["SubjectID"].ToString() != "")
        //                    {
        //                        // model.f_subjectid = int.Parse(ds.Tables[0].Rows[0]["SubjectID"].ToString());

        //                        bool res = false;
        //                        if (bool.TryParse(ds.Tables[0].Rows[0]["SubjectID"].ToString(), out res))
        //                        {
        //                            model.f_subjectid = res ? 1 : 0;
        //                        }
        //                        else
        //                        {
        //                            model.f_subjectid = int.Parse(ds.Tables[0].Rows[0]["SubjectID"].ToString());
        //                        }
        //                    }

        //                    return model;
        //                }
        //                else
        //                {
        //                    return null;
        //                }
        //            }




        //        }

        //        /// <summary>
        //        /// 获取模板试卷id
        //        /// </summary>
        //        /// <param name="ExamID"></param>
        //        /// <returns></returns>
        //        public List<ExamSubjectItem> GetTempItemlist(long ExamID)
        //        {

        //            string sql = @"SELECT ExamItemID,ExamID,ExamKnowID,ParentPointID,ItemID,DiffNum,ItemIndex,ItemSource,PointID,PointName,CreateTime
        //                from mfg_exam_subject_item t WHERE   ExamID=@ExamID ";
        //            List<MySqlParameter> Parameters = new List<MySqlParameter> {
        //                new MySqlParameter("@ExamID",MySqlDbType.Int64) { Value=ExamID}

        //            };
        //            return MySQLHelper.ExecuteStatementList(sql, a =>
        //            {
        //                List<ExamSubjectItem> lis = new List<ExamSubjectItem>();

        //                if (a.HasRows)
        //                {
        //                    while (a.Read())
        //                    {
        //                        ExamSubjectItem item = new ExamSubjectItem();
        //                        item.f_examitemid = a.GetInt32(0);
        //                        item.f_examid = a.GetInt32(1);
        //                        item.f_examknowid = a.GetInt32(2);
        //                        item.f_parentpointid = a.GetString(3);
        //                        item.f_itemid = a.GetInt32(4);
        //                        item.f_diffnum = a.GetInt32(5);
        //                        item.f_itemindex = a.GetInt32(6);
        //                        item.f_itemsource = a.GetInt32(7);
        //                        item.f_pointid = a.GetString(8);
        //                        item.f_pointname = a.GetString(9);
        //                        item.f_createtime = a.GetDateTime(10);
        //                        lis.Add(item);
        //                    }


        //                }

        //                return lis;

        //            }, Parameters);

        //        }

        //        /// <summary>
        //        /// 获取学科测评试卷列表
        //        /// </summary>
        //        /// <param name="TeachID"></param>
        //        /// <param name="GradeID"></param>
        //        /// <param name="SubjectID"></param>
        //        /// <param name="PageIndex"></param>
        //        /// <returns></returns>
        //        public ExamSubjectTemplatePageInfo GetExamList(int TeachID, int GradeID, int SubjectID, int PageIndex)
        //        {
        //            /*
        //             1.获取教师的全部试卷
        //             2.获取指定老师的当前页试卷

        //             */
        //            string sql = @"SELECT COUNT(1) from (
        //SELECT st.ExamID,st.ExamName,st.TID,st.LastTID,st.StageID,st.GradeID,st.ExamTerm,
        //st.SubjectID,st.ScheduledTime,st.IsEnable,st.IsDelete,st.CreateTime,st.LastUpdateTime,
        //st.MaterialID,st.Mversion,st.ExamVersion,st.Remarks,st.SourceID
        // from mfg_base_userinfo ui INNER JOIN mfg_exam_subject_template st on ui.accountnumber=st.TID
        //where  ui.orgid =(SELECT t.orgid from mfg_base_userinfo t  WHERE t.accountnumber=@TeachID)   and (st.GradeID=@GradeID or @GradeID=0)
        //and (st.SubjectID=@SubjectID or @SubjectID=0)and st.IsEnable=1 and st.IsDelete=0
        //) t1;
        // 
        //set @rownum:=@pageIndex*10;
        //select t.*,@rownum:=@rownum+1 as rownum from(
        //SELECT st.ExamID,st.ExamName,st.TID,st.LastTID,st.StageID,st.GradeID,st.ExamTerm,
        //st.SubjectID,st.ScheduledTime,st.IsEnable,st.IsDelete,st.CreateTime,st.LastUpdateTime,
        //st.MaterialID,st.Mversion,st.ExamVersion,st.Remarks,st.SourceID,ui.`name`
        // from mfg_base_userinfo ui INNER JOIN mfg_exam_subject_template st on ui.accountnumber=st.TID
        //where ui.orgid =(SELECT t.orgid from mfg_base_userinfo t  WHERE t.accountnumber=@TeachID)  and (st.GradeID=@GradeID or @GradeID=0)
        //and (st.SubjectID=@SubjectID or @SubjectID=0)and st.IsEnable=1 and st.IsDelete=0
        // ORDER BY st.LastUpdateTime desc) t
        //LIMIT @PageIndex,10;";
        //            List<MySqlParameter> parameters = new List<MySqlParameter> {
        //            new MySqlParameter("@TeachID",TeachID),
        //            new MySqlParameter("@GradeID",GradeID),
        //            new MySqlParameter("@SubjectID",SubjectID),
        //            new MySqlParameter("@PageIndex",PageIndex-1)
        //            };


        //            return MySQLHelper.ExecuteStatementList<ExamSubjectTemplatePageInfo>(sql.ToString(), a =>
        //            {



        //                ExamSubjectTemplatePageInfo item = new ExamSubjectTemplatePageInfo();
        //                item.PageContent = new List<ExamSubjectTemplateExt>();

        //                if (a.HasRows)
        //                {
        //                    while (a.Read())
        //                    {
        //                        item.PageCount = a.GetInt32(0);
        //                    }

        //                }

        //                if (a.NextResult())
        //                {
        //                    if (a.HasRows)
        //                        while (a.Read())
        //                        {
        //                            if (a == null) continue;


        //                            ExamSubjectTemplateExt model = new ExamSubjectTemplateExt();

        //                            model.f_examid = a.GetInt32(0);
        //                            model.f_examname = a.IsDBNull(1) ? "" : a.GetString(1);
        //                            model.f_tid = a.IsDBNull(2) ? "" : a.GetString(2);
        //                            model.f_lasttid = a.IsDBNull(3) ? "" : a.GetString(3);
        //                            model.f_stageid = a.IsDBNull(4) ? 0 : a.GetInt32(4);
        //                            model.f_gradeid = a.IsDBNull(5) ? 0 : a.GetInt32(5);
        //                            model.f_examterm = a.IsDBNull(6) ? 0 : a.GetInt32(6);
        //                            model.f_subjectid = a.IsDBNull(7) ? 0 : a.GetInt32(7);
        //                            model.f_scheduledtime = a.IsDBNull(8) ? 0 : a.GetDecimal(8);
        //                            model.f_isenable = a.IsDBNull(9) ? false : a.GetBoolean(9);
        //                            model.f_isdelete = a.IsDBNull(10) ? false : a.GetBoolean(10);
        //                            model.f_createtime = a.IsDBNull(11) ? DateTime.MinValue : a.GetDateTime(11);
        //                            model.f_lastupdatetime = a.IsDBNull(12) ? DateTime.MinValue : a.GetDateTime(12);
        //                            model.f_materialid = a.IsDBNull(13) ? "" : a.GetString(13);
        //                            model.f_mversion = a.IsDBNull(14) ? "" : a.GetString(14);
        //                            model.f_examversion = a.IsDBNull(15) ? 0 : a.GetInt32(15);
        //                            model.f_remarks = a.IsDBNull(16) ? "" : a.GetString(16);
        //                            model.f_sourceid = a.IsDBNull(17) ? 0 : a.GetInt32(17);
        //                            model.rownum = a.GetInt32(19);
        //                            model.TeachName = a.GetString(18);
        //                            model.ShowTime = model.f_lastupdatetime.ToString();

        //                            item.PageContent.Add(model);

        //                        }


        //                }
        //                item.PageHelper = PageNavHelper.ShowPage(10, PageIndex, item.PageCount);
        //                return item;
        //            }, parameters);


        //        }

        //        /// <summary>
        //        /// 获取本地试卷的试题id集合
        //        /// </summary>
        //        /// <param name="PaperID"></param>
        //        /// <returns></returns>
        //        public List<ExamSubjectPaperItem> GetQuestionIdList(long PaperID)
        //        {

        //            List<MySqlParameter> parameters = new List<MySqlParameter> {
        //            new MySqlParameter("@PaperID",PaperID) };

        //            string sql = @"SELECT PaperItemID,PaperID,ExamKnowID,ExamItemID,ExamID,ParentPointID,ItemID,DiffNum,ItemIndex,ItemSource,PointID,PointName,CreateTime
        // from mfg_exam_subject_paper_item
        //where PaperID = @PaperID";
        //            List<ExamSubjectPaperItem> list = new List<ExamSubjectPaperItem>();

        //            return MySQLHelper.ExecuteStatementList<List<ExamSubjectPaperItem>>(sql.ToString(), a =>
        //            {


        //                if (a.HasRows)
        //                {

        //                    while (a.Read())
        //                    {
        //                        ExamSubjectPaperItem item = new ExamSubjectPaperItem();
        //                        item.f_paperitemid = a.IsDBNull(0) ? 0 : a.GetInt64(0);
        //                        item.f_paperid = a.IsDBNull(1) ? 0 : a.GetInt64(1);
        //                        item.f_examknowid = a.IsDBNull(2) ? 0 : a.GetInt32(2);
        //                        item.f_examitemid = a.IsDBNull(3) ? 0 : a.GetInt32(3);
        //                        item.f_examid = a.IsDBNull(4) ? 0 : a.GetInt32(4);
        //                        item.f_parentpointid = a.IsDBNull(5) ? "" : a.GetString(5);
        //                        item.f_itemid = a.IsDBNull(6) ? 0 : a.GetInt32(6);
        //                        item.f_diffnum = a.IsDBNull(7) ? 0 : a.GetInt32(7);
        //                        item.f_itemindex = a.IsDBNull(8) ? 0 : a.GetInt32(8);
        //                        item.f_itemsource = a.IsDBNull(9) ? 0 : a.GetInt32(9);
        //                        item.f_pointid = a.IsDBNull(10) ? "" : a.GetString(10);
        //                        item.f_pointname = a.IsDBNull(11) ? "" : a.GetString(11);
        //                        item.f_createtime = a.GetDateTime(12);
        //                        list.Add(item);
        //                    }

        //                }


        //                return list;
        //            }, parameters);
        //        }
        //        /// <summary>
        //        /// 获取选中试卷
        //        /// </summary>
        //        /// <param name="PaperID"></param>
        //        /// <returns></returns>
        //        public ExamSubjectPaper GetPaperModel(long PaperID)
        //        {
        //            string sql = @"select PaperID, ExamID, ExamName, TID, LastTID, StageID, GradeID, ExamTerm, SubjectID, 
        //                        ScheduledTime, IsEnable, IsDelete, CreateTime, LastUpdateTime, MaterialID, Mversion, ExamVersion, Remarks, SourceID
        //                        from mfg_exam_subject_paper
        //                        where PaperID =@PaperID";

        //            MySqlParameter[] parameters = new MySqlParameter[]{
        //            new MySqlParameter("@PaperID",PaperID) };

        //            using (dbHelper_Source = new MySqlDbHelper(ConnectionConfigHelper.DBConnetionStr))
        //            {
        //                return dbHelper_Source.GetDataInfo<ExamSubjectPaper>(sql, a =>
        //                {
        //                    ExamSubjectPaper item = new ExamSubjectPaper();
        //                    item.f_paperid = a.GetInt64(0);
        //                    item.f_examid = a.GetInt32(1);
        //                    item.f_examname = a.GetString(2);
        //                    item.f_tid = a.GetString(3);
        //                    item.f_lasttid = a.GetString(4);
        //                    item.f_stageid = a.GetInt32(5);
        //                    item.f_gradeid = a.GetInt32(6);
        //                    item.f_examterm = a.GetInt32(7);
        //                    item.f_subjectid = a.GetInt32(8);
        //                    item.f_scheduledtime = a.GetDecimal(9);
        //                    item.f_isenable = a.GetBoolean(10);
        //                    item.f_isdelete = a.GetBoolean(11);
        //                    item.f_createtime = a.GetDateTime(12);
        //                    item.f_lastupdatetime = a.GetDateTime(13);
        //                    item.f_materialid = a.GetString(14);
        //                    item.f_mversion = a.GetString(15);
        //                    item.f_examversion = a.GetInt32(16);
        //                    item.f_remarks = a.GetString(17);
        //                    item.f_sourceid = a.GetInt32(18);
        //                    return item;

        //                }, parameters);



        //            }

        //        }










        //        #region 学科测评报告
        //        /// <summary>获取学科模型方法
        //        /// </summary>
        //        /// <param name="paperId">测评ID</param>
        //        /// <param name="tempId">学生ID</param>
        //        /// <returns>学科测评报告模型</returns>
        //        public KnowledgeSubjectPointModel GetSubjectReport(string paperId, string tempId)
        //        {
        //            #region sql
        //            StringBuilder stringBuilder = new StringBuilder();

        //            stringBuilder.Append(@"SELECT
        //	                                A.TempID,
        //	                                A.TempName,
        //	                                A.Phone,
        //	                                A.GradeID,
        //	                                A.StageID,
        //	                                A.Age,
        //	                                A.School,
        //	                                A.Adddress,
        //	                                A.Gender,
        //	                                A.SubjectID,
        //	                                A.TotalHour,
        //	                                A.createtime
        //                                FROM
        //	                                mfg_exam_student A 
        //                                WHERE A.TempID=@TempID ;");
        //            stringBuilder.Append(@"SELECT
        //	                                    B.TempID,
        //	                                    B.PaperID,
        //	                                    B.ParentPointID,
        //	                                    B.ParentPointName,
        //									    S.ResultLevel,
        //	                                    B.PointID,
        //	                                    B.PointName,
        //	                                    B.PTotalCount,
        //	                                    B.PRightCount,
        //	                                    B.CreateTime,
        //	                                    B.Degree,
        //	                                    B.EstimateValue,
        //	                                    B.PointLevel
        //                                    FROM
        //	                                    mfg_exam_subject_grasp B INNER JOIN mfg_exam_student S ON B.TempID=S.TempID 
        //                                    where B.TempID=@TempID AND B.PaperID=@PaperID;");
        //            stringBuilder.Append(@"SELECT
        //	                                    B.TempID,
        //	                                    B.PaperID,
        //	                                    B.DiffictyCode,
        //	                                    B.DiffictyName,
        //	                                    B.TotalCount,
        //	                                    B.RightCount,
        //	                                    B.AnswerTime,
        //	                                    B.RightRate,
        //	                                    CASE
        //                                     WHEN B.IsUpdate=0 THEN
        //	                                    B.ExpectRate
        //                                    ELSE
        //	                                    B.UpdateExpectRate
        //                                    END AS ExpectRate,
        //                                     B.UpdateExpectRate,
        //                                     B.IsUpdate,
        //                                     B.CreateTime
        //                                    FROM
        //	                                    mfg_exam_subject_diff B 
        //                                        where B.TempID=@TempID and  B.PaperID=@PaperID;
        //                                        ");

        //            #endregion
        //            List<MySqlParameter> p = new List<MySqlParameter> {
        //                                     new MySqlParameter("@PaperID", MySqlDbType.String) { Direction = ParameterDirection.Input, Value = paperId },
        //                                     new MySqlParameter("@TempID", MySqlDbType.String) { Direction = ParameterDirection.Input, Value = tempId }
        //                                 };
        //            var knowsubjectModel = new KnowledgeSubjectPointModel();
        //            knowsubjectModel.SujectPointList = new List<SubjectPointModel>();
        //            knowsubjectModel.SubjectDiffyList = new List<SubjectDifficyModel>();
        //            MySQLHelper.ExecuteStatementList<KnowledgeSubjectPointModel>(stringBuilder.ToString(), a =>
        //            {
        //                if (a.HasRows)
        //                {
        //                    while (a.Read())
        //                    {
        //                        knowsubjectModel.TempID = a.GetString(0);
        //                        knowsubjectModel.TempName = a.GetString(1);
        //                        knowsubjectModel.Phone = a.GetString(2);
        //                        knowsubjectModel.GradeID = a.GetString(3);
        //                        knowsubjectModel.StageID = a.GetString(4);
        //                        knowsubjectModel.Age = a.GetString(5);
        //                        knowsubjectModel.School = a.GetString(6);
        //                        knowsubjectModel.Adddress = a.GetString(7);
        //                        knowsubjectModel.Gender = a.GetString(8);
        //                        knowsubjectModel.SubjectID = a.IsDBNull(9) ? "0" : a.GetByte(9).ToString();
        //                        knowsubjectModel.TotalHour = a.IsDBNull(10) ? "0" : a.GetString(10);
        //                        knowsubjectModel.CreateTime = a.GetDateTime(11);
        //                    }
        //                }
        //                if (a.NextResult())
        //                {
        //                    if (a.HasRows)
        //                    {

        //                        while (a.Read())
        //                        {
        //                            knowsubjectModel.SujectPointList.Add(new SubjectPointModel()
        //                            {
        //                                TempID = a.GetString(0),
        //                                PaperID = a.GetString(1),
        //                                ParentPointID = a.GetString(2),
        //                                ParentPointName = a.GetString(3),
        //                                ResultLevel = a.GetString(4),
        //                                PointID = a.GetString(5),
        //                                PointName = a.GetString(6),
        //                                PTotalCount = a.IsDBNull(7) ? string.Empty : a.GetString(7),
        //                                PRightCount = a.IsDBNull(8) ? string.Empty : a.GetString(8),
        //                                CreateTime = a.GetString(9),
        //                                Degree = a.IsDBNull(10) ? string.Empty : a.GetString(10),
        //                                EstimateValue = a.IsDBNull(10) ? string.Empty : a.GetString(11),
        //                                PointLevel = a.IsDBNull(10) ? string.Empty : a.GetString(12)
        //                            });
        //                        }
        //                    }

        //                }
        //                if (a.NextResult())
        //                {
        //                    if (a.HasRows)
        //                    {

        //                        while (a.Read())
        //                        {
        //                            knowsubjectModel.SubjectDiffyList.Add(new SubjectDifficyModel()
        //                            {
        //                                TempID = a.IsDBNull(0) ? string.Empty : a.GetString(0),
        //                                PaperID = a.IsDBNull(1) ? string.Empty : a.GetString(1),
        //                                DiffictyCode = a.IsDBNull(2) ? string.Empty : a.GetString(2),
        //                                DiffictyName = a.IsDBNull(3) ? string.Empty : a.GetString(3),
        //                                TotalCount = a.IsDBNull(4) ? string.Empty : a.GetString(4),
        //                                RightCount = a.IsDBNull(5) ? string.Empty : a.GetString(5),
        //                                AnswerTime = a.IsDBNull(6) ? string.Empty : a.GetString(6),
        //                                RightRate = a.IsDBNull(7) ? string.Empty : a.GetString(7),
        //                                ExpectRate = a.IsDBNull(8) ? string.Empty : a.GetString(8),
        //                                UpdateExpectRate = a.IsDBNull(9) ? string.Empty : a.GetString(9),
        //                                IsUpdate = a.IsDBNull(10) ? "0" : a.GetString(10),
        //                                CreateTime = a.GetString(11),

        //                            });
        //                        }
        //                    }
        //                }

        //                return knowsubjectModel;

        //            }, p);

        //            var subjectList = knowsubjectModel.SujectPointList;
        //            int flagi = 0;
        //            if (subjectList != null)
        //                foreach (var item in subjectList)
        //                {
        //                    flagi++;
        //                    if (item.PointLevel == "必考")
        //                    {
        //                        long iticks = DateTime.Now.Ticks;
        //                        Random ran = new Random((int)iticks * flagi);
        //                        item.KaoPin = ran.Next(76, 100).ToString();
        //                    }
        //                    else if (item.PointLevel == "常考")
        //                    {
        //                        long iticks = DateTime.Now.Ticks;
        //                        Random ran = new Random((int)iticks * flagi);
        //                        item.KaoPin = ran.Next(51, 75).ToString();
        //                    }
        //                    else if (item.PointLevel == "易考")
        //                    {
        //                        long iticks = DateTime.Now.Ticks;
        //                        Random ran = new Random((int)iticks * flagi);
        //                        item.KaoPin = ran.Next(26, 50).ToString();
        //                    }
        //                    else if (item.PointLevel == "不考")
        //                    {
        //                        long iticks = DateTime.Now.Ticks;
        //                        Random ran = new Random((int)iticks * flagi);
        //                        item.KaoPin = ran.Next(0, 25).ToString();
        //                    }

        //                }
        //            //防止重复数据
        //            var difflist = knowsubjectModel.SubjectDiffyList;
        //            var grouplist = difflist.GroupBy(x => x.DiffictyName).Distinct();
        //            List<SubjectDifficyModel> listdiffmodel = new List<SubjectDifficyModel>();
        //            foreach (var item in grouplist)
        //            {
        //                listdiffmodel.Add(difflist.Where(x => x.DiffictyName == item.Key).FirstOrDefault());

        //            }
        //            knowsubjectModel.SubjectDiffyList = listdiffmodel.OrderBy(x => x.DiffictyCode).ToList();

        //            return knowsubjectModel;




        //        }

        //        /// <summary>获取测评打印设置内容
        //        /// </summary>
        //        /// <param name="paperIdList">测评ID列表</param>
        //        /// <returns>测评打印设置内容</returns>
        //        public ReportConteSetModel GetConteSetModel(string paperIdList)
        //        {
        //            ReportConteSetModel reportsetmodel = new ReportConteSetModel();
        //            List<HourClassModel> HourClassList = new List<HourClassModel>();
        //            List<DiffClassModel> DiffClassList = new List<DiffClassModel>();
        //            StringBuilder strSql = new StringBuilder();
        //            strSql.AppendFormat(@"SELECT
        //	                                    A.SubjectID,
        //	                                    A.TotalHour,
        //	                                    B.PaperID,
        //	                                    B.PointID,
        //	                                    B.PointName,
        //	                                    B.ClassHour
        //                                    FROM
        //	                                    mfg_exam_student A
        //                                    INNER JOIN  mfg_exam_subject_paper_point B ON A.PaperID = B.PaperID  and a.ExamType=0 
        //                                    where B.IsUse=1 and  B.PaperID in({0}) ORDER BY  A.CreateTime DESC;", paperIdList);
        //            strSql.AppendFormat(@"SELECT
        //                                            A.SubjectID,
        //                                            B.PaperID,
        //	                                        B.DiffictyName,
        //	                                        SUM(B.TotalCount) AS TotalCount,
        //	                                        SUM(B.RightCount) AS RightCount,
        //	                                        B.ExpectRate,
        //	                                        B.UpdateExpectRate,
        //	                                        B.IsUpdate
        //                                        FROM
        //                                        mfg_exam_student A  INNER JOIN
        //	                                        mfg_exam_subject_diff B
        //                                        ON A.PaperID=B.PaperID  and a.ExamType=0 
        //                                        WHERE
        //	                                        A.PaperID IN ({0})
        //                                        GROUP BY B.PaperID,B.DiffictyName 
        //                                        ORDER BY A.CreateTime DESC;", paperIdList);
        //            strSql.AppendFormat(@"SELECT DISTINCT DiffictyName from mfg_exam_subject_diff WHERE PaperID in ({0}) ORDER BY DiffictyCode;", paperIdList);

        //            MySQLHelper.ExecuteStatementList<ReportConteSetModel>(strSql.ToString(), a =>
        //            {

        //                if (a.HasRows)
        //                {
        //                    while (a.Read())
        //                    {
        //                        HourClassModel hmodel = new HourClassModel();
        //                        hmodel.SubjectID = a.IsDBNull(0) ? 0 : a.GetByte(0);
        //                        hmodel.TotalHour = a.IsDBNull(1) ? 0 : a.GetInt32(1);
        //                        hmodel.PaperID = a.IsDBNull(2) ? string.Empty : a.GetString(2);
        //                        hmodel.PointID = a.IsDBNull(3) ? string.Empty : a.GetString(3);
        //                        hmodel.PointName = a.IsDBNull(4) ? string.Empty : a.GetString(4);
        //                        hmodel.ClassHour = a.IsDBNull(5) ? 0 : a.GetInt32(5);
        //                        HourClassList.Add(hmodel);
        //                    }
        //                }
        //                if (a.NextResult())
        //                {
        //                    if (a.HasRows)
        //                    {
        //                        while (a.Read())
        //                        {
        //                            DiffClassModel dmodel = new DiffClassModel();
        //                            dmodel.SubjectID = a.IsDBNull(0) ? 0 : a.GetByte(0);
        //                            dmodel.PaperID = a.IsDBNull(1) ? string.Empty : a.GetString(1);
        //                            dmodel.DiffictyName = a.IsDBNull(2) ? string.Empty : a.GetString(2);
        //                            dmodel.TotalCount = a.IsDBNull(3) ? 0 : a.GetInt32(3);
        //                            dmodel.RightCount = a.IsDBNull(4) ? 0 : a.GetInt32(4);
        //                            dmodel.ExpectRate = a.IsDBNull(5) ? string.Empty : a.GetString(5);
        //                            dmodel.UpdateExpectRate = a.IsDBNull(6) ? string.Empty : a.GetString(6);
        //                            dmodel.IsUpdate = a.IsDBNull(7) ? 0 : a.GetInt32(7);
        //                            DiffClassList.Add(dmodel);
        //                        }
        //                    }
        //                }
        //                if (a.NextResult())
        //                {
        //                    if (a.HasRows)
        //                    {
        //                        reportsetmodel.DiffictyName = new List<string>();
        //                        while (a.Read())
        //                        {
        //                            reportsetmodel.DiffictyName.Add(a.GetString(0));
        //                        }
        //                    }
        //                }
        //                return reportsetmodel;
        //            }, null);

        //            reportsetmodel.HourClassList = HourClassList;
        //            reportsetmodel.DiffClassList = DiffClassList;

        //            return reportsetmodel;
        //        }


        //        /// <summary>保存设置内容
        //        /// </summary>
        //        /// <param name="reportsetModel">测评设置内容</param>
        //        /// <returns>保存结果</returns>
        //        public bool SaveReportSet(ReportSetSaveModel reportsetModel)
        //        {
        //            StringBuilder strSql = new StringBuilder();
        //            var SetHourList = reportsetModel.SetHourList;
        //            var SetDiffList = reportsetModel.SetDiffList;
        //            //更新课时
        //            if (SetHourList.Count() > 0)
        //            {
        //                foreach (var item in SetHourList)
        //                {
        //                    strSql.AppendFormat(@"update 
        //mfg_exam_student set TotalHour='{0}' where PaperID='{1}'  AND ExamType=0; ", item.TotalHour, item.PaperID);

        //                }
        //            }
        //            //更新难度
        //            if (SetDiffList.Count() > 0)
        //            {
        //                foreach (var diff in SetDiffList)
        //                {
        //                    strSql.AppendFormat(@"update mfg_exam_subject_diff set UpdateExpectRate='{0}',IsUpdate=1 where PaperID='{1}' AND DiffictyName='{2}';", diff.ExpertRate, diff.PaperID, diff.DiffName);
        //                }
        //            }

        //            return MySQLHelper.ExecuteSql(strSql.ToString()) > 0;
        //        }

        //        #endregion


        //        #region 获取学科试卷详情 包含学生答题信息

        //        /// <summary>获取学科试卷详情 包含学生答题信息
        //        /// </summary>
        //        /// <param name="paperId">测评试卷ID</param>
        //        /// <returns></returns>
        //        public ExamSubjectInfoModel GetSubjectExamModel(string paperId)
        //        {

        //            ExamSubjectInfoModel _knowassessmentModel = new ExamSubjectInfoModel();
        //            _knowassessmentModel.PaperID = paperId;
        //            _knowassessmentModel.tarelitemmodelList = GetItemModelList(paperId);
        //            _knowassessmentModel.tarelknomodelList = GetPaperKnowModelList(paperId);
        //            _knowassessmentModel.tanswermodelList = GetPaperAnswerModelList(paperId);
        //            _knowassessmentModel.SubjectID = _knowassessmentModel.tarelitemmodelList.Any() ? _knowassessmentModel.tarelitemmodelList.First().SubjectId : 0;
        //            return _knowassessmentModel;

        //        }

        //        /// <summary>
        //        /// 根据测评ID获取测评试卷详情
        //        /// </summary>
        //        /// <param name="id"></param>
        //        /// <returns></returns>
        //        public List<PaperRelItemModel> GetItemModelList(string paperId)
        //        {
        //            List<PaperRelItemModel> list = new List<PaperRelItemModel>();
        //            StringBuilder strSql = new StringBuilder();
        //            strSql.Append(@"SELECT
        //	                            a.PaperID,
        //	                            a.ParentPointID,
        //                                a.ItemID,
        //	                            a.DiffNum,
        //	                            a.ItemIndex,
        //	                            a.ItemSource,
        //	                            a.PointID,
        //	                            a.PointName,
        //                                b.SubjectID
        //                            FROM
        //	                            mfg_exam_subject_paper_item a  
        //                                INNER JOIN mfg_exam_subject_paper b on a.PaperID=b.PaperID ");
        //            strSql.Append(" where a.PaperID=@PaperID ORDER BY ItemIndex  ");
        //            MySqlParameter[] parameters = {
        //                new MySqlParameter("@PaperID", MySqlDbType.VarChar,40) { Value = paperId }
        //            };

        //            list = MySQLHelper.ExecuteStatementList<List<PaperRelItemModel>>(strSql.ToString(), a =>
        //            {

        //                if (a.HasRows)
        //                {
        //                    while (a.Read())
        //                    {
        //                        if (a == null) continue;
        //                        PaperRelItemModel model = new PaperRelItemModel();
        //                        model.PaperID = a.IsDBNull(0) ? "0" : a.GetString(0);
        //                        model.ParentPointID = a.IsDBNull(1) ? string.Empty : a.GetString(1);
        //                        model.ItemID = a.IsDBNull(2) ? 0 : a.GetInt32(2);
        //                        model.DiffNum = a.IsDBNull(3) ? 0 : a.GetInt32(3);
        //                        model.ItemIndex = a.IsDBNull(4) ? 0 : a.GetInt32(4);
        //                        model.ItemSource = a.IsDBNull(5) ? 0 : a.GetInt32(5);
        //                        model.PointID = a.IsDBNull(6) ? string.Empty : a.GetString(6);
        //                        model.PointName = a.IsDBNull(7) ? string.Empty : a.GetString(7);
        //                        model.SubjectId = a.IsDBNull(8) ? 0 : a.GetByte(8);
        //                        list.Add(model);
        //                    }
        //                }
        //                return list;

        //            }, parameters.ToList());

        //            return list;


        //        }


        //        /// <summary>
        //        /// 根据测评ID获取知识点列表
        //        /// </summary>
        //        /// <param name="paperId"></param>
        //        /// <returns></returns>
        //        public List<PaperRelKnowModel> GetPaperKnowModelList(string paperId)
        //        {
        //            List<PaperRelKnowModel> list = new List<PaperRelKnowModel>();
        //            StringBuilder strSql = new StringBuilder();
        //            strSql.Append(@"SELECT
        //	                            a.PaperID,
        //	                            a.PointID,
        //	                            a.PointName,
        //                                a.DiffNum,
        //	                            a.ClassHour,
        //	                            a.DefaultHour
        //                            FROM
        //	                            mfg_exam_subject_paper_point a ");
        //            strSql.Append(" where a.PaperID=@PaperID  ");
        //            MySqlParameter[] parameters = {
        //                     new MySqlParameter("@PaperID", MySqlDbType.VarChar,40) { Value=paperId} };
        //            list = MySQLHelper.ExecuteStatementList<List<PaperRelKnowModel>>(strSql.ToString(), a =>
        //            {

        //                if (a.HasRows)
        //                {
        //                    while (a.Read())
        //                    {
        //                        if (a == null) continue;
        //                        PaperRelKnowModel model = new PaperRelKnowModel();
        //                        model.PaperID = a.IsDBNull(0) ? "0" : a.GetString(0);
        //                        model.PointID = a.IsDBNull(1) ? string.Empty : a.GetString(1);
        //                        model.PointName = a.IsDBNull(2) ? string.Empty : a.GetString(2);
        //                        model.DiffNum = a.IsDBNull(3) ? 0 : a.GetInt32(3);
        //                        model.ClassHour = a.IsDBNull(4) ? 0 : a.GetInt32(4);
        //                        model.DefaultHour = a.IsDBNull(5) ? 0 : a.GetInt32(5);
        //                        list.Add(model);
        //                    }
        //                }
        //                return list;

        //            }, parameters.ToList());
        //            return list;
        //        }


        //        /// <summary>
        //        /// 获取测评试卷答题详情
        //        /// </summary>
        //        /// <param name="taid"></param>
        //        /// <returns></returns>
        //        public List<PaperAnswerModel> GetPaperAnswerModelList(string paperId)
        //        {
        //            List<PaperAnswerModel> list = new List<PaperAnswerModel>();
        //            StringBuilder strSql = new StringBuilder();
        //            strSql.Append(@"SELECT
        //	                            A.PaperID,
        //	                            A.ParentPointID ,
        //	                            A.ItemID ,
        //	                            B.Accuracy,
        //	                            B.Answer,
        //	                            B.ItemSource,
        //	                            B.CreateTime
        //                            FROM
        //	                            mfg_exam_subject_paper_item A
        //                            LEFT JOIN mfg_exam_subject_answer B ON A.PaperItemID = B.PaperItemID ");
        //            strSql.Append(" WHERE A.PaperID=@PaperID ");

        //            MySqlParameter[] parameters = {
        //                     new MySqlParameter("@PaperID", MySqlDbType.VarChar,40) { Value=paperId} };
        //            list = MySQLHelper.ExecuteStatementList<List<PaperAnswerModel>>(strSql.ToString(), a =>
        //            {

        //                if (a.HasRows)
        //                {
        //                    while (a.Read())
        //                    {
        //                        if (a == null) continue;
        //                        PaperAnswerModel model = new PaperAnswerModel();
        //                        model.PaperID = a.IsDBNull(0) ? "0" : a.GetString(0);
        //                        model.ParentPointID = a.IsDBNull(1) ? string.Empty : a.GetString(1);
        //                        model.ItemID = a.IsDBNull(2) ? 0 : a.GetInt32(2);
        //                        model.Accuracy = a.IsDBNull(3) ? 0 : a.GetInt32(3);
        //                        model.Answer = a.IsDBNull(4) ? string.Empty : a.GetString(4);
        //                        model.ItemSource = a.IsDBNull(5) ? 0 : a.GetInt32(5);
        //                        model.CreateTime = a.IsDBNull(6) ? DateTime.Now : a.GetDateTime(6);
        //                        list.Add(model);
        //                    }
        //                }
        //                return list;

        //            }, parameters.ToList());
        //            return list;
        //        }

        //        #endregion
    }
}
