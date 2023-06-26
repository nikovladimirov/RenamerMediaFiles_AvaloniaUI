using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RenamerMediaFiles.Helpers
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }
        /// <summary>
        /// WebGL так не работает :(
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="targetObject"></param>
        /// <typeparam name="T"></typeparam>
        public static void CopyByInterfaceTo<T>(this T sourceObject, T targetObject)
        {
            CopyByInterface(sourceObject, targetObject);
        }
        
        public static void CopyByInterface<T>(T sourceObject, T targetObject)
        {
            foreach (var property in typeof(T).GetPublicProperties().Where(p => p.CanWrite))
            {
                property.SetValue(targetObject, property.GetValue(sourceObject, null), null);
            }
        }
        
        private static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
        {
            if (!type.IsInterface)
                return type.GetProperties();

            return (new Type[] { type })
                .Concat(type.GetInterfaces())
                .SelectMany(i => i.GetProperties());
        }
        
        public static List<List<T>> Partition<T>(this List<T> values, int chunkSize)
        {
            var partitions = new List<List<T>>();
            for (int i = 0; i < values.Count; i += chunkSize) {
                partitions.Add(values.GetRange(i, Math.Min(chunkSize, values.Count - i)));
            }
            return partitions;
        }
    }
}