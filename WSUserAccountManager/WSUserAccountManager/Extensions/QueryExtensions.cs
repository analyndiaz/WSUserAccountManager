using System;

namespace WSUserAccountManager.Extensions
{
    public static class QueryExtensions
    {
        public static string GetProperty<T>(this T obj, string name) where T : class
        {
            return obj.GetType().GetProperty(name).GetValue(obj).ToString();
        }
    }
}
