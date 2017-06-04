using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intersection
{
    public class Helper
    {
        private const double EPSILON = 0.000001;

        static public double Max(double a, double b, double c)
        {
            if (a > b)
                if (b > c)
                    return a;
            if (c > a)
                return c;
            return a;
            if (b > c)
                return b;
            return c;
        }

        static public void Sort(ref double a, ref double b)
        {
            if (a > b)
            {
                a = a + b;
                b = a - b;
                a = a - b;
            }
        }

        static public double EpsilonTest(double f)
        {
            if (Math.Abs(f) <= EPSILON) return .0;
            return f;
        }
    }
}
