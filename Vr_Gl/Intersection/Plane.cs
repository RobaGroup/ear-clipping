using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulation;


namespace Intersection
{
    public class Plane
    {
        public Vector3m O { set; get; }

        public Vector3m N { get; }

        public double d { get; }

        public Plane(Vector3m n, double d)
        {
            N = n;
            this.d = d;
        }

        public Plane(Vector3m o, Vector3m n, double d): this(n, d)
        {
            O = o;
        }

        public double Distance(Vector3m v)
        {
            return N.Dot(v) + d;
        }

        public static Tuple<bool, Line> Intersection(Plane p1, Plane p2)
        {
            Vector3m norm1 = p1.N.Normalized(), norm2 = p2.N.Normalized();
            Vector3m D = norm1.Cross(norm2);
            double det = D.Length();
            Line insect_l;
            if (det != 0)
            {
                insect_l = new Line((D.Cross(norm1) * p1.d + D.Cross(norm2) * p2.d) / det, D);
                return new Tuple<bool, Line>(true, insect_l);
            }

            return new Tuple<bool, Line>(false, null);
        }
    }
}
