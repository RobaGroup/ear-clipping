using Intersection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulation;
using OpenTK.Graphics.OpenGL;
using Vr_Gl.Model;
using Vr_Gl.Graphics;
using GeoAPI.Geometries;
using NetTopologySuite.Triangulate.QuadEdge;

namespace Vr_Gl.Simulation
{
    public class Cutted
    {
        List<Triangle> Triangles { get; set; }
        public Cutter Cutter { get; set; }

        public int Texture;

        public Cutted(string fileName, Cutter cutter)
        {
            this.Triangles = new Loader(3).Load(fileName);
            this.Cutter = cutter;
        }

        public Cutted(List<Triangle> tris, Cutter cutter)
        {
            this.Triangles = tris;
            this.Cutter = cutter;
        }

        public Cutted(List<Triangle> tris, Cutter cutter, Vector3 initialPos, string texturePath = "D:/cutted.jpg") : this(tris, cutter)
        {
            this.Move(initialPos);
            this.Texture = AssetsLoader.LoadTexture(texturePath);
        }

        public Cutted(string fileName, Cutter cutter, Vector3 initialPos, string texturePath = "D:/cutted.jpg") : this(fileName, cutter)
        {
            this.Move(initialPos);
            this.Texture = AssetsLoader.LoadTexture(texturePath);
        }

        public void Move(Vector3 trans)
        {
            for (int i = 0; i < Triangles.Count; i++)
            {
                Triangles[i].Move(trans);
            }
        }

