/***************************************************************************** 
*        filename :KetamaNodeLocator 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   KetamaNodeLocator 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Algorithm 
*        文件名:             KetamaNodeLocator 
*        创建系统时间:       2015/12/11 17:09:36 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    /// <summary>
    /// 一致Hash算法
    /// </summary>
    public class KetamaNodeLocator
    {
        /// <summary>
        /// 原文中的JAVA类TreeMap实现了Comparator方法，这里我图省事，直接用了net下的SortedList，其中Comparer接口方法
        /// </summary>
        private readonly SortedList<long, string> _ketamaNodes;
        /// <summary>
        /// 此处参数与JAVA版中有区别，因为使用的静态方法，所以不再传递HashAlgorithm alg参数
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="numReps"></param>
        public KetamaNodeLocator(List<string> nodes, int numReps)
        {
            _ketamaNodes = new SortedList<long, string>();
            //对所有节点，生成nCopies个虚拟结点
            foreach (string node in nodes)
            {
                //每四个虚拟结点为一组
                for (int i = 0; i < numReps / 4; i++)
                {
                    //getKeyForNode方法为这组虚拟结点得到惟一名称 
                    byte[] digest = HashAlgorithm.ComputeMd5(node + i);
                    /** Md5是一个16字节长度的数组，将16字节的数组每四个字节一组，分别对应一个虚拟结点，这就是为什么上面把虚拟结点四个划分一组的原因*/
                    for (int h = 0; h < 4; h++)
                    {
                        long m = HashAlgorithm.Hash(digest, h);
                        _ketamaNodes[m] = node;
                    }
                }
            }
        }
        public KetamaNodeLocator(Dictionary<string, int> dictionary)
        {
            _ketamaNodes = new SortedList<long, string>();
            //对所有节点，生成nCopies个虚拟结点
            foreach (string node in dictionary.Keys)
            {
                int numReps = dictionary[node] / 4 <= 0 ? 1 : dictionary[node] / 4;
                //每四个虚拟结点为一组
                for (int i = 0; i < numReps; i++)
                {
                    //getKeyForNode方法为这组虚拟结点得到惟一名称 
                    byte[] digest = HashAlgorithm.ComputeMd5(node + i);
                    /** Md5是一个16字节长度的数组，将16字节的数组每四个字节一组，分别对应一个虚拟结点，这就是为什么上面把虚拟结点四个划分一组的原因*/
                    for (int h = 0; h < 4; h++)
                    {
                        long m = HashAlgorithm.Hash(digest, h);
                        _ketamaNodes[m] = node;
                    }
                }
            }
        }
        public string GetPrimary(string k)
        {
            byte[] digest = HashAlgorithm.ComputeMd5(k);
            string rv = GetNodeForKey(HashAlgorithm.Hash(digest, 0));
            return rv;
        }
        private string GetNodeForKey(long hash)
        {
            try
            {
                long key = hash;
                //如果找到这个节点，直接取节点，返回   
                if (!_ketamaNodes.ContainsKey(key))
                {
                    //得到大于当前key的那个子Map，然后从中取出第一个key，就是大于且离它最近的那个key 说明详见: http://www.javaeye.com/topic/684087
                    var tailMap = from coll in _ketamaNodes
                                  where coll.Key > hash
                                  select new { coll.Key };
                    if (!tailMap.Any())
                        key = _ketamaNodes.FirstOrDefault().Key;
                    else
                        key = tailMap.FirstOrDefault().Key;
                    // var tailMap = _ketamaNodes.FirstOrDefault(m => m.Key > hash);
                    //if (tailMap==)
                    //{
                    //    key = ketamaNodes.First();
                    //}
                    // key = tailMap.Key;
                }
                var rv = _ketamaNodes[key];
                return rv;
            }
            catch
            {
                return "";
            }
        }
    }

    public class HashAlgorithm
    {
        public static long Hash(byte[] digest, int nTime)
        {
            long rv = ((long)(digest[3 + nTime * 4] & 0xFF) << 24)
                      | ((long)(digest[2 + nTime * 4] & 0xFF) << 16)
                      | ((long)(digest[1 + nTime * 4] & 0xFF) << 8)
                      | ((long)digest[0 + nTime * 4] & 0xFF);
            return rv & 0xffffffffL; /* Truncate to 32-bits */
        }

        /// <summary>
        /// Get the md5 of the given key.
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static byte[] ComputeMd5(string k)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] keyBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(k));
            md5.Clear();
            //md5.update(keyBytes);
            //return md5.digest();
            return keyBytes;
        }
    }
}
