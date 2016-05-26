using System;
using System.Collections.Generic;

namespace Dobbler.Core
{
    public static class Utils
    {
        public static readonly Random rng = new Random();

        // http://stackoverflow.com/questions/5383498/shuffle-rearrange-randomly-a-liststring
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                var k = (rng.Next(0, n) % n);
                n--;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}