        public void Update()
        {
            List<Triangle> tris = new List<Triangle>();
            var intersectedTris = Cutter.Intersect(this.Triangles);
            for (int i = 0; i < Triangles.Count; i++)
            {
                var tri = Triangles[i];
                if (intersectedTris[i].Count == 0)
                {
                    tris.Add(tri);
                    continue;
                }
                var result = IntersectionDetector.IntersectWithTriangles(tri, intersectedTris[i]);
                if (result.Count >= 2)
                {
                    List<List<Vector3>> holes = new List<List<Vector3>>();
                    List<Vector3> temp = new List<Vector3>();
                    List<List<int>> insides = new List<List<int>>();
                    bool anyOutside = false;
                    for (int j = 0; j < result.Count; j++)
                    {
                        if (tri.Inside(result[j].Item1.V1) && tri.Inside(result[j].Item1.V2))
                        {
                            temp.Add(result[j].Item1.V1);
                            temp.Add(result[j].Item1.V2);
                            var cutterTri = result[j].Item2;
                            var cuttedTri = tri;
                            Vector3 directed_seg = cutterTri.V2 - cutterTri.V1;
                            tri.ComputePlane();
                            Vector3 inside_dir = tri.plane.N.Cross(directed_seg);
                            List<int> inn = new List<int>();
                            if ((cuttedTri.V1 - cutterTri.V1).Dot(inside_dir) >= 0)
                                inn.Add(0);
                            if ((cuttedTri.V2 - cutterTri.V1).Dot(inside_dir) >= 0)
                                inn.Add(1);
                            if ((cuttedTri.V3 - cutterTri.V1).Dot(inside_dir) >= 0)
                                inn.Add(2);
                            insides.Add(inn);
                        }
                    }
                    if (temp.Count <= 2)
                    {
                        tris.Add(new Triangle(tri));
                        continue;
                    }
                    for (int k = 0; k < insides.Count; ++k)
                    {
                        Console.WriteLine(insides[k].Count);
                        var first = temp[2 * k];
                        var second = temp[2 * k + 1];
                        if (insides[k].Count == 1)
                        {
                            tris.Add(new Triangle(first, second, result[k].Item2[insides[k][0]]));
                        }
                        else
                        {
                            var third = result[k].Item2[insides[k][0]];
                            var fourt = result[k].Item2[insides[k][1]];
                            tris.Add(new Triangle(second, third, fourt));
                            tris.Add(new Triangle(first, second, fourt));
                        }
                    }
                    temp = temp.Distinct().ToList();
                    if (anyOutside)
                    {
                        NetTopologySuite.Geometries.LinearRing ring = new NetTopologySuite.Geometries.LinearRing(new Coordinate[] { new Coordinate(tri.V1.X, tri.V1.Y, tri.V1.Z), new Coordinate(tri.V2.X, tri.V2.Y, tri.V2.Z), new Coordinate(tri.V3.X, tri.V3.Y, tri.V3.Z), new Coordinate(tri.V1.X, tri.V1.Y, tri.V1.Z) });
                        List<Coordinate> holesCoords = new List<Coordinate>();
                        List<Vertex> vertices = new Vertex[] { new Vertex(tri.V1.X, tri.V1.Y, tri.V1.Z), new Vertex(tri.V2.X, tri.V2.Y, tri.V2.Z), new Vertex(tri.V3.X, tri.V3.Y, tri.V3.Z) }.ToList();
                        //foreach (var item in temp)
                        //{
                        //    holesCoords.Add(new Coordinate(item.X, item.Y, item.Z));
                        //}
                        List<NetTopologySuite.Triangulate.Segment> segments = new List<NetTopologySuite.Triangulate.Segment>();
                        List<Vertex> holesVertices = new List<Vertex>();
                        for (int v = 0; v < temp.Count - 1; v += 2)
                        {
                            holesVertices.Add(new Vertex(temp[v].X, temp[v].Y, temp[v].Z));
                            holesVertices.Add(new Vertex(temp[v + 1].X, temp[v + 1].Y, temp[v + 1].Z));
                            segments.Add(new NetTopologySuite.Triangulate.Segment(temp[v].X, temp[v].Y, temp[v].Z, temp[v + 1].X, temp[v + 1].Y, temp[v + 1].Z));
                        }
                        //holesCoords.Add(new Coordinate(temp[0].X, temp[0].Y, temp[0].Z));
                        //var holesRing = new NetTopologySuite.Geometries.LinearRing(holesCoords.ToArray());
                        NetTopologySuite.Triangulate.ConformingDelaunayTriangulator triss = new NetTopologySuite.Triangulate.ConformingDelaunayTriangulator(vertices, 0.0001);
                        //triss.SetConstraints(segments, holesVertices);
                        triss.SetConstraints(segments, holesVertices);
                        triss.FormInitialDelaunay();
                        triss.EnforceConstraints();
                        var ttt = triss.Subdivision.GetTriangles(NetTopologySuite.Geometries.Geometry.DefaultFactory);
                        foreach (var tttt in ttt)
                        {
                            tris.Add(new Triangle(tttt.Coordinates.Select(x => new Vector3(x.X, x.Y, x.Z)).ToList()));
                        }
                        //NetTopologySuite.Geometries.Polygon triPolygon = new NetTopologySuite.Geometries.Polygon(ring, new ILinearRing[] { holesRing});
                        //NetTopologySuite.Triangulate.DelaunayTriangulationBuilder bu = new NetTopologySuite.Triangulate.DelaunayTriangulationBuilder();
                        //bu.SetSites(triPolygon);
                        //var triis = bu.GetTriangles(NetTopologySuite.Geometries.Geometry.DefaultFactory);
                        //foreach (var item1 in triis)
                        //{
                        //    var pos = item1.Coordinates.Select(x => new Vector3(x.X, x.Y, x.Z)).ToList();
                        //    tris.Add(new Triangle(pos));
                        //}
                        //NetTopologySuite.Geometries.Polygon holePolygon = new NetTopologySuite.Geometries.Polygon(holesRing);
                        //var cut = triPolygon.Difference(holePolygon);
                        //foreach (var item in cut)
                        //{
                        //    NetTopologySuite.Triangulate.DelaunayTriangulationBuilder bu = new NetTopologySuite.Triangulate.DelaunayTriangulationBuilder();
                        //    bu.SetSites(item);
                        //    var triis = bu.GetTriangles(NetTopologySuite.Geometries.Geometry.DefaultFactory);
                        //    foreach (var item1 in triis)
                        //    {
                        //        var pos = item1.Coordinates.Select(x => new Vector3(x.X, x.Y, x.Z)).ToList();
                        //        tris.Add(new Triangle(pos));
                        //    }
                        //}
                        continue;
                    }
                    //if (allOutside)
                    //{
                    //    var zAxis = new Vector3(0, 0, 1);
                    //    var normal = tri.Normal();
                    //    var rotAxis = normal.Cross(zAxis).Normalized();
                    //    var angle = Math.Acos(zAxis.Dot(normal));
                    //    var rot = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3((float)rotAxis.X, (float)rotAxis.Y, (float)rotAxis.Z), (float)angle);
                    //    List<Tuple<int, OpenTK.Vector4>> modified = new List<Tuple<int, OpenTK.Vector4>>();
                    //    List<List<int>> indices = new List<List<int>>();
                    //    var xAxis = new Vector3(1, 0, 0);
                    //    var t1 = rot * new OpenTK.Vector4((float)tri.V1.X, (float)tri.V1.Y, (float)tri.V1.Z, 1);
                    //    var t2 = rot * new OpenTK.Vector4((float)tri.V1.X, (float)tri.V1.Y, (float)tri.V1.Z, 1);
                    //    var t3 = rot * new OpenTK.Vector4((float)tri.V1.X, (float)tri.V1.Y, (float)tri.V1.Z, 1);
                    //    //var v1 = new Vector3(t1.X, t1.Y, t1.Z);
                    //    //var v2 = new Vector3(t2.X, t2.Y, t2.Z);
                    //    //var v3 = new Vector3(t3.X, t3.Y, t3.Z);
                    //    int cnt = 1;
                    //    for (int v = 0; v < temp.Count; ++v)
                    //    {
                    //        var tt = rot * new OpenTK.Vector4((float)temp[v].X, (float)temp[v].Y, (float)temp[v].Z, 1);
                    //        if (new Vector3(tt.X, tt.Y, tt.Z).IsParallel(xAxis))
                    //        {
                    //            List<int> ints = new List<int>();
                    //            modified.Add(new Tuple<int, OpenTK.Vector4>(v, tt));
                    //            if(cnt % 2 == 1)
                    //            {
                    //                if (t1.Y < tt.Y)
                    //                {
                    //                    ints.Add(0);
                    //                }
                    //                if (t2.Y < tt.Y)
                    //                {
                    //                    ints.Add(1);
                    //                }
                    //                if (t3.Y < tt.Y)
                    //                {
                    //                    ints.Add(2);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (t1.Y > tt.Y)
                    //                {
                    //                    ints.Add(0);
                    //                }
                    //                if (t2.Y > tt.Y)
                    //                {
                    //                    ints.Add(1);
                    //                }
                    //                if (t3.Y > tt.Y)
                    //                {
                    //                    ints.Add(2);
                    //                }
                    //            }
                    //            indices.Add(ints);
                    //            cnt++;
                    //        }
                    //    }
                    //    modified.Sort((x, y) => x.Item2.Y > y.Item2.Y ? 1 : x.Item2.Y < y.Item2.Y ? -1 : 0);
                    //    List<Vector3> res = new List<Vector3>();
                    //    List<List<Vector3>> neighs = new List<List<Vector3>>();
                    //    cnt = 0;
                    //    foreach (var item in modified)
                    //    {
                    //        res.Add(temp[item.Item1]);
                    //        List<Vector3> neigh = new List<Vector3>();
                    //        foreach (var item1 in indices[cnt])
                    //        {
                    //            neigh.Add(tri[item1]);
                    //        }
                    //        neighs.Add(neigh);
                    //        ++cnt;
                    //    }
                    //    if(res.Count == 2)
                    //    {
                    //        for (int k = 0; k < res.Count; k++)
                    //        {

                    //        }
                    //    }
                    //    continue;
                    //}
                    List<Vector3> points = new Vector3[] { tri.V1, tri.V2, tri.V3 }.ToList();
                    //var orig1 = ((tri.V1 + tri.V2 / 2) + tri.V3) / 2;
                    //var orig2 = new Vector3(0, 0, 0);
                    //for (int k = 0; k < temp.Count - 1; k += 2)
                    //{
                    //    orig2 = (orig2 + (temp[k] + temp[k + 1]) / 2) / 2;
                    //}
                    //orig2 = temp.Aggregate((x, y) => x + y) / temp.Count;
                    points = SortVerticies(points);
                    points.Reverse();
                    //points.Sort(new CounterClockwiseComp(orig1));
                    //temp.Sort(new ClockwiseComp(orig2));
                    temp = SortVerticies(temp);
                    //Console.WriteLine("After:");
                    //foreach (var item in temp)
                    //{
                    //    Console.WriteLine(item.ToString());
                    //}
                    //Console.WriteLine(points[0].ToString());
                    //Console.WriteLine(points[1].ToString());
                    //Console.WriteLine(points[2].ToString());
                    //Console.WriteLine();
                    //Console.WriteLine();
                    holes.Add(temp);
                    EarClipping clipper = new EarClipping();
                    clipper.SetPoints(points, holes);
                    clipper.Triangulate();
                    var te = clipper.Result;
                    for (int j = 0; j < te.Count - 2; j += 3)
                    {
                        tris.Add(new Triangle(te[j], te[j + 1], te[j + 2]));
                    }
                }
                else
                {
                    //tris.Add(tri);
                    tris.Add(new Triangle(tri));
                }
            }
            this.Triangles = tris;
            Cutter.Reset();
            this.Reset();
        }

