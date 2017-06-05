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
                if(intersectedTris[i].Count == 0)
                {
                    tris.Add(tri);
                    continue;
                }
                //var result = IntersectionDetector.Detect(tri, intersectedTris[i]);
                var result = IntersectionDetector.Intersect(tri, intersectedTris[i]);
                if (result.Count >= 2)
                {
                    List<List<Vector3>> holes = new List<List<Vector3>>();
                    ISet<Vector3> t = new HashSet<Vector3>();
                    //List<Vector3> t = new List<Vector3>();
                    for (int j = 0; j < result.Count; j++)
                    {
                        t.Add(result[j].V1);
                        t.Add(result[j].V2);
                    }
                    var temp = t.ToList();
                    List<Vector3> points = new Vector3[] { tri.V1, tri.V2, tri.V3}.ToList();
                    var orig1 = (tri.V1 + tri.V2 + tri.V3) / 3;
                    var orig2 = new Vector3(0, 0, 0);
                    for (int k = 0; k < temp.Count - 1; k += 2)
                    {
                        orig2 = (orig2 + (temp[k] + temp[k + 1]) / 2) / 2;
                    }
                    EarClipping clipper = new EarClipping();
                    points.Sort(new CounterClockwiseComp(orig1));
                    temp.Sort(new ClockwiseComp(orig2));
                    foreach (var item in temp)
                    {
                        Console.WriteLine(item.ToString());
                    }
                    holes.Add(temp);
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
                    tris.Add(tri);
                }
            }
            this.Triangles = tris;
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
    }
}
