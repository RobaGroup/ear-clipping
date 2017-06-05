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
using Vr_Gl.Graphics;

namespace Vr_Gl.Simulation
{
    public class Cutter
    {
        public AabbTree Tree { get; set; }
        public int Texture { get; set; }

        public Cutter(string fileName)
        {
            Tree = new AabbTree(new Loader(1000).Load(fileName));
        }

        public Cutter(List<Triangle> tris, string texturePath = "D:/cutter.jpg")
        {
            Tree = new AabbTree(tris);
            this.Texture = AssetsLoader.LoadTexture(texturePath);
        }

        public Cutter(string fileName, Vector3 initialPos, string texturePath = "D:/cutter.jpg") : this(fileName)
        {
            this.Move(initialPos);
            this.Texture = AssetsLoader.LoadTexture(texturePath);
        }

        public List<List<Triangle>> Intersect(List<Triangle> tris)
        {
            return Tree.IntersectTriangles(tris);
        }

        public void Move(Vector3 trans)
        {
            Tree.Move(trans);
        }

        public void Draw()
        {
            for (int i = 0; i < Tree.Triangles.Count; ++i)
            {
                var tri = Tree.Triangles[i];
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

        public void Draw(Vector3 color)
        {
            for (int i = 0; i < Tree.Triangles.Count; ++i)
            {
                var tri = Tree.Triangles[i];
                GL.Color3(color.X, color.Y, color.Z);
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