        private static g3.Vector3d _fromVector(Vector3 vec)
        {
            return new g3.Vector3d(vec.X, vec.Y, vec.Z);
        }

        public void Reset()
        {
            for (int i = 0; i < Triangles.Count; i++)
            {
                var tri = Triangles[i];
                tri.V1.DynamicProperties = new DynamicProperties();
                tri.V2.DynamicProperties = new DynamicProperties();
                tri.V3.DynamicProperties = new DynamicProperties();
            }
        }

        private void _swap(int i, int j, List<Vector3> temp)
        {
            var tt = temp[i];
            temp[i] = temp[j];
            temp[i] = tt;
        }

        public void Draw(Vector3 color)
        {
            for (int i = 0; i < Triangles.Count; ++i)
            {
                var tri = Triangles[i];
                GL.Color3(color.X, color.Y, color.Z);
                GL.Begin(BeginMode.Triangles);
                GL.Vertex3(tri.V1.Data());
                GL.Vertex3(tri.V2.Data());
                GL.Vertex3(tri.V3.Data());
                GL.End();
            }
        }

        public void Draw()
        {
            for (int i = 0; i < Triangles.Count; ++i)
            {
                var tri = Triangles[i];
                var normal = (tri.V1 - tri.V2).Cross(tri.V1 - tri.V3).Normalized();
                GL.BindTexture(TextureTarget.Texture2D, Texture);
                GL.Begin(BeginMode.Triangles);
                GL.Normal3(normal.Data());
                GL.TexCoord2(0, 0);
                GL.Vertex3(tri.V1.Data());
                GL.TexCoord2(1, 0);
                GL.Vertex3(tri.V2.Data());
                GL.TexCoord2(0.5, 0.5);
                GL.Vertex3(tri.V3.Data());
                GL.End();
            }
        }

