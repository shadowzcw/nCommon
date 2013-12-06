using System;
using System.Collections.Generic;
using System.Web;

namespace nCommon
{
    public static class CacheHelper
    {
        /// <summary>
        /// 获取Cache值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key"></param>
        /// <param name="f">得到值的方法(lambda)</param>
        /// <returns></returns>
        public static T Get<T>(string key, Func<T> f) where T : class
        {
            if (!Exists(key))
            {
                Insert(key, f());
            }

            return (T)HttpRuntime.Cache[key];
        }

        public static T Get<T>(string key, Func<T> f, int minitues)
        {
            if (!Exists(key))
            {
                Insert(key, f(), minitues);
            }

            return (T)HttpRuntime.Cache[key];
        }

        public static void Get<T>(string key, out T t)
        {
            t = (T)HttpRuntime.Cache.Get(key);
        }

        public static T Get<T>(string key)
        {
            return (T)HttpRuntime.Cache.Get(key);
        }

        public static void Set(string key, object value)
        {
            HttpRuntime.Cache[key] = value;
        }

        public static void Insert<T>(string key, T t) where T : class
        {
            if (t == null) return;
            HttpRuntime.Cache.Insert(key, t);
        }

        public static void Insert<T>(string key, T t, int minitues)
        {
            HttpRuntime.Cache.Insert(key, t, null, DateTime.Now.AddMinutes(minitues), TimeSpan.Zero);
        }

        public static bool Exists(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }

        public static void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        public static void Clear()
        {
            var caches = HttpRuntime.Cache.GetEnumerator();
            var keys = new List<String>();
            while (caches.MoveNext())
            {
                keys.Add(caches.Key.ToString());
            }
            foreach (var key in keys)
            {
                Remove(key);
            }
        }

        public static void Clear(string likeName)
        {
            var caches = HttpRuntime.Cache.GetEnumerator();
            var keys = new List<String>();
            while (caches.MoveNext())
            {
                keys.Add(caches.Key.ToString());
            }

            foreach (var key in keys)
            {
                if (key.Contains(likeName))
                    Remove(key);
            }
        }
    }
}
