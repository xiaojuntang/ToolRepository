//Written By ssh.cn at 2015.7.3
using Mfg.Resouce.IDAL;
using Mfg.Resouce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1;

namespace Mfg.Resouce.IDAL
{
    /// <summary>
    /// 题型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IDALQuestionStyle<T> : IDALBase<QuestionStyle<T>>, IAutoIdentity<QuestionStyle<T>>
    {
        /// <summary>
        /// 从主键读取信息
        /// </summary>
        /// <param name="pkID"></param>
        /// <returns>查询到的数据</returns>
        QuestionStyle<T> GetInfoByPK(string pkID);
    }
}
