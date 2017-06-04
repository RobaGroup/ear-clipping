using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intersection
{
    public class Triangle_Pair
    {
        public Triangle tr1;
        public Triangle tr2;
        public bool isect;
        public Segment seg;

        Triangle_Pair(Triangle t1, Triangle t2, bool b, Segment s)
        {
            tr1 = t1;
            tr2 = t2;
            isect = b;
            seg = s;
        }
    };
}
