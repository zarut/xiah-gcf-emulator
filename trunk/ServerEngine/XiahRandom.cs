using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerEngine
{
    //public class XiahRandom
    //{
    //    private static Random rnd;

    //    static XiahRandom()
    //    {
    //        rnd = new Random();
    //    }

    //    public static bool PercentSuccess(double percent)
    //    {
    //        return ((double)rnd.Next(1, 1000000)) / 10000 >= 100 - percent;
    //    }

    //    public static int Next(int one, int two)
    //    {
    //        return rnd.Next(one, two);
    //    }
    //}
    public static class XiahRandom
    {
        static readonly Random rnd = new Random();

        public static bool PercentSuccess(double percent)
        {
            lock (rnd) return ((double)rnd.Next(1, 1000000)) / 10000 >= 100 - percent;
        }
        public static int Next(int one, int two)
        {
            lock (rnd) return rnd.Next(one, two);
        }
    }
}
