using Intersection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Tree;
using Vr_Gl.Model;
using Triangulation;

namespace Vr_Gl.Simulation
{
    public class Cutter
    {
        public AabbTree Tree { get; set; }

        public Cutter(string fileName)
        {
            Tree = new AabbTree(new Loader(1000).Load(fileName));
        }

        public Cutter(string fileName, Vector3m initialPos) : this(fileName)
        {
            this.Move(initialPos);
        }

        public List<List<Triangle>> Intersect(List<Triangle> tris)
        {
            return Tree.IntersectTriangles(tris);
        }

        public void Move(Vector3m trans)
        {
            Tree.Move(trans);
        }

        public void Draw(Vector3m color)
        {
            for(int i = 0; i < Tree.Triangles.Count; ++i)
            {
                var tri = Tree.Triangles[i];
                GL.Color3(color.X, color.Y, color.Z);
                GL.Begin(BeginMode.Triangles);
                GL.Vertex3(tri.V1.Data());
                GL.Vertex3(tri.V2.Data());
                GL.Vertex3(tri.V3.Data());
                GL.End();
            }
        }
    }
}
