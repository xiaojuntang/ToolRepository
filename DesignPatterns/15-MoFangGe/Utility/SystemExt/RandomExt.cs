
/**************************************************
* 文 件 名：RandomExt.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/2 0:10:15
* 文件说明：随机数扩展类
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System;
using System.Globalization;

namespace Climb.Utility.SystemExt
{
    /// <summary>
    /// 随机类扩展
    /// </summary>
    public static class RandomExt
    {
        #region 生成一个指定范围的随机整数，该随机数范围包括最小值，但不包括最大值
        /// <summary>
        /// 生成一个指定范围的随机整数，该随机数范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="minNum">最小值</param>
        /// <param name="maxNum">最大值</param>
        public static int GetRandomInt(int minNum, int maxNum)
        {
            Random randRandom = new Random(Guid.NewGuid().GetHashCode());
            return randRandom.Next(minNum, maxNum);
        }
        #endregion

        #region 1-int.MaxValue 随机数字
        /// <summary>
        /// 得到一个随机数值
        /// </summary>
        /// <returns></returns>
        public static int GetRandomInt()
        {
            Random randRandom = new Random(Guid.NewGuid().GetHashCode());
            return randRandom.Next(1, int.MaxValue);
        }
        #endregion

        #region 生成一个0.0到1.0的随机小数
        /// <summary>
        /// 生成一个0.0到1.0的随机小数
        /// </summary>
        public static double GetRandomDouble()
        {
            Random randRandom = new Random(Guid.NewGuid().GetHashCode());
            return randRandom.NextDouble();
        }
        #region 对一个数组进行随机排序
        /// <summary>
        /// 对一个数组进行随机排序
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="arr">需要随机排序的数组</param>
        public static void RandomArray<T>(T[] arr)
        {
            //对数组进行随机排序的算法:随机选择两个位置，将两个位置上的值交换
            //交换的次数,这里使用数组的长度作为交换次数
            int count = arr.Length;
            //开始交换
            for (int i = 0; i < count; i++)
            {
                //生成两个随机数位置
                int randomNum1 = GetRandomInt(0, arr.Length);
                int randomNum2 = GetRandomInt(0, arr.Length);

                //定义临时变量
                //交换两个随机数位置的值
                T temp = arr[randomNum1];
                arr[randomNum1] = arr[randomNum2];
                arr[randomNum2] = temp;
            }
        }
        #endregion

        #endregion

        #region 随机bool值
        /// <summary>
        /// 随机返回true 或者false
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static bool NextBool(this Random random)
        {
            return random.NextDouble() > 0.5;
        }
        #endregion

        #region  生成不重复的随机字符串
        /// <summary>
        /// 生成不重复的随机字符串
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static  string CreateCheckCodeNum(int codeCount)
        {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10))));
            }
            return str;
        }
        //方法二：随机生成字符串（数字和字母混和）
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateCheckCode(int codeCount)
        {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString(CultureInfo.InvariantCulture);
            }
            return str;
        }
        #endregion

        #region 生成随机字符码
        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="arrary"></param>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public  static string CreateRandomCode(char [] arrary,int codeCount)
        {
            string randomCode = string .Empty ;
            char[] charAry = arrary.Clone() as char[];
            if (charAry == null) return randomCode;
            int length = charAry.Length;
            for (int i = 0; i < length; i++)
            {
                randomCode += charAry[GetRandomInt(0, length)];
                if (randomCode.Length == codeCount) break;
            }
            return randomCode;
        }

        /// <summary>
        /// 随机数字 字符
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateRandNumberCode(int codeCount)
        {
            return CreateRandomCode(Tools.NumCodeArray, codeCount);
        }
        /// <summary>
        /// 随机大写字符
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateRandUpperCode(int codeCount)
        {
            return CreateRandomCode(Tools.UperCodeArray, codeCount);
        }
        /// <summary>
        /// 随机小写字符
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateRandLowerCode(int codeCount)
        {
            return CreateRandomCode(Tools.LowerCodeArray, codeCount);
        }
        /// <summary>
        /// 随机 字符 大写字母加上 数字
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateRandUperNumberCode(int codeCount)
        {
            char[] combineAry = ArraryExt.CombineArrary(Tools.UperCodeArray, Tools.NumCodeArray);
            return CreateRandomCode(combineAry, codeCount);
        }
        #endregion

        #region 随机枚举
        /// <summary>
        /// 随机一个枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T NextEnum<T>()
    where T : struct
        {
            Random randRandom = new Random(Guid.NewGuid().GetHashCode());
            Type type = typeof(T);
            if (type.IsEnum == false) throw new InvalidOperationException();

            var array = Enum.GetValues(type);
            var index = randRandom.Next(array.GetLowerBound(0), array.GetUpperBound(0) + 1);
            return (T)array.GetValue(index);
        }
        #endregion
    }
}
