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

        public static List<Tuple<bool, Segment>> Detect(Triangle tri, List<Triangle> tris)
        {
            List<Tuple<bool, Segment>> res = new List<Tuple<bool, Segment>>();
            foreach (var t in tris)
            {
                var temp = Triangle.Intersection(tri, t);
                res.Add(temp);
            }
            return res;
        }

    }
}
