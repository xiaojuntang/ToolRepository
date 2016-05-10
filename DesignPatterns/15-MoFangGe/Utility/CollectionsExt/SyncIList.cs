
/**************************************************
* 文 件 名：SyncIList.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/9 9:31:15
* 文件说明：实现一个能够在高并发下 异步访问的链表
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1570

namespace Climb.Utility.CollectionsExt
{
    /// <summary>
    /// 实现一个能够在高并发下 异步访问的链表对象 
    /// 链表本身基于List 实现
    /// </summary>
    /// <typeparam name="T">链表的类型 type </typeparam>
    [Serializable]
    public  class SyncList<T> : List<T>
    {
        private readonly List<T> _synList;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="list">list集合</param>
        public SyncList(List<T> list)
        {
            _synList = list;
        }
       
        /// <summary>
        /// 启用线程
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="item">type 类型的对象</param>
        public new void Add(T item)
        {
            lock (_synList)
            {
                _synList.Add(item);
            }
        }

        /// <summary>
        /// 批量添加集合
        /// </summary>
        /// <param name="collection">T类型的集合</param>
        public new void AddRange(IEnumerable<T> collection)
        {
            lock (_synList)
            {
                _synList.InsertRange(Count, collection);
            }
        }

        /// <summary>
        /// 集合设置成只读
        /// </summary>
        /// <returns></returns>
        public new ReadOnlyCollection<T> AsReadOnly()
        {
            lock (_synList)
            {
                return new ReadOnlyCollection<T>(_synList);
            }
        }

        /// <summary>
        /// 清空链表中的对象
        /// </summary>
        public new void Clear()
        {
            lock (_synList)
            {
                _synList.Clear();
            }
        }
        /// <summary>
        /// 判断是否包含对象
        /// </summary>
        /// <param name="item">t类型对象</param>
        /// <returns>如果包含t对象则返回true 否则 返回false</returns>
        public new bool Contains(T item)
        {
            bool flag;

            lock (_synList)
            {
                flag = _synList.Contains(item);
            }

            return flag;
        }
        /// <summary>
        /// 将对象转换成 可适配的类型
        /// </summary>
        /// <typeparam name="TOutput">需要输出的类型</typeparam>
        /// <param name="converter">转换委托</param>
        /// <returns>返回转换的类型</returns>
        public new List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            List<TOutput> list;
            lock (_synList)
            {
                list = new List<TOutput>(_synList.ConvertAll(converter));
            }

            return list;

        }
       /// <summary>
       /// 复制数组的中的对象到链表中
       /// </summary>
       /// <param name="array">需要拷贝的数组</param>
        public new void CopyTo(T[] array)
        {
            lock (_synList)
            {
                _synList.CopyTo(array, 0);
            }
        }
       /// <summary>
       /// 从某个起始位置开始拷贝数组
       /// </summary>
        /// <param name="array">需要拷贝的数组</param>
       /// <param name="arrayIndex">从指定的位置开始拷贝</param>
        public new void CopyTo(T[] array, int arrayIndex)
        {
            lock (_synList)
            {
                _synList.CopyTo(array, arrayIndex);
            }
        }
      
        /// <summary>
        /// 从某个位置开始拷贝 以及设置拷贝对象的数量
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="array">数组</param>
        /// <param name="arrayIndex">起始位置</param>
        /// <param name="count">需要拷贝的数量</param>
        public new void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            lock (_synList)
            {
                _synList.CopyTo(index, array, arrayIndex, count);
            }
        }

        /// <summary>
        /// 判断某个对象是否存在
        /// </summary>
        /// <param name="match">存在的委托</param>
        /// <returns>存在返回ture 或者false</returns>
        public new bool Exists(Predicate<T> match)
        {
            return (FindIndex(match) != -1);
        }

        /// <summary>
        /// 返回匹配的第一个对象
        /// </summary>
        /// <param name="match">匹配类型的委托</param>
        /// <returns>返回匹配的对象</returns>
        public new T Find(Predicate<T> match)
        {
            T temp;

            lock (_synList)
            {
                temp = _synList.Find(match);
            }

            return temp;
        }
        /// <summary>
        /// 返回匹配的所有元素
        /// </summary>
        /// <param name="match">匹配类型的委托</param>
        /// <returns>返回匹配的对象</returns>
        public new List<T> FindAll(Predicate<T> match)
        {
            List<T> temp;

            lock (_synList)
            {
                temp = new List<T>(_synList.FindAll(match));
            }

            return temp;
        }
        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素，并返回整个 System.Collections.Generic.List<T> 中第一个匹配元素的从零开始的索引
        /// </summary>
        /// <param name="match">System.Predicate<T> 委托，用于定义要搜索的元素的条件</param>
        /// <returns>如果找到与 match 定义的条件相匹配的第一个元素，则为该元素的从零开始的索引；否则为 -1</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public new int FindIndex(Predicate<T> match)
        {
            int temp;

            lock (_synList)
            {
                temp = _synList.FindIndex(match);
            }

            return temp;
        }
      

        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素，并返回 System.Collections.Generic.List<T> 中从指定索引到最后一个元素的元素范围内第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="match">System.Predicate<T> 委托，用于定义要搜索的元素的条件</param>
        /// <returns>返回匹配的位置</returns>
        public new int FindIndex(int startIndex, Predicate<T> match)
        {
            int temp;

            lock (_synList)
            {
                temp = _synList.FindIndex(startIndex, match);
            }

            return temp;

        }
        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素。返回前n条数据。
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">需要获取的数量</param>
        /// <param name="match">System.Predicate<T> 委托，用于定义要搜索的元素的条件</param>
        /// <returns>返回匹配的位置</returns>
        public new int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            int temp;

            lock (_synList)
            {
                temp = _synList.FindIndex(startIndex, count, match);
            }

            return temp;

        }
      
        /// <summary>
        /// 返回匹配的最后一个元素
        /// </summary>
        /// <param name="match">System.Predicate<T> 委托，用于定义要搜索的元素的条件</param>
        /// <returns>返回最后的对象</returns>
        public new T FindLast(Predicate<T> match)
        {
            T temp;

            lock (_synList)
            {
                temp = _synList.FindLast(match);
            }

            return temp;

        }

        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素，并返回 System.Collections.Generic.List<T> 从最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="match">System.Predicate<T> 委托，用于定义要搜索的元素的条件</param>
        /// <returns>最后一个匹配元素从0开始</returns>
        public new int FindLastIndex(Predicate<T> match)
        {
            int temp;

            lock (_synList)
            {
                temp = _synList.FindLastIndex(match);
            }

            return temp;
        }
        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素，并返回 System.Collections.Generic.List<T> 从第一个元素指定索引元素的范伟从最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="startIndex">其实位置</param>
        /// <param name="match">System.Predicate<T> 委托，用于定义要搜索的元素的条件</param>
        /// <returns>最后一个匹配元素从开始<returns>
        public new int FindLastIndex(int startIndex, Predicate<T> match)
        {
            int temp;

            lock (_synList)
            {
                temp = _synList.FindLastIndex(startIndex, match);
            }

            return temp;
        }

        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素，并返回 System.Collections.Generic.List<T> 从第一个元素指定索引元素的范伟从最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="startIndex">起始位置的开始</param>
        /// <param name="count">匹配的数量</param>
        /// <param name="match">System.Predicate<T> 委托，用于定义要搜索的元素的条件</param>
        /// <returns>最后一个匹配元素从开始</returns>
        public new int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            int temp;

            lock (_synList)
            {
                temp = _synList.FindLastIndex(startIndex, count, match);
            }

            return temp;

        }

        /// <summary>
        /// 遍历链表
        /// </summary>
        /// <param name="action">委托遍历</param>
        public new void ForEach(Action<T> action)
        {

            lock (_synList)
            {
                _synList.ForEach(action);
            }
        }
        /// <summary>
        /// 返回循环访问对象的枚举
        /// </summary>
        /// <returns></returns>
        public new Enumerator GetEnumerator()
        {
            Enumerator temp;
            lock (_synList)
            {
                temp = _synList.GetEnumerator();
            }

            return temp;
        }
        /// <summary>
        /// 获得一个副本
        /// </summary>
        /// <param name="index">其其实位置</param>
        /// <param name="count">需要副本数量</param>
        /// <returns>返回这个副本</returns>
        public new List<T> GetRange(int index, int count)
        {
            List<T> list;
            lock (_synList)
            {
                list = new List<T>(_synList.GetRange(index, count));
            }

            return list;
        }

        /// <summary>
        /// 搜索指定对象，并返回整个 匹配从0开始索引
        /// </summary>
        /// <param name="item">item 对象</param>
        /// <returns>返回索引位置</returns>
        public new int IndexOf(T item)
        {
            int temp;

            lock (_synList)
            {
                temp = _synList.IndexOf(item);
            }

            return temp;
        }

        /// <summary>
        /// 搜索指定对象，并返回整个 匹配从0开始索引
        /// </summary>
        /// <param name="item">item 对象</param>
        /// <param name="index">从某项开始</param>
        /// <returns>返回索引位置</returns>
        public new int IndexOf(T item, int index)
        {
            int temp;

            lock (_synList)
            {
                temp = _synList.IndexOf(item, index);
            }

            return temp;
        }
        /// <summary>
        /// 搜索指定对象，返回匹配的位置
        /// </summary>
        /// <param name="item">对象</param>
        /// <param name="index">开始位置</param>
        /// <param name="count">匹配的数量</param>
        /// <returns>返回匹配的值</returns>
        public new int IndexOf(T item, int index, int count)
        {
            int temp;

            lock (_synList)
            {
                temp = _synList.IndexOf(item, index, count);
            }

            return temp;
        }
   
        /// <summary>
        ///  插入一个元素 从指定索引处
        /// </summary>
        /// <param name="index">位置</param>
        /// <param name="item">元素</param>
        public new void Insert(int index, T item)
        {
            lock (_synList)
            {
                _synList.Insert(index, item);
            }
        }
        /// <summary>
        /// 批量插入一批数据 从某个索引处
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="collection">集合</param>
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            lock (_synList)
            {
                _synList.InsertRange(index, collection);
            }
        } // InsertRange
      
        /// <summary>
        /// 搜索元素在某个位置 从最后一个0位置索引
        /// </summary>
        /// <param name="item">搜索对象</param>
        /// <returns>索引位置</returns>
        public new int LastIndexOf(T item)
        {
            int flag;
            lock (_synList)
            {
                flag = _synList.LastIndexOf(item);
            }

            return flag;
        }
       /// <summary>
        /// 搜索元素在某个位置 从指定元素 开始索引
       /// </summary>
        /// <param name="item">搜索对象</param>
       /// <param name="index">开始位置</param>
        /// <returns>索引位置</returns>
        public new int LastIndexOf(T item, int index)
        {
            int flag;
            lock (_synList)
            {
                flag = _synList.LastIndexOf(item, index);
            }

            return flag;
        }

        /// <summary>
        /// 搜索元素在某个位置 从指定元素 开始索引
        /// </summary>
        /// <param name="item">对象item</param>
        /// <param name="index">开始位置</param>
        /// <param name="count">数量</param>
        /// <returns>索引位置</returns>
        public new int LastIndexOf(T item, int index, int count)
        {
            int flag;
            lock (_synList)
            {
                flag = _synList.LastIndexOf(item, index, count);
            }

            return flag;
        }
        /// <summary>
        /// 删除某个对象
        /// </summary>
        /// <param name="item">要删除对象</param>
        /// <returns>返回true 或者false  成功为ture</returns>
        public new bool Remove(T item)
        {
            bool flag;
            lock (_synList)
            {
                flag = _synList.Remove(item);
            }

            return flag;
        }
        /// <summary>
        /// 移除所有元素
        /// </summary>
        /// <param name="match">匹配的委托</param>
        /// <returns>返回删除的数量</returns>
        public new int RemoveAll(Predicate<T> match)
        {
            int flag;
            lock (_synList)
            {
                flag = _synList.RemoveAll(match);
            }

            return flag;

        }
        /// <summary>
        /// 移除某个元素
        /// </summary>
        /// <param name="index">元素的位置</param>
        public new void RemoveAt(int index)
        {
            lock (_synList)
            {
                _synList.RemoveAt(index);
            }
        }
        /// <summary>
        /// 删除一定范伟的元素
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="count">删除的数量</param>
        public new void RemoveRange(int index, int count)
        {
            lock (_synList)
            {
                _synList.RemoveRange(index, count);
            }

        }
        /// <summary>
        /// 反转链表
        /// </summary>
        public new void Reverse()
        {
            lock (_synList)
            {
                _synList.Reverse();
            }
        }
      /// <summary>
      /// 反转顺序
      /// </summary>
      /// <param name="index">起始位置</param>
      /// <param name="count">数量</param>
        public new void Reverse(int index, int count)
        {
            lock (_synList)
            {
                _synList.Reverse(index, count);
            }
        }

        /// <summary>
        /// 链表排序
        /// </summary>
        public new void Sort()
        {
            lock (_synList)
            {
                _synList.Sort();
            }
        }
        /// <summary>
        /// 对链表进行排序
        /// </summary>
        /// <param name="comparer">接口对象</param>
        public new void Sort(IComparer<T> comparer)
        {
            lock (_synList)
            {
                _synList.Sort(comparer);
            }
        }
        /// <summary>
        /// 对链表进行排序
        /// </summary>
        /// <param name="comparison">排序的委托</param>
        public new void Sort(Comparison<T> comparison)
        {
            lock (_synList)
            {
                _synList.Sort(comparison);
            }
        }
        /// <summary>
        /// 从某个元素开始排序
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <param name="comparer">排序的委托</param>
        public new void Sort(int index, int count, IComparer<T> comparer)
        {
            lock (_synList)
            {
                _synList.Sort(index, count, comparer);
            }
        }
        
        /// <summary>
        /// 将链表转换成数组
        /// </summary>
        /// <returns>返回数组</returns>
        public new T[] ToArray()
        {
            T[] localArray1;
            lock (_synList)
            {
                localArray1 = _synList.ToArray();
            }

            return localArray1;
        }
       
        /// <summary>
        /// 将容量设置实际的数目
        /// </summary>
        public new void TrimExcess()
        {
            lock (_synList)
            {
                _synList.TrimExcess();
            }
        }
        /// <summary>
        /// 判断元素是否全部匹配
        /// </summary>
        /// <param name="match">匹配的委托</param>
        /// <returns>全部匹配返回true </returns>
        public new bool TrueForAll(Predicate<T> match)
        {
            bool flag;
            lock (_synList)
            {
                flag = _synList.TrueForAll(match);
            }
            return flag;
        }

        /// <summary>
        /// 设置数链表容量
        /// </summary>
        public new int Capacity
        {
            get
            {
                int temp;
                lock (_synList)
                {
                    temp = _synList.Capacity;
                }
                return temp;
            }
            set
            {
                lock (_synList)
                {
                    _synList.Capacity = value;
                }

            }
        }
      
        /// <summary>
        /// 返回数量
        /// </summary>
        public new int Count
        {
            get
            {
                lock (_synList)
                {
                    return _synList.Count;
                }
            }
        }
        /// <summary>
        /// 索引查找对象
        /// </summary>
        /// <param name="index">索引id 位置</param>
        /// <returns>返回对象</returns>
        public new T this[int index]
        {
            get
            {
                lock (_synList)
                {
                    return _synList[index];
                }
            }
            set
            {
                lock (_synList)
                {
                    _synList[index] = value;
                }
            }
        }

    } 
}
