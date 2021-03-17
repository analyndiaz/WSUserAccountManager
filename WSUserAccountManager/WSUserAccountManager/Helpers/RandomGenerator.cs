using System;
using System.Linq;

namespace WSUserAccountManager.Helpers
{
    public static class RandomGenerator
    {
        private static Random _random = new Random();

        public static string String(int length)
        {
            const string chars = "abcdefghijklmnopgrstuvwxyz1234567890";
            return new string(Enumerable.Repeat(chars, length)
                       .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public static string Code(int length)
        {
            const string chars = "1234567890";
            return new string(Enumerable.Repeat(chars, length)
                       .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public static string Id(int length)
        {
            const string chars = "abcdefghijklmnopgrstuvwxyz1234567890";
            return new string(Enumerable.Repeat(chars, length)
                       .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
