using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;

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

        private static Triangle3f _transform(Triangle tri)
        {
            return new Triangle3f(new Vector3f(tri.V1.X, tri.V1.Y, tri.V1.Z), new Vector3f(tri.V2.X, tri.V2.Y, tri.V2.Z), new Vector3f(tri.V3.X, tri.V3.Y, tri.V3.Z));
        }

        public static List<Segment> Intersect(Triangle tri, List<Triangle> tris)
        {
            List<Segment> segements = new List<Segment>();
            Triangle3f temp1 = _transform(tri);
            foreach (var item in tris)
            {
                Triangle3f temp2 = _transform(item);
                IntrTriangle3Triangle3 inter = new IntrTriangle3Triangle3(temp1, temp2);
                inter = inter.Compute();
                if(inter.Result == IntersectionResult.Intersects && inter.Type == IntersectionType.Segment)
                {
                    var t1 = inter.Points[0];
                    var t2 = inter.Points[1];
                    segements.Add(new Segment(new Triangulation.Vector3(t1.x, t1.y, t1.z), new Triangulation.Vector3(t2.x, t2.y, t2.z)));
                }
            }
            return segements;
        }

        public static List<Tuple<bool, Segment>> IntersectWithBool(Triangle tri, List<Triangle> tris)
        {
            List<Tuple<bool, Segment>> segements = new List<Tuple<bool, Segment>>();
            Triangle3f temp1 = _transform(tri);
            foreach (var item in tris)
            {
                Triangle3f temp2 = _transform(item);
                IntrTriangle3Triangle3 inter = new IntrTriangle3Triangle3(temp1, temp2);
                inter = inter.Compute();
                if (inter.Result == IntersectionResult.Intersects && inter.Type == IntersectionType.Segment)
                {
                    var t1 = inter.Points[0];
                    var t2 = inter.Points[1];
                    segements.Add(new Tuple<bool , Segment>(true, new Segment(new Triangulation.Vector3(t1.x, t1.y, t1.z), new Triangulation.Vector3(t2.x, t2.y, t2.z))));
                }
                else
                {
                    segements.Add(new Tuple<bool, Segment>(false, null));
                }
            }
            return segements;
        }

    }
}
