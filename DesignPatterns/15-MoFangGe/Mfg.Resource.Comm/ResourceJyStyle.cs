using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mfg.Resource.Comm
{
    /// <summary>
    /// 教研的题型获取
    /// </summary>
    public class ResourceJyStyle
    {
        /// <summary>
        /// 根据题型的id 返回教研的题型id
        /// </summary>
        /// <param name="styleid">题型的id</param>
        /// <returns>返回题型的名称</returns>
        public static string GetStyle(int styleid)
        {
            if (styleid <= 100)
            {
                return "选择题";
            }
            if (styleid > 100 && styleid <= 200)
            {
                return "填空题";
            }
            if (styleid > 200 && styleid <= 300)
            {
                return "解答题";
            }
            return "未知题型";
        }

        /// <summary>
        /// 根据题型的id 返回教研的题型id
        /// </summary>
        /// <param name="styleid">题型的id</param>
        /// <returns>返回题型号（1选择题，2填空3解答，4其他）</returns>
        public static int GetQuestionStyle(int styleid)
        {
            if (styleid <= 100)
            {
                return 1;//选择题
            }
            if (styleid > 100 && styleid <= 200)
            {
                return 2;//填空题
            }
            if (styleid > 200 && styleid <= 300)
            {
                return 3;//解答题
            }
            return 4;//未知题型
        }
    }

}
