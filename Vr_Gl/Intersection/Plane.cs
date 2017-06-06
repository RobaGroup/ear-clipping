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
        public Vector3 O { set; get; }

        public Vector3 N { private set; get; }

        public double d { private set; get; }

        public Plane(Vector3 n, double d)
        {
            N = n;
            this.d = d;
        }

        public Plane(Vector3 o, Vector3 n, double d): this(n, d)
        {
            O = o;
        }

        public double Distance(Vector3 v)
        {
            return N.Dot(v) + d;
        }

        public static Tuple<bool, Line> Intersection(Plane p1, Plane p2)
        {
            Vector3 norm1 = p1.N.Normalized(), norm2 = p2.N.Normalized();
            Vector3 D = norm1.Cross(norm2);
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
