using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intersection
{
    public class IntersectionDetector
    {
        public static void Detect(List<Triangle_Pair> vec)
        {
            for (int i = 0; i < vec.Count; i++)
            {
                Tuple<bool, Segment> t = Triangle.Intersection(vec[i].tr1, vec[i].tr2);
                vec[i].isect = t.Item1;
                vec[i].seg = t.Item2;
            }
        }

        public static List<Segment> Detect(Triangle tri, List<Triangle> tris)
        {
            List<Segment> res = new List<Segment>();
            foreach (var t in tris)
            {
                var temp = Triangle.Intersection(tri, t);
                if(temp.Item1)
                    res.Add(temp.Item2);
            }
            return res;
        }

    }
}
