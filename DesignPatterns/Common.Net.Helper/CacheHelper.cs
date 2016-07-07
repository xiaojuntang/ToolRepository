using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;
using System.Web.Hosting;

namespace Common.Net.Helper
{
    /// <summary>
    /// IIS进程内的缓存
    /// </summary>
    public static class CacheHelper
    {
        private static readonly Cache _cache;

        public static double SaveTime
        {
            get;
            set;
        }

        static CacheHelper()
        {
            _cache = HostingEnvironment.Cache;
            SaveTime = 15.0;
        }

        /// <summary>
        /// 获取一个Key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return _cache.Get(key);
        }

        /// <summary>
        /// 获取一个Key的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            object obj = Get(key);
            return obj == null ? default(T) : (T)obj;
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsExist(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (_cache.Get(key) != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加一个缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Add(string key, object value)
        {
            Insert(key, value, null, CacheItemPriority.Default, null);
        }

        /// <summary>
        /// 添加一个缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="timeout">超时时间（millisecond）</param>
        public static void Add(string key, object value, double timeout)
        {
            _cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(timeout));
        }

        /// <summary>
        /// 移除指定的Key
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            _cache.Remove(key);
        }

        /// <summary>
        /// 获取所有的缓存Key
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetKeys()
        {
            List<string> keys = new List<string>();
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            return keys.AsReadOnly();
        }

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        public static void RemoveAll()
        {
            IList<string> keys = GetKeys();
            foreach (string key in keys)
            {
                _cache.Remove(key);
            }
        }

        private static void Insert(string key, object value, CacheDependency dependency, CacheItemPriority priority, CacheItemRemovedCallback callback)
        {
            _cache.Insert(key, value, dependency, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(SaveTime), priority, callback);
        }

        private static void Insert(string key, object value, CacheDependency dependency, CacheItemRemovedCallback callback)
        {
            Insert(key, value, dependency, CacheItemPriority.Default, callback);
        }

        private static void Insert(string key, object value, CacheDependency dependency)
        {
            Insert(key, value, dependency, CacheItemPriority.Default, null);
        }
    }
}

