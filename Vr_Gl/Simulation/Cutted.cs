using Intersection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Triangulation;
using OpenTK.Graphics.OpenGL;
using Vr_Gl.Model;
using Vr_Gl.Graphics;
using Vr_Gl.Properties;
using GeoAPI.Geometries;
using NetTopologySuite.Triangulate.QuadEdge;
using NetTopologySuite.Geometries.Utilities;
using NetTopologySuite.Operation.Polygonize;
using ClipperLib;

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

//overload with imag 
    public Cutted(List<Triangle> tris, Cutter cutter, Vector3 initialPos, Bitmap bitmap ) : this(tris, cutter)
        {
            this.Move(initialPos);
            this.Texture = Vr_Gl.Graphics.AssetsLoader.LoadTexture(bitmap);
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
                        temp.Add(result[j].Item1.V1);
                        temp.Add(result[j].Item1.V2);
                        if (tri.Inside(result[j].Item1.V1) && tri.Inside(result[j].Item1.V2))
                        {
                            var cutterTri = result[j].Item2;
                            var cuttedTri = tri;
                            Vector3 directed_seg = result[j].Item1.V2 - result[j].Item1.V1;
                            Vector3 inside_dir = cutterTri.Normal().Normalized().Cross(directed_seg);
                            List<int> inn = new List<int>();
                            var f = result[j].Item1;
                            if ((f.V1 - cutterTri.V1).Dot(inside_dir) >= 0)
                                inn.Add(0);
                            if ((f.V1 - cutterTri.V2).Dot(inside_dir) >= 0)
                                inn.Add(1);
                            if ((f.V1 - cutterTri.V3).Dot(inside_dir) >= 0)
                                inn.Add(2);
                            insides.Add(inn);
                        }
                        else
                        {
                            anyOutside = true;
                        }
                    }
                    if (temp.Count <= 2)
                    {
                        tris.Add(new Triangle(tri));
                        continue;
                    }
                    if (!Vr_Gl.Config.OnlyHoles)
                    {
                        Console.WriteLine("Inside");
                        for (int k = 0; k < insides.Count; ++k)
                        {
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
                    }
                    temp = temp.Distinct().ToList();
                    if (anyOutside)
                    {
                        tris.Add(tri);
                        continue;
                        //int factor = 1000;
                        //var zAxis = new Vector3(0, 0, 1);
                        //var normal = tri.Normal();
                        //var rotAxis = normal.Cross(zAxis);
                        //bool ignore = rotAxis.IsMostlyZero();
                        //if (!ignore)
                        //    rotAxis = rotAxis.Normalized();
                        //var angle = Math.Acos(zAxis.Dot(normal));
                        //var rot = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3((float)rotAxis.X, (float)rotAxis.Y, (float)rotAxis.Z), (float)angle);
                        //var revRot = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3((float)rotAxis.X, (float)rotAxis.Y, (float)rotAxis.Z), ((float)-angle));
                        //Func<Vector3, IntPoint> to2D = (vec) =>
                        //{
                        //    Vector3 vecc = vec;
                        //    //var ress = rot * new OpenTK.Vector4((float)vecc.X, (float)vecc.Y, (float)vecc.Z, 1);
                        //    double x = vec.X * rot.M11 + vec.Y * rot.M12 + vec.Z * rot.M13, 
                        //           y = vec.X * rot.M21 + vec.Y * rot.M22 + vec.Z * rot.M23,
                        //           z = vec.X * rot.M31 + vec.Y * rot.M32 + vec.Z * rot.M33;
                        //    return !ignore ? new IntPoint((int)(x * factor), (int)(y * factor)) : new IntPoint((int)(vec.X * factor), (int)(vec.Y * factor));
                        //};

                        //Func<IntPoint, Vector3> to3D = (point) =>
                        //{
                        //    double x = point.X * rot.M11 + point.Y * rot.M12 ,
                        //           y = point.X * rot.M21 + point.Y * rot.M22,
                        //           z = point.X * rot.M31 + point.Y * rot.M32;
                        //    return (!ignore ? new Vector3(x / factor, y / factor, z) : new Vector3((float)point.X / factor, (float)point.Y / factor, 0.01));
                        //};
                        ////    List<Tuple<int, OpenTK.Vector4>> modified = new List<Tuple<int, OpenTK.Vector4>>();
                        ////    List<List<int>> indices = new List<List<int>>();
                        ////    var xAxis = new Vector3(1, 0, 0);
                        ////    var t1 = rot * new OpenTK.Vector4((float)tri.V1.X, (float)tri.V1.Y, (float)tri.V1.Z, 1);
                        ////    var t2 = rot * new OpenTK.Vector4((float)tri.V1.X, (float)tri.V1.Y, (float)tri.V1.Z, 1);
                        ////    var t3 = rot * new OpenTK.Vector4((float)tri.V1.X, (float)tri.V1.Y, (float)tri.V1.Z, 1);
                        //List<IntPoint> subj = new List<IntPoint>();
                        //var c1 = to2D(tri.V1);
                        //var c2 = to2D(tri.V2);
                        //var c3 = to2D(tri.V3);
                        //subj.Add(c1);
                        //subj.Add(c2);
                        //subj.Add(c3);
                        //List<IntPoint> clip = new List<IntPoint>();
                        //foreach (var item in temp)
                        //{
                        //    clip.Add(to2D(item));
                        //}
                        //List<List<IntPoint>> sol = new List<List<IntPoint>>();
                        //Clipper c = new Clipper();
                        //c.AddPolygon(subj, PolyType.ptSubject);
                        //c.AddPolygon(clip, PolyType.ptClip);
                        //c.Execute(ClipType.ctDifference, sol, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);
                        //Console.WriteLine(sol.Count);
                        //foreach (var item in sol)
                        //{
                        //    var ps = item.Select(to3D).ToList();
                        //    ps = SortVerticies(ps);
                        //    EarClipping cp = new EarClipping();
                        //    cp.SetPoints(ps);
                        //    cp.Triangulate();
                        //    var tes = cp.Result;
                        //    for (int j = 0; j < tes.Count - 2; j += 3)
                        //    {
                        //        tris.Add(new Triangle(tes[j], tes[j + 1], tes[j + 2]));
                        //    }
                        //}
                        //var ring = new NetTopologySuite.Geometries.LinearRing(new Coordinate[] { new Coordinate(tri.V1.X, tri.V1.Y, tri.V1.Z), new Coordinate(tri.V2.X, tri.V2.Y, tri.V2.Z), new Coordinate(tri.V3.X, tri.V3.Y, tri.V3.Z), new Coordinate(tri.V1.X, tri.V1.Y, tri.V1.Z) });
                        //List<Coordinate> holesCoords = new List<Coordinate>();
                        //List<Vertex> vertices = new Vertex[] { new Vertex(tri.V1.X, tri.V1.Y, tri.V1.Z), new Vertex(tri.V2.X, tri.V2.Y, tri.V2.Z), new Vertex(tri.V3.X, tri.V3.Y, tri.V3.Z) }.ToList();
                        //foreach (var item in temp)
                        //{
                        //    holesCoords.Add(new Coordinate(item.X, item.Y, item.Z));
                        //}
                        //holesCoords.Add(new Coordinate(temp[0].X, temp[0].Y, temp[0].Z));
                        //var holesRing = new NetTopologySuite.Geometries.LinearRing(holesCoords.ToArray());
                        //NetTopologySuite.Geometries.Polygon triPolygon = new NetTopologySuite.Geometries.Polygon(ring);
                        //NetTopologySuite.Geometries.Polygon holePolygon = new NetTopologySuite.Geometries.Polygon(holesRing);
                        //var cut = ClipPolygon(triPolygon, holePolygon);
                        //Console.WriteLine(cut.IsEmpty);
                        //var cuttt = cut as IGeometryCollection;
                        //foreach(var cutt in cuttt)
                        //{
                        //    NetTopologySuite.Triangulate.DelaunayTriangulationBuilder builder = new NetTopologySuite.Triangulate.DelaunayTriangulationBuilder();
                        //    builder.Tolerance = 0.0001;
                        //    builder.SetSites(cut);
                        //    var ress = builder.GetSubdivision().GetTriangles(NetTopologySuite.Geometries.Geometry.DefaultFactory);
                        //    foreach (var item in ress)
                        //    {
                        //        if (!item.IsEmpty)
                        //            tris.Add(new Triangle(ress.Coordinates.Select(x => new Vector3(x.X, x.Y, x.Z)).ToList()));
                        //    }
                        //}
                        //continue;
                    }
                    List<Vector3> points = new Vector3[] { tri.V1, tri.V2, tri.V3 }.ToList();
                    points = SortVertices(points);
                    points.Reverse();
                    temp = SortVertices(temp);
                    holes.Add(temp);
                    EarClipping clipper = new EarClipping();
                    foreach (var item in temp)
                    {
                        Console.WriteLine(item.ToString());
                    }
                    Console.WriteLine("Triangle:");
                    Console.WriteLine(tri.V1.ToString());
                    Console.WriteLine(tri.V2.ToString());
                    Console.WriteLine(tri.V3.ToString());
                    Console.WriteLine();
                    clipper.SetPoints(points, holes);
                    clipper.Triangulate();
                    var te = clipper.Result;
                    Console.WriteLine(te.Count);
                    for (int j = 0; j < te.Count - 2; j += 3)
                    {
                        tris.Add(new Triangle(te[j], te[j + 1], te[j + 2]));
                    }

                    //var ring = new NetTopologySuite.Geometries.LinearRing(new Coordinate[] { new Coordinate(tri.V1.X, tri.V1.Y, tri.V1.Z), new Coordinate(tri.V2.X, tri.V2.Y, tri.V2.Z), new Coordinate(tri.V3.X, tri.V3.Y, tri.V3.Z), new Coordinate(tri.V1.X, tri.V1.Y, tri.V1.Z) });
                    //List<Coordinate> holesCoords = new List<Coordinate>();
                    //List<Vertex> vertices = new Vertex[] { new Vertex(tri.V1.X, tri.V1.Y, tri.V1.Z), new Vertex(tri.V2.X, tri.V2.Y, tri.V2.Z), new Vertex(tri.V3.X, tri.V3.Y, tri.V3.Z) }.ToList();
                    //foreach (var item in temp)
                    //{
                    //    holesCoords.Add(new Coordinate(item.X, item.Y, item.Z));
                    //}
                    //holesCoords.Add(new Coordinate(temp[0].X, temp[0].Y, temp[0].Z));
                    //var holesRing = new NetTopologySuite.Geometries.LinearRing(holesCoords.ToArray());
                    //NetTopologySuite.Geometries.Polygon triPolygon = new NetTopologySuite.Geometries.Polygon(ring);
                    //NetTopologySuite.Geometries.Polygon holePolygon = new NetTopologySuite.Geometries.Polygon(holesRing);
                    //var cut = triPolygon.Difference(holePolygon);
                    //NetTopologySuite.Triangulate.DelaunayTriangulationBuilder builder = new NetTopologySuite.Triangulate.DelaunayTriangulationBuilder();
                    //builder.Tolerance = 0.0001;
                    //builder.SetSites(triPolygon);
                    //var ress = builder.GetTriangles(NetTopologySuite.Geometries.Geometry.DefaultFactory);
                    //foreach (var item in ress)
                    //{
                    //    if (!item.IsEmpty)
                    //        tris.Add(new Triangle(ress.Coordinates.Select(x => new Vector3(x.X, x.Y, x.Z)).ToList()));
                    //}
                }
                else
                {
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

        public static List<Vector3> SortVertices(List<Vector3> nodes)
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
        internal static IGeometry ClipPolygon(IGeometry polygon, IPolygonal clipPolygonal)
        {
            var clipPolygon = (IGeometry)clipPolygonal;
            var nodedLinework = polygon.Boundary.Union(clipPolygon.Boundary);
            var polygons = Polygonize(nodedLinework);

            /*
            // Build a prepared clipPolygon
            var prepClipPolygon = NetTopologySuite.Geometries.Prepared.PreparedGeometryFactory.Prepare(clipPolygon);
                */

            // only keep polygons which are inside the input
            var output = new List<IGeometry>();
            for (var i = 0; i < polygons.NumGeometries; i++)
            {
                var candpoly = (IPolygon)polygons.GetGeometryN(i);
                var interiorPoint = candpoly.InteriorPoint;
                if (polygon.Contains(interiorPoint) &&
                    /*prepClipPolygon.Contains(candpoly)*/
                    clipPolygon.Contains(interiorPoint))
                    output.Add(candpoly);
            }
            /*
            return polygon.Factory.CreateGeometryCollection(
                GeometryFactory.ToGeometryArray(output));
                */
            Console.WriteLine(output.Count);
            return polygon.Factory.BuildGeometry(output);
        }

        internal static IGeometry Polygonize(IGeometry geometry)
        {
            var lines = LineStringExtracter.GetLines(geometry);
            var polygonizer = new Polygonizer();
            polygonizer.Add(lines);
            var polys = polygonizer.GetPolygons();
            var polyArray = NetTopologySuite.Geometries.GeometryFactory.ToGeometryArray(polys);
            return geometry.Factory.CreateGeometryCollection(polyArray);
        }

        internal static IGeometry PolygonizeForClip(IGeometry geometry, GeoAPI.Geometries.Prepared.IPreparedGeometry clip)
        {
            var lines = LineStringExtracter.GetLines(geometry);
            var clippedLines = new List<IGeometry>();
            foreach (ILineString line in lines)
            {
                if (clip.Contains(line))
                    clippedLines.Add(line);
            }
            var polygonizer = new Polygonizer();
            polygonizer.Add(clippedLines);
            var polys = polygonizer.GetPolygons();
            var polyArray = NetTopologySuite.Geometries.GeometryFactory.ToGeometryArray(polys);
            return geometry.Factory.CreateGeometryCollection(polyArray);
        }

        internal static IGeometry SplitPolygon(IGeometry polygon, IGeometry line)
        {
            var nodedLinework = polygon.Boundary.Union(line);
            var polygons = Polygonize(nodedLinework);

            // only keep polygons which are inside the input
            var output = new List<IGeometry>();
            for (var i = 0; i < polygons.NumGeometries; i++)
            {
                var candpoly = (IPolygon)polygons.GetGeometryN(i);
                if (polygon.Contains(candpoly.InteriorPoint))
                    output.Add(candpoly);
            }
            /*
            return polygon.Factory.CreateGeometryCollection(
                GeometryFactory.ToGeometryArray(output));
             */
            return polygon.Factory.BuildGeometry(output);
        }
    }
}