        private class ClockwiseComp : IComparer<Vector3>
        {
            Vector3 Origin { get; set; }
            public ClockwiseComp(Vector3 ori)
            {
                Origin = ori;
            }
            public int Compare(Vector3 x, Vector3 y)
            {
                var angle1 = Math.Abs(Math.Acos((Origin.Dot(x)) / (Origin.Length() * x.Length())));
                var angle2 = Math.Abs(Math.Acos((Origin.Dot(y)) / (Origin.Length() * y.Length())));
                return angle1 > angle2 ? 1 : angle2 > angle1 ? -1 : 0;
            }
        }

        private class CounterClockwiseComp : IComparer<Vector3>
        {
            Vector3 Origin { get; set; }
            public CounterClockwiseComp(Vector3 ori)
            {
                Origin = ori;
            }
            public int Compare(Vector3 x, Vector3 y)
            {
                var angle1 = Math.Abs(Math.Acos((Origin.Dot(x)) / (Origin.Length() * x.Length())));
                var angle2 = Math.Abs(Math.Acos((Origin.Dot(y)) / (Origin.Length() * y.Length())));
                return angle1 > angle2 ? -1 : angle2 > angle1 ? 1 : 0;
            }
        }

        public static List<Vector3> SortVerticies(List<Vector3> nodes)
        {

            //Vector3 first = nodes[0];

            ////Sort by distance from random point to get 2 adjacent points.
            //List<Vector3> temp = nodes.OrderBy(n => (n - first).Length()).ToList();

            ////Create a vector from the 2 adjacent points,
            ////this will be used to sort all points, except the first, by the angle to this vector.
            ////Since the shape is convex, angle will not exceed 180 degrees, resulting in a proper sort.
            //Vector3 refrenceVec = (temp[1] - first);

            ////Sort by angle to reference, but we are still missing the first one.
            //List<Vector3> results = temp.Skip(1).OrderBy(n => refrenceVec.Angle(n - first)).ToList();

            ////insert the first one, at index 0.
            //results.Insert(0, nodes[0]);

            ////Now that it is sorted, we check if we got the direction right, if we didn't we reverse the list.
            ////We compare the given normal and the cross product of the first 3 point.
            ////If the magnitude of the sum of the normal and cross product is less than Sqrt(2) then then there is more than 90 between them.
            //if (((results[1] - results[0]).Cross(results[2]- results[0]).Normalized() + normal.Normalized()).Length() < 1.414f)
            //{
            //    results.Reverse();
            //}

            //return results;

            var vertices = nodes;
            var center = vertices.Aggregate((x, y) => x + y) / vertices.Count;
            var normal = (vertices[1] - vertices[0]).Cross(vertices[2] - vertices[0]);
            var before = new List<Vector3>();
            foreach (var item in vertices)
            {
                before.Add(item);
            }
            int size = vertices.Count;
            for (int i = 0; i < size; i++)
            {
                var tt = new List<Vector3>();
                vertices = new List<Vector3>();
                foreach (var item in before)
                {
                    vertices.Add(item);
                }
                tt.Add(vertices[i]);
                Vector3 current = vertices[i];
                vertices.RemoveAt(i);
                int cnt = vertices.Count;
                while (cnt > 0)
                {
                    bool done = false;
                    for (int j = 0; j < vertices.Count; j++)
                    {
                        var t = normal.Dot((current - center).Cross(vertices[j] - center));
                        if (t < 0)
                        {
                            tt.Add(vertices[j]);
                            current = vertices[j];
                            vertices.RemoveAt(j);
                            done = true;
                            break;
                        }
                    }
                    if (done)
                    {
                        cnt--;
                    }
                    else
                    {
                        break;
                    }
                }
                if (cnt == 0)
                    return tt;
                else
                {
                    continue;
                }
            }
            throw new Exception("Holy shit");
        }
    }
}
