using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulation;

namespace Intersection
{
    public class Triangle : IComparable<Triangle>
    {
        public Vector3 V1 { private set; get; }

        public Vector3 V2 { private set; get; }

        public Vector3 V3 { private set; get; }

        public Plane plane { private set; get; }

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            this.V1 = v1;
            this.V2 = v2;
            this.V3 = v3;
        }

        public Triangle(Triangle tri)
        {
            this.V1 = new Vector3(tri.V1);
            this.V2 = new Vector3(tri.V2);
            this.V3 = new Vector3(tri.V3);
        }

        public void ComputePlane()
        {
            Vector3 n = (V2 - V1).Cross(V3 - V1);
            plane = new Plane(n, -n.Dot(V1));
        }

        public static Tuple<bool, Segment> Intersection(Triangle tr1, Triangle tr2)
        {

            //1. Compute plane equation of triangle 2.
            tr1.ComputePlane();
            //2. Reject as trivial if all points of triangle 1 are on same side. (or all of them are on the other plane)
            //co-planar or no intersection
            if (!_can_be_isect(tr1, tr2)) return new Tuple<bool, Segment>(false, null);

            //3. Compute plane equation of triangle 1.
            tr2.ComputePlane();
            //4. Reject as trivial if all points of triangle 1 are on same side. (or all of them are on the other plane)
            //co-planar or no intersection
            if (!_can_be_isect(tr2, tr1)) return new Tuple<bool, Segment>(false, null);
            //5. Compute intersection line and project onto largest axis.
            //6. Compute the intervals for each triangle.
            Line isect_line;
            //parallel
            Tuple<bool, Line> tup = Plane.Intersection(tr1.plane, tr2.plane);
            if (!tup.Item1) return new Tuple<bool, Segment>(false, null);
            isect_line = tup.Item2;
            Segment s1, s2;
            bool b1, b2;
            double t1_1, t1_2, t2_1, t2_2;
            Tuple<bool, Segment, double, double> tup1 = _insect_helper(tr1, tr1.plane, new Line(isect_line));
            Tuple<bool, Segment, double, double> tup2 = _insect_helper(tr2, tr2.plane, new Line(isect_line));
            b1 = tup1.Item1; b2 = tup2.Item1;
            s1 = tup1.Item2; s2 = tup2.Item2;
            t1_1 = tup1.Item3; t1_2 = tup1.Item4;
            t2_1 = tup2.Item3; t2_2 = tup2.Item4;
            //one point intersection
            if (!b1 || !b2) return new Tuple<bool, Segment>(false, null);
            //7. Intersect the intervals.

            Helper.Sort(ref t1_1, ref t1_2);
            Helper.Sort(ref t2_1, ref t2_2);
            Segment s;
            if (t1_1 < t2_1)
            {
                if (t2_1 < t1_2)
                {
                    if (t2_2 < t1_2)
                        s = new Segment(s2.V1, s2.V2);
                    else
                        s = new Segment(s2.V1, s1.V2);
                    return new Tuple<bool, Segment>(true, s);
                }
            }
            else
                if (t1_1 < t2_2)
            {
                if (t1_2 < t2_2)
                    s = new Segment(s1.V1, s1.V2);
                else
                    s = new Segment(s1.V1, s2.V2);
                return new Tuple<bool, Segment>(true, s);
            }

            //no intersection
            return new Tuple<bool, Segment>(false, null);
        }

