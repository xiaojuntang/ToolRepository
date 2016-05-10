using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Climb.Utility
{
    /// <summary>
    /// isbetween 扩展
    /// 判断值是否在某个值范伟值之间
    /// </summary>
    public static class IsBetweenExt
    {
       
        /// <summary>
        /// 判断某个值是否在某个值之间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <param name="includeLowerBound"></param>
        /// <param name="includeUpperBound"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public static bool IsBetween<T>(this T t, T lowerBound, T upperBound, bool includeLowerBound = false, bool includeUpperBound = false)
        where T : IComparable<T>
        {
            if (t == null) throw new ArgumentNullException("t");

            var lowerCompareResult = t.CompareTo(lowerBound);
            var upperCompareResult = t.CompareTo(upperBound);

            return (includeLowerBound && lowerCompareResult == 0) ||
                (includeUpperBound && upperCompareResult == 0) ||
                (lowerCompareResult > 0 && upperCompareResult < 0);
        }

        /// <summary>
        /// 判断某个对象的某个属性是否在某个值之间 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <param name="includeLowerBound"></param>
        /// <param name="includeUpperBound"></param>
        /// <returns></returns>
        //BigInt bi = new BigInt();
        //bool b8 = bi.IsBetween(10, 20);
        public static bool IsBetween<T>(this IComparable<T> t, T lowerBound, T upperBound, bool includeLowerBound = false, bool includeUpperBound = false)
        {
            if (t == null) throw new ArgumentNullException("t");

            var lowerCompareResult = t.CompareTo(lowerBound);
            var upperCompareResult = t.CompareTo(upperBound);

            return (includeLowerBound && lowerCompareResult == 0) ||
                (includeUpperBound && upperCompareResult == 0) ||
                (lowerCompareResult > 0 && upperCompareResult < 0);
        }
    }
}
