using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Common.Extensions
{
    public static class ReflectionExt
    {
        private const int CacheTime = 6*60;

        public static Object GetPropValue(this Object obj, String name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static T GetPropValue<T>(this Object obj, String name)
        {
            var retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

        public static IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit) where TAttribute : Attribute
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                   from t in a.GetTypes()
                   where t.IsDefined(typeof(TAttribute), inherit)
                   select t;
        }

        public static Type GetTypeByAttributeValue<TAttribute>(Type t, Func<TAttribute, object> pred, object oValue)
        {
            var cacheName = "TypeByAttribute_" + t.ToString() + pred.ToString() + oValue.ToString();

            return CacheManager.Get(cacheName, CacheTime, () =>
            {
                var type = typeof (TAttribute);
                var currentType =
                    AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.GlobalAssemblyCache && a.FullName.StartsWith("AdvantShop"))
                        .SelectMany(s => s.GetTypes()).Where(x =>
                            t.IsAssignableFrom(x) &&
                            x.GetCustomAttributes(type, true)
                                .Cast<TAttribute>()
                                .Any(oTemp => Equals(pred(oTemp), oValue))).ToList();

                if (currentType.Count > 1)
                    throw new Exception("duplicate for " + oValue);
                return currentType.FirstOrDefault();
            });
        }

        public static List<string> GetAttributeValue<TAttribute>(Type t)
        {
            var currentTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                                        .Where(x=> t.IsAssignableFrom(x) && x.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().Any()).ToList();
            var temp = currentTypes.Select(AttributeHelper.GetAttributeValue<TAttribute, string>).ToList();
            return temp;
        }

        public static T ToObject<T>(this IDictionary<string, string> source)
         where T : class, new()
        {
            T someObject = new T();
            Type someObjectType = someObject.GetType();

            foreach (KeyValuePair<string, string> item in source)
            {
                var property = someObjectType.GetProperty(item.Key);
                if (property == null) continue;
                property.SetValue(someObject, item.Value, BindingFlags.GetProperty, null, null, CultureInfo.InvariantCulture);
            }

            return someObject;
        }

        public static Dictionary<string, string> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, BindingFlags.GetProperty, null, null, CultureInfo.InvariantCulture).ToString()
            );
        }
    }
}