        private static Tuple<bool, Segment, double, double> _insect_helper(Triangle tr, Plane other_plane, Line isect_line)
        {
            double d1, d2, d3 = .0;
            d1 = Helper.EpsilonTest(other_plane.Distance(tr.V1));
            d2 = Helper.EpsilonTest(other_plane.Distance(tr.V2));
            d3 = Helper.EpsilonTest(other_plane.Distance(tr.V3));

            double t1, t2 = .0;
            Segment out_s;

            //Segment
            if (d1 * d2 > 0 && d3 != 0)
            {
                //v1v3 ^ L && v2v3 ^ L
                t1 = isect_line.Interval(new Segment(tr.V3, tr.V1), d3, d1);
                t2 = isect_line.Interval(new Segment(tr.V3, tr.V2), d3, d2);
                out_s = new Segment(isect_line.Eval(t1), isect_line.Eval(t2));
                return new Tuple<bool, Segment, double, double>(true, out_s, t1, t2);
            }
            if (d1 * d3 > 0 && d2 != 0)
            {
                //v1v2 ^ L && v3v2 ^ L
                t1 = isect_line.Interval(new Segment(tr.V2, tr.V1), d2, d1);
                t2 = isect_line.Interval(new Segment(tr.V2, tr.V3), d2, d3);
                out_s = new Segment(isect_line.Eval(t1), isect_line.Eval(t2));
                return new Tuple<bool, Segment, double, double>(true, out_s, t1, t2);
            }
            if (d2 * d3 > 0 && d1 != 0)
            {
                //v2v1 ^ L && v3v1 ^ L
                t1 = isect_line.Interval(new Segment(tr.V1, tr.V2), d1, d2);
                t2 = isect_line.Interval(new Segment(tr.V1, tr.V3), d1, d3);
                out_s = new Segment(isect_line.Eval(t1), isect_line.Eval(t2));
                return new Tuple<bool, Segment, double, double>(true, out_s, t1, t2);
            }

            //Segment with one vertex on the plane
            if (d1 * d2 < 0 && d3 == 0)
            {
                //v1v2 ^ L
                t1 = isect_line.Interval(new Segment(tr.V1, tr.V2), d1, d2);
                t2 = isect_line.Interval(new Segment(tr.V1, tr.V3), d1, d3);
                out_s = new Segment(isect_line.Eval(t1), tr.V3);
                return new Tuple<bool, Segment, double, double>(true, out_s, t1, t2);
            }
            if (d1 * d3 < 0 && d2 == 0)
            {
                //v1v3 ^ L
                t1 = isect_line.Interval(new Segment(tr.V1, tr.V3), d1, d3);
                t2 = isect_line.Interval(new Segment(tr.V1, tr.V2), d1, d2);
                out_s = new Segment(isect_line.Eval(t1), tr.V2);
                return new Tuple<bool, Segment, double, double>(true, out_s, t1, t2);
            }
            if (d2 * d3 < 0 && d1 == 0)
            {
                //v2v3 ^ L
                t1 = isect_line.Interval(new Segment(tr.V2, tr.V3), d2, d3);
                t2 = isect_line.Interval(new Segment(tr.V2, tr.V1), d2, d1);
                out_s = new Segment(isect_line.Eval(t1), tr.V1);
                return new Tuple<bool, Segment, double, double>(true, out_s, t1, t2);
            }

            //Segment with two vertices on the plane
            if (d1 == 0 && d2 == 0 && d3 != 0)
            {
                t1 = isect_line.Interval(new Segment(tr.V3, tr.V1), d3, d1);
                t2 = isect_line.Interval(new Segment(tr.V3, tr.V2), d3, d2);
                out_s = new Segment(tr.V1, tr.V2);
                return new Tuple<bool, Segment, double, double>(true, out_s, t1, t2);
            }
            if (d1 == 0 && d3 == 0 && d2 != 0)
            {
                t1 = isect_line.Interval(new Segment(tr.V2, tr.V1), d2, d1);
                t2 = isect_line.Interval(new Segment(tr.V2, tr.V3), d2, d3);
                out_s = new Segment(tr.V1, tr.V3);
                return new Tuple<bool, Segment, double, double>(true, out_s, t1, t2);
            }
            if (d2 == 0 && d3 == 0 && d1 != 0)
            {
                t1 = isect_line.Interval(new Segment(tr.V1, tr.V2), d1, d2);
                t2 = isect_line.Interval(new Segment(tr.V1, tr.V3), d1, d3);
                out_s = new Segment(tr.V2, tr.V3);
                return new Tuple<bool, Segment, double, double>(true, out_s, t1, t2);
            }

            //no segment (one point intersection)
            return new Tuple<bool, Segment, double, double>(false, null, -1 ,-1);
        }

        private static bool _can_be_isect(Triangle tr1, Triangle tr2)
        {
            double d1, d2, d3 = .0;
            d1 = Helper.EpsilonTest(tr1.plane.Distance(tr2.V1));
            d2 = Helper.EpsilonTest(tr1.plane.Distance(tr2.V2));
            d3 = Helper.EpsilonTest(tr1.plane.Distance(tr2.V3));

            //co-planar
            if (d1 == 0 && d2 == 0 && d3 == 0) return false;

            //not intersected
            if (d1 * d2 > 0 && d2 * d3 > 0) return false;

            return true;
        }

        public int CompareTo(Triangle other)
        {
            double min1 = Math.Min(Math.Min(V1.X, V2.X), V3.X);
            double min2 = Math.Min(Math.Min(other.V1.X, other.V2.X), other.V3.X);
            return min1 < min2 ? -1 : min1 == min2 ? 0 : 1;
        }

        public void Move(Vector3 trans)
        {
            this.V1 = this.V1 + trans;
            this.V2 = this.V2 + trans;
            this.V3 = this.V3 + trans;
        }

        public List<Vector3> Points()
        {
            return new Vector3[] { V1, V2, V3 }.ToList();
        }

        public override string ToString()
        {
            return $"V1 = {V1.ToString()}, V2 = {V2.ToString()}, V3 = {V3.ToString()}";
        }
    }
}
