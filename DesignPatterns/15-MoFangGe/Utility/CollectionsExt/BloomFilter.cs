
/**************************************************
* 文 件 名：BloomFilter.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/7/1 23:59:30
* 文件说明：过滤器 检查元素是否是在集合中
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System;
using System.Collections;
using System.Globalization;
using Climb.Utility.SystemExt;

namespace Climb.Utility.CollectionsExt
{
    /// <summary>
    /// 布隆过滤器 检索一个元素是否在一个集合中
    /// </summary>
    public sealed class BloomFilter
    {
        private static BitArray _bitArray;
        private readonly int _size;
        /// <summary>
        /// 构造函数，初始化分配内存
        /// </summary>
        /// <param name="size">分配的内存大小,必须保证被2整除</param>
        public BloomFilter(int size)
        {
            if (size % 8 == 0)
            {
                _bitArray = new BitArray(size, false);
                _size = size;
            }
            else
            {
                throw new Exception("错误的长度,不能被2整除");
            }

        }

        /// <summary>
        /// 将str加入Bloomfilter，主要是HASH后找到指定位置置true
        /// </summary>
        /// <param name="str">字符串</param>
        public void Add(string str)
        {
            int[] offsetList = GetOffset(str);
            if (offsetList != null)
            {
                Put(offsetList[0]);
                Put(offsetList[1]);
                Put(offsetList[2]);
                Put(offsetList[3]);
                Put(offsetList[4]);
                Put(offsetList[5]);
                Put(offsetList[6]);
                Put(offsetList[7]);
            }
            else
            {
                throw new Exception("字符串不能为空");
            }
        }

        /// <summary>
        /// 判断该字符串是否重复
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>true重复反之则false</returns>
        public bool Contains(string str)
        {
            int[] offsetList = GetOffset(str);
            if (offsetList != null)
            {
                int i = 0;
                while (i < 8)
                {
                    if ((get(offsetList[i]) == false)) { return false; }
                    i++;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回内存块指定位置状态
        /// </summary>
        /// <param name="offset">位置</param>
        /// <returns>状态为TRUE还是FALSE 为TRUE则被占用</returns>
        private bool get(int offset)
        {
            return _bitArray[offset];
        }

        /// <summary>
        /// 改变指定位置状态
        /// </summary>
        /// <param name="offset">位置</param>
        /// <returns>改变成功返回TRUE否则返回FALSE</returns>
        private static void Put(int offset)
        {
            if (_bitArray[offset])
            {
                return;
            }
            _bitArray[offset] = true;
        }

        private int[] GetOffset(string str)
        {
            if (str.IsNullOrEmpty() != true)
            {
                int[] offsetList = new int[8];
                string tmpCode = Hash(str).ToString(CultureInfo.InvariantCulture);
                //    int hashCode = Hash2(tmpCode);
                int hashCode = HashCode.Hash1(tmpCode);
                int offset = Math.Abs(hashCode % (_size / 8) - 1);
                offsetList[0] = offset;
                //   hashCode = Hash3(str);
                hashCode = HashCode.Hash2(tmpCode);
                offset = _size / 4 - Math.Abs(hashCode % (_size / 8)) - 1;
                offsetList[1] = offset;

                hashCode = HashCode.Hash3(tmpCode);
                offset = Math.Abs(hashCode % (_size / 8) - 1) + _size / 4;
                offsetList[2] = offset;
                //   hashCode = Hash3(str);
                hashCode = HashCode.Hash4(tmpCode);
                offset = _size / 2 - Math.Abs(hashCode % (_size / 8)) - 1;
                offsetList[3] = offset;

                hashCode = HashCode.Hash5(tmpCode);
                offset = Math.Abs(hashCode % (_size / 8) - 1) + _size / 2;
                offsetList[4] = offset;
                hashCode = HashCode.Hash6(tmpCode);
                offset = 3 * _size / 4 - Math.Abs(hashCode % (_size / 8)) - 1;
                offsetList[5] = offset;

                hashCode = HashCode.Hash7(tmpCode);
                offset = Math.Abs(hashCode % (_size / 8) - 1) + 3 * _size / 4;
                offsetList[6] = offset;
                //   hashCode = Hash3(str);
                hashCode = HashCode.Hash8(tmpCode);
                offset = _size - Math.Abs(hashCode % (_size / 8)) - 1;
                offsetList[7] = offset;
                return offsetList;
            }
            return null;
        }

        /// <summary>
        /// 内存块大小
        /// </summary>
        public int Size
        {
            get { return _size; }
        }

        /// <summary>
        /// 获取字符串HASHCODE
        /// </summary>
        /// <param name="val">字符串</param>
        /// <returns>HASHCODE</returns>
        private static int Hash(string val)
        {
            return val.GetHashCode();
        }
    }

    /// <summary>
    /// 获取对象的哈希码
    /// </summary>
    static class HashCode
    {

        // BKDR Hash Function
        public static int Hash1(string str)
        {
            const int seed = 131; // 31 131 1313 13131 131313 etc..
            int hash = 0;
            char[] bitarray = str.ToCharArray();
            int count = bitarray.Length;
            while (count > 0)
            {
                hash = hash * seed + (bitarray[bitarray.Length - count]);
                count--;
            }
            return (hash & 0x7FFFFFFF);
        }
        //AP hash function
        public static int Hash2(string str)
        {
            int hash = 0;
            int i;
            char[] bitarray = str.ToCharArray();
            int count = bitarray.Length;
            for (i = 0; i < count; i++)
            {
                if ((i & 1) == 0)
                {
                    hash ^= ((hash << 7) ^ (bitarray[i]) ^ (hash >> 3));
                }
                else
                {
                    hash ^= (~((hash << 11) ^ (bitarray[i]) ^ (hash >> 5)));
                }
                count--;
            }

            return (hash & 0x7FFFFFFF);
        }

        //SDBM Hash function
        public static int Hash3(string str)
        {
            int hash = 0;
            char[] bitarray = str.ToCharArray();
            int count = bitarray.Length;
            while (count > 0)
            {
                hash = (bitarray[bitarray.Length - count]) + (hash << 6) + (hash << 16) - hash;
                count--;
            }

            return (hash & 0x7FFFFFFF);
        }

        // RS Hash Function
        public static int Hash4(string str)
        {
            const int b = 378551;
            int a = 63689;
            int hash = 0;
            char[] bitarray = str.ToCharArray();
            int count = bitarray.Length;
            while (count > 0)
            {
                hash = hash * a + (bitarray[bitarray.Length - count]);
                a *= b;
                count--;
            }

            return (hash & 0x7FFFFFFF);
        }

        // JS Hash Function
        public static int Hash5(string str)
        {
            int hash = 1315423911;
            char[] bitarray = str.ToCharArray();
            int count = bitarray.Length;
            while (count > 0)
            {
                hash ^= ((hash << 5) + (bitarray[bitarray.Length - count]) + (hash >> 2));
                count--;
            }

            return (hash & 0x7FFFFFFF);
        }

        // P. J. Weinberger Hash Function
        public static int Hash6(string str)
        {
            const int bitsInUnignedInt = sizeof(int) * 8;
            const int threeQuarters = (bitsInUnignedInt * 3) / 4;
            const int oneEighth = bitsInUnignedInt / 8;
            int hash = 0;
            unchecked
            {
                const int highBits = (int)(0xFFFFFFFF) << (bitsInUnignedInt - oneEighth);
                char[] bitarray = str.ToCharArray();
                int count = bitarray.Length;
                while (count > 0)
                {
                    hash = (hash << oneEighth) + (bitarray[bitarray.Length - count]);
                    int test;
                    if ((test = hash & highBits) != 0)
                    {
                        hash = ((hash ^ (test >> threeQuarters)) & (~highBits));
                    }
                    count--;
                }
            }
            return (hash & 0x7FFFFFFF);
        }

        // ELF Hash Function
        public static int Hash7(string str)
        {
            int hash = 0;
            char[] bitarray = str.ToCharArray();
            int count = bitarray.Length;
            unchecked
            {
                while (count > 0)
                {
                    hash = (hash << 4) + (bitarray[bitarray.Length - count]);
                    int x;
                    if ((x = hash & (int)0xF0000000) != 0)
                    {
                        hash ^= (x >> 24);
                        hash &= ~x;
                    }
                    count--;
                }
            }
            return (hash & 0x7FFFFFFF);
        }


        // DJB Hash Function
        public static int Hash8(string str)
        {
            int hash = 5381;
            char[] bitarray = str.ToCharArray();
            int count = bitarray.Length;
            while (count > 0)
            {
                hash += (hash << 5) + (bitarray[bitarray.Length - count]);
                count--;
            }

            return (hash & 0x7FFFFFFF);
        }


    }
}
