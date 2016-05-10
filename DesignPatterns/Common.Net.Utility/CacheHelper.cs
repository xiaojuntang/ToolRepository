using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace Common.Net.Utility
{
    /// <summary>
    /// 缓存帮助
    /// </summary>
    public class CacheHelper
    {
        //public static object Get(string key)
        //{
        //    var cacheConfig = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
        //    return cacheConfig.CacheProvider.Get(key);
        //}

        //public static void Set(string key, object value)
        //{
        //    var cacheConfig = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);

        //    cacheConfig.CacheProvider.Set(key, value, cacheConfig.CacheConfigItem.Minitus, cacheConfig.CacheConfigItem.IsAbsoluteExpiration, null);
        //}

        //public static void Remove(string key)
        //{
        //    var cacheConfig = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
        //    cacheConfig.CacheProvider.Remove(key);
        //}

        //public static void Clear(string keyRegex = ".*", string moduleRegex = ".*")
        //{
        //    if (!Regex.IsMatch(CacheConfigContext.ModuleName, moduleRegex, RegexOptions.IgnoreCase))
        //        return;

        //    foreach (var cacheProviders in CacheConfigContext.CacheProviders.Values)
        //        cacheProviders.Clear(keyRegex);
        //}

        //如果缓存里没有，则取数据然后缓存起来
        //public static F Get<F>(string key, Func<F> getRealData)
        //{
        //    var getDataFromCache = new Func<F>(() =>
        //    {
        //        F data = default(F);
        //        var cacheData = Get(key);
        //        if (cacheData == null)
        //        {
        //            data = getRealData();
        //            if (data != null)
        //                Set(key, data);
        //        }
        //        else
        //        {
        //            data = (F)cacheData;
        //        }
        //        return data;
        //    });

        //    return GetItem<F>(key, getDataFromCache);
        //}

        //public static F Get<F>(string key, int id, Func<int, F> getRealData)
        //{
        //    return Get<F, int>(key, id, getRealData);
        //}

        //public static F Get<F>(string key, string id, Func<string, F> getRealData)
        //{
        //    return Get<F, string>(key, id, getRealData);
        //}

        //public static F Get<F>(string key, string branchKey, Func<F> getRealData)
        //{
        //    return Get<F, string>(key, branchKey, id => getRealData());
        //}

        //public static F Get<F, T>(string key, T id, Func<T, F> getRealData)
        //{
        //    key = string.Format("{0}_{1}", key, id);

        //    var getDataFromCache = new Func<F>(() =>
        //    {
        //        F data = default(F);
        //        var cacheData = Get(key);
        //        if (cacheData == null)
        //        {
        //            data = getRealData(id);

        //            if (data != null)
        //                Set(key, data);
        //        }
        //        else
        //        {
        //            data = (F)cacheData;
        //        }

        //        return data;
        //    });

        //    return GetItem<F>(key, getDataFromCache);
        //}

        #region 以下几个方法从HttpContext.Items缓存页面数据，适合页面生命周期，页面载入后就被移除，而非HttpContext.Cache在整个应用程序都有效
        //如果上下文HttpContext.Current.Items里没有，则取数据然后加入Items，在页面生命周期内有效
        public static F GetItem<F>(string name, Func<F> getRealData)
        {
            if (HttpContext.Current == null)
                return getRealData();

            var httpContextItems = HttpContext.Current.Items;
            if (httpContextItems.Contains(name))
            {
                return (F)httpContextItems[name];
            }
            else
            {
                var data = getRealData();
                if (data != null)
                    httpContextItems[name] = data;
                return data;
            }
        }

        public static F GetItem<F>() where F : new()
        {
            return GetItem<F>(typeof(F).ToString(), () => new F());
        }

        public static F GetItem<F>(Func<F> getRealData)
        {
            return GetItem<F>(typeof(F).ToString(), getRealData);
        }
        #endregion

    }

    public class CacheConfigContext
    {
        private static readonly object olock = new object();

        internal static CacheConfig CacheConfig
        {
            get
            {
                return null;//CachedConfigContext.Current.CacheConfig;
            }
        }

        /// <summary>
        /// 首次加载所有的CacheConfig, wrapCacheConfigItem相对于cacheConfigItem把providername通过反射还原成了具体provider类
        /// </summary>
        private static List<WrapCacheConfigItem> wrapCacheConfigItems;
        internal static List<WrapCacheConfigItem> WrapCacheConfigItems
        {
            get
            {
                if (wrapCacheConfigItems == null)
                {
                    lock (olock)
                    {
                        if (wrapCacheConfigItems == null)
                        {
                            wrapCacheConfigItems = new List<WrapCacheConfigItem>();

                            foreach (var i in CacheConfig.CacheConfigItems)
                            {
                                var cacheWrapConfigItem = new WrapCacheConfigItem();
                                cacheWrapConfigItem.CacheConfigItem = i;
                                cacheWrapConfigItem.CacheProviderItem = CacheConfig.CacheProviderItems.SingleOrDefault(c => c.Name == i.ProviderName);
                                cacheWrapConfigItem.CacheProvider = CacheProviders[i.ProviderName];

                                wrapCacheConfigItems.Add(cacheWrapConfigItem);
                            }
                        }
                    }
                }

                return wrapCacheConfigItems;
            }
        }

        /// <summary>
        /// 首次加载所有的CacheProviders
        /// </summary>
        private static Dictionary<string, ICacheProvider> cacheProviders;
        internal static Dictionary<string, ICacheProvider> CacheProviders
        {
            get
            {
                if (cacheProviders == null)
                {
                    lock (olock)
                    {
                        if (cacheProviders == null)
                        {
                            cacheProviders = new Dictionary<string, ICacheProvider>();

                            foreach (var i in CacheConfig.CacheProviderItems)
                                cacheProviders.Add(i.Name, (ICacheProvider)Activator.CreateInstance(Type.GetType(i.Type)));
                        }
                    }
                }

                return cacheProviders;
            }
        }

        /// <summary>
        /// 根据Key，通过正则匹配从WrapCacheConfigItems里帅选出符合的缓存项目，然后通过字典缓存起来
        /// </summary>
        private static Dictionary<string, WrapCacheConfigItem> wrapCacheConfigItemDic;
        internal static WrapCacheConfigItem GetCurrentWrapCacheConfigItem(string key)
        {
            if (wrapCacheConfigItemDic == null)
                wrapCacheConfigItemDic = new Dictionary<string, WrapCacheConfigItem>();

            if (wrapCacheConfigItemDic.ContainsKey(key))
                return wrapCacheConfigItemDic[key];

            var currentWrapCacheConfigItem = WrapCacheConfigItems.Where(i =>
                Regex.IsMatch(ModuleName, i.CacheConfigItem.ModuleRegex, RegexOptions.IgnoreCase) &&
                Regex.IsMatch(key, i.CacheConfigItem.KeyRegex, RegexOptions.IgnoreCase))
                .OrderByDescending(i => i.CacheConfigItem.Priority).FirstOrDefault();

            if (currentWrapCacheConfigItem == null)
                throw new Exception(string.Format("Get Cache '{0}' Config Exception", key));

            lock (olock)
            {
                if (!wrapCacheConfigItemDic.ContainsKey(key))
                    wrapCacheConfigItemDic.Add(key, currentWrapCacheConfigItem);
            }

            return currentWrapCacheConfigItem;
        }

        /// <summary>
        /// 得到网站项目的入口程序模块名名字，用于CacheConfigItem.ModuleRegex
        /// </summary>
        /// <returns></returns>
        private static string moduleName;
        public static string ModuleName
        {
            get
            {
                if (moduleName == null)
                {
                    lock (olock)
                    {
                        if (moduleName == null)
                        {
                            var entryAssembly = Assembly.GetEntryAssembly();

                            if (entryAssembly != null)
                            {
                                moduleName = entryAssembly.FullName;
                            }
                            else
                            {
                                moduleName = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Name;
                            }
                        }
                    }
                }

                return moduleName;
            }
        }

    }

    public class WrapCacheConfigItem
    {
        public CacheConfigItem CacheConfigItem { get; set; }
        public CacheProviderItem CacheProviderItem { get; set; }
        public ICacheProvider CacheProvider { get; set; }
    }

    [Serializable]
    public class CacheConfig : ConfigFileBase
    {
        public CacheConfig()
        {
        }

        public CacheConfigItem[] CacheConfigItems { get; set; }
        public CacheProviderItem[] CacheProviderItems { get; set; }
    }

    public class CacheProviderItem : ConfigNodeBase
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
    }

    public class CacheConfigItem : ConfigNodeBase
    {
        [XmlAttribute(AttributeName = "keyRegex")]
        public string KeyRegex { get; set; }

        [XmlAttribute(AttributeName = "moduleRegex")]
        public string ModuleRegex { get; set; }

        [XmlAttribute(AttributeName = "providerName")]
        public string ProviderName { get; set; }

        [XmlAttribute(AttributeName = "minitus")]
        public int Minitus { get; set; }

        [XmlAttribute(AttributeName = "priority")]
        public int Priority { get; set; }

        [XmlAttribute(AttributeName = "isAbsoluteExpiration")]
        public bool IsAbsoluteExpiration { get; set; }

        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
    }

    public class ConfigNodeBase
    {

    }

    public class ConfigFileBase
    {

    }

    public interface ICacheProvider
    {

    }
}
