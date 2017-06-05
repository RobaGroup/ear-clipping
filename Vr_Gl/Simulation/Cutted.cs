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
            this.Triangles = new Loader(300).Load(fileName);
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
                Console.WriteLine(i);
                var result = IntersectionDetector.Detect(tri, intersectedTris[i]);
                List<List<Vector3>> holes = new List<List<Vector3>>();
                List<Vector3> temp = new List<Vector3>();
                Console.WriteLine(tri.ToString());
                Console.WriteLine(intersectedTris[0][0].ToString());
                for (int j = 0; j < result.Count; j++)
                {
                     Console.WriteLine("In");
                     temp.Add(result[j].V1);
                     temp.Add(result[j].V2);
                }
                if (result.Count >= 2)
                {
                    holes.Add(temp);
                    List<Vector3> points = tri.Points();
                    EarClipping clipper = new EarClipping();
                    clipper.SetPoints(points, holes);
                    clipper.Triangulate();
                    var t = clipper.Result;
                    Console.WriteLine(t.Count);
                    for (int j = 0; j < t.Count - 2; j += 3)
                    {
                        tris.Add(new Triangle(t[j], t[j + 1], t[j + 2]));
                    }
                }
                else
                {
                    tris.Add(tri);
                }
            }
            this.Triangles = tris;
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
                GL.BindTexture(TextureTarget.Texture2D, Texture);
                GL.Begin(BeginMode.Triangles);
                GL.TexCoord2(0, 0);
                GL.Vertex3(tri.V1.Data());
                GL.TexCoord2(1, 0);
                GL.Vertex3(tri.V2.Data());
                GL.TexCoord2(0.5, 0.5);
                GL.Vertex3(tri.V3.Data());
                GL.End();
            }
        }
    }
}
