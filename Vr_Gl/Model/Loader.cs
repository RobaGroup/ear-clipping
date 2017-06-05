using Assimp;
using Assimp.Configs;
using Intersection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulation;

namespace Vr_Gl.Model
{
    public class Loader
    {
        private double factor;

        public Loader(double factor = 100)
        {
            this.factor = factor;
        }
        public List<Triangle> Load(string fileName)
        {
            AssimpContext importer = new AssimpContext();
            //importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
            List<Triangle> tris = new List<Triangle>();
            Scene scene = importer.ImportFile(fileName, PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs | PostProcessSteps.JoinIdenticalVertices | PostProcessSteps.OptimizeGraph | PostProcessSteps.SplitLargeMeshes | PostProcessSteps.GenerateNormals);
            foreach (var mesh in scene.Meshes)
            {
                foreach (var face in mesh.Faces)
                {
                    Vector3D p1 = mesh.Vertices[face.Indices[0]],
                             p2 = mesh.Vertices[face.Indices[1]],
                             p3 = mesh.Vertices[face.Indices[2]];
                    tris.Add(new Triangle(new Vector3(p1.X, p1.Y, p1.Z) / factor, new Vector3(p2.X, p2.Y, p2.Z) / factor, new Vector3(p3.X, p3.Y, p3.Z) / factor));
                }
            }
            return tris;
        }

        
    }
}
