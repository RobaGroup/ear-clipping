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
        private List<Vector3> _normals;

        public Cutter(string fileName)
        {
            _normals = new List<Vector3>();
            var tris = new Loader(40).Load(fileName);
            foreach (var tri in tris)
            {
                var normal = (tri.V1 - tri.V2).Cross(tri.V1 - tri.V3).Normalized();
                _normals.Add(normal);
            }
            Tree = new AabbTree(tris);
        }

        public Cutter(List<Triangle> tris, string texturePath = "D:/cutter.jpg")
        {
            _normals = new List<Vector3>();
            Tree = new AabbTree(tris);
            foreach (var tri in tris)
            {
                var normal = (tri.V1 - tri.V2).Cross(tri.V1 - tri.V3).Normalized();
                _normals.Add(normal);
            }
            this.Texture = AssetsLoader.LoadTexture(texturePath);
        }


        public void Reset()
        {
            for (int i = 0; i < Tree.Triangles.Count; i++)
            {
                var tri = Tree.Triangles[i];
                tri.V1.DynamicProperties = new DynamicProperties();
                tri.V2.DynamicProperties = new DynamicProperties();
                tri.V3.DynamicProperties = new DynamicProperties();
            }
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
                GL.Normal3(_normals[i].Data());
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
