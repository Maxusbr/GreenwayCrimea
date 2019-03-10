//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;

namespace AdvantShop.Core.Caching
{
    public class CacheManager
    {
        private const double DefaultTime = 5;
        private static readonly Object SyncObject = new Object();

        public static Cache CacheObject = HttpRuntime.Cache;

        public static void Insert<T>(String key, T value)
        {
            Insert(key, value, DefaultTime);
        }

        public static void Insert<T>(String key, T value, double minutes)
        {
            lock (SyncObject)
            {
                DateTime expiration = DateTime.Now.AddMinutes(minutes);
                CacheObject.Insert(key, value, null, expiration, TimeSpan.Zero, CacheItemPriority.Default, null);
            }
        }

        public static bool Contains(String key)
        {
            return CacheObject.Get(key) != null;
        }

        public static int Count()
        {
            return CacheObject.Count;
        }

        public static T Get<T>(string key)
        {
            var value = CacheObject.Get(key);
            return (value is T) ? ((T)value) : default(T);
        }

        public static T Get<T>(string key, Func<T> acquire)
        {
            return Get<T>(key, DefaultTime, acquire);
        }

        public static T Get<T>(string key, double cacheTime, Func<T> acquire)
        {
            T result;
            if (!TryGetValue(key, out result))
            {
                result = acquire();
                if (result != null)
                {
                    Insert(key, result, cacheTime);
                }
            }
            return result;
        }

        public static bool TryGetValue<T>(string key, out T value)
        {
            var v = CacheObject.Get(key);
            if (v != null)
            {
                value = (T)v;
                return true;
            }
            value = default(T);
            return false;
        }

        public static void Clean()
        {
            IDictionaryEnumerator enumerator = CacheObject.GetEnumerator();
            while (enumerator.MoveNext())
            {
                CacheObject.Remove(enumerator.Key.ToString());
            }
        }

        public static void Remove(string key)
        {
            CacheObject.Remove(key);
        }

        public static void RemoveByPattern(string key)
        {
            IDictionaryEnumerator item = CacheObject.GetEnumerator();
            while (item.MoveNext())
            {
                if (item.Key.ToString().Contains(key))
                    CacheObject.Remove(item.Key.ToString());
            }
        }

        public static List<T> GetByPattern<T>(string key)
        {
            var list = new List<T>();
            IDictionaryEnumerator item = CacheObject.GetEnumerator();
            while (item.MoveNext())
            {
                if (item.Key.ToString().Contains(key))
                {
                    var value = CacheObject.Get(key);
                    list.Add((value is T) ? ((T)value) : default(T));
                }
            }
            return list;
        }
    }
}