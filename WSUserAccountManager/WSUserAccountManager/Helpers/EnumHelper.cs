using System;

namespace WSUserAccountManager.Helpers
{
    public static class EnumHelper
    {
        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
