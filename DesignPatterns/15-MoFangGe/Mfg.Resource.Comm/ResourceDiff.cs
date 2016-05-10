using System;

namespace Mfg.Resource.Comm
{
    /// <summary>
    /// 难度的公用方法
    /// </summary>
    [Serializable]
    public class QuestionDiff
    {
        /// <summary>
        /// 根据难度数量 转换成难度的名称
        /// </summary>
        /// <param name="diff">diff难度数量</param>
        /// <returns>返回难度名称</returns>
        public static string GetDiffName(int diff)
        {
            var difname = "";
            if (diff <= 40)
            {
                difname = "简单";
            }
            else if (diff > 70)
            {
                difname = "困难";
            }
            else
            {
                difname = "中等";
            }
            return difname;
        }

    }
}
