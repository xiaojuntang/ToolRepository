using System;
using System.Collections.Generic;
using System.Linq;

namespace Mfg.Resource.Comm
{
    /// <summary>
    /// 资源公共类库
    /// dec 对整个资源体系提供 公用方法
    /// 科目
    /// 大年年级
    /// 小年级
    /// 学制
    /// </summary>
    public sealed class ResourceComFunc
    {
        /// <summary>
        /// 年级列表
        /// </summary>
        public static readonly List<ClassEntity> GradeList = null;
        /// <summary>
        /// 科目列表
        /// </summary>
        public static readonly List<SubjectEntity> SubejctList = null;
        /// <summary>
        /// 大年年级列表
        /// </summary>
        public static readonly List<BClassEntity> BGradeList = null;
        /// <summary>
        /// 学制
        /// </summary>
        public static readonly List<EduEntity> EduList = null;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ResourceComFunc()
        {
            //填充年级数据
            GradeList = new List<ClassEntity>
            {
                new ClassEntity() {ID = "x1", Name = "一年级"},
                new ClassEntity() {ID = "x2", Name = "二年级"},
                new ClassEntity() {ID = "x3", Name = "三年级"},
                new ClassEntity() {ID = "x4", Name = "四年级"},
                new ClassEntity() {ID = "x5", Name = "五年级"},
                new ClassEntity() {ID = "x6", Name = "六年级"},
                new ClassEntity() {ID = "c1", Name = "七年级"},
                new ClassEntity() {ID = "c2", Name = "八年级"},
                new ClassEntity() {ID = "c3", Name = "九年级"},
                new ClassEntity() {ID = "g1", Name = "高一"},
                new ClassEntity() {ID = "g2", Name = "高二"},
                new ClassEntity() {ID = "g3", Name = "高三"}
            };
            //填充科目数据
            SubejctList = new List<SubjectEntity>
            {
                new SubjectEntity() {ID = "01", Name = "语文"},
                new SubjectEntity() {ID = "02", Name = "数学"},
                new SubjectEntity() {ID = "03", Name = "英语"},
                new SubjectEntity() {ID = "04", Name = "物理"},
                new SubjectEntity() {ID = "05", Name = "化学"},
                new SubjectEntity() {ID = "06", Name = "地理"},
                new SubjectEntity() {ID = "07", Name = "历史"},
                new SubjectEntity() {ID = "08", Name = "政治"},
                new SubjectEntity() {ID = "09", Name = "生物"},
                new SubjectEntity() {ID = "10", Name = "科学"},
                new SubjectEntity() {ID = "11", Name = "理综"},
                new SubjectEntity() {ID = "12", Name = "文综"},
                new SubjectEntity() {ID = "13", Name = "历史与社会"},
                new SubjectEntity() {ID = "14", Name = "奥数"}
            };

            //大年级填充数据
            BGradeList = new List<BClassEntity>
            {
                new BClassEntity() {ID = "x", Name = "小学"},
                new BClassEntity() {ID = "c", Name = "初中"},
                new BClassEntity() {ID = "g", Name = "高中"}
            };

            //学制
            EduList = new List<EduEntity>
            {
                 new EduEntity()  {ID =0,Name ="六三制"},
                 new EduEntity()  {ID =1,Name ="五四制"}
            };

        }


        /// <summary>获取大年级
        /// </summary>
        /// <param name="smallGrade">小年级</param>
        /// <param name="edu">学制 0 六三 1五四</param>
        /// <returns>返回大年级</returns>
        public static BClassEntity GetBGrade(string smallGrade, int edu)
        {
            var bclassid = "";
            if (string.IsNullOrEmpty(smallGrade)) return BGradeList.First(t => t.ID == bclassid);
            if (smallGrade == "x6")
            {
                bclassid = edu == 0 ? "x" : "c";
            }
            else
            {
                bclassid = smallGrade.Substring(0, 1);
            }
            return BGradeList.First(t => t.ID == bclassid);
        }

        /// <summary>  获取科目名称
        /// 根据输入科目的id 返回科目的名称
        /// </summary>
        /// /*编写人：mxk  日期：2011-08-04*/
        /// <param name="id">输入科目的Id</param>
        /// <returns>科目的中文名称</returns>
        public static SubjectEntity FindOneSubject(string id)
        {
            return SubejctList.First(s => s.ID == id);
        }

        /// <summary>获取年级名称
        /// 输入年级的科目返回年级的名称
        /// </summary>  
        /// /*编写人：mxk  日期：2011-08-04*/
        /// <param name="id">年级的id</param>
        /// <returns>年级的中文名称</returns>
        public static ClassEntity FindOneClass(string id)
        {
            return GradeList.First(c => c.ID == id);
        }

        /// <summary>获取大年级名称
        /// 输入年级的科目返回年级的名称
        /// </summary>  
        /// /*编写人：mxk  日期：2011-08-04*/
        /// <param name="id">年级的id</param>
        /// <returns>年级的中文名称</returns>
        public static BClassEntity FindOneBClass(string id)
        {
            return BGradeList.First(c => c.ID == id);
        }

        /// <summary>
        /// 根据id获取edu对象
        /// </summary>
        /// <param name="id">学制的id</param>
        /// <returns>返回学制的对象</returns>
        public static EduEntity FindOneEdu(int id)
        {
            return EduList.First(c => c.ID == id);
        }

        /// <summary>
        /// 根据大年或者小年级获取当前可用的 科目列表
        /// </summary>
        /// <param name="bgradeOrgrade">大年或者小年级</param>
        /// <returns>返回集合列表</returns>
        public static List<SubjectEntity> GetSubjectByGrade(string bgradeOrgrade)
        {
            return bgradeOrgrade.IndexOf("x", StringComparison.Ordinal) > -1 ? SubejctList.Take(3).ToList() : SubejctList;
        }
    }
}
