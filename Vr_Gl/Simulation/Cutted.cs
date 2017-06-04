using Intersection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulation;
using OpenTK.Graphics.OpenGL;
using Vr_Gl.Model;

namespace Vr_Gl.Simulation
{
    public class Cutted
    {
        List<Triangle> Triangles { get; set; }
        public Cutter Cutter { get; set; }

        public Cutted(string fileName, Cutter cutter)
        {
            this.Triangles = new Loader(300).Load(fileName);
            this.Cutter = cutter;
        }

        public Cutted(string fileName, Cutter cutter, Vector3m initialPos) : this(fileName, cutter)
        {
            this.Move(initialPos);
        }

        public void Move(Vector3m trans)
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
                EarClipping clipper = new EarClipping();
                if(intersectedTris[i].Count == 0)
                {
                    tris.Add(tri);
                    continue;
                }
                var result = IntersectionDetector.Detect(tri, intersectedTris[i]);
                List<List<Vector3m>> holes = new List<List<Vector3m>>();
                List<Vector3m> temp = new List<Vector3m>();
                for (int j = 0; j < result.Count; j++)
                {
                    if (result[j].Item1)
                    {
                        temp.Add(result[j].Item2.V1);
                        temp.Add(result[j].Item2.V2);
                    }
                }
                if (temp.Count <= 1)
                {
                    holes.Add(temp);
                    List<Vector3m> points = tri.Points();
                    clipper.SetPoints(points, holes);
                    clipper.Triangulate();
                    var t = clipper.Result;
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

        public void Draw()
        {
            for(int i = 0; i < Triangles.Count; ++i)
            {
                var tri = Triangles[i];
                GL.Begin(BeginMode.Triangles);
                GL.Vertex3(tri.V1.Data());
                GL.Vertex3(tri.V2.Data());
                GL.Vertex3(tri.V3.Data());
                GL.End();
            }
        }
    }
}
