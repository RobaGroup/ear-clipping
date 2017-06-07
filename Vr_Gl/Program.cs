using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Assimp.Configs;
using Intersection;
using Triangulation;
using Vr_Gl.Model;
using Tree;
using Vr_Gl.Graphics;
using Vr_Gl.Simulation;
//using g3;

namespace Vr_Gl
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<Triangle> cutterTris = new Loader(10).Load(@"D:/pyramid.3ds");
            List<Triangle> cutterTris = new Triangle[]
            {
                new Triangle(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0.3, 0.3, -0.5)),
                new Triangle(new Vector3(0, 0, 0), new Vector3(0, 0.6, 0), new Vector3(0.3, 0.3, -0.5)),
                new Triangle(new Vector3(1, 0, 0), new Vector3(1, 0.6, 0), new Vector3(0.3, 0.3, -0.5)),
                new Triangle(new Vector3(0, 0.6, 0), new Vector3(1, 0.6, 0), new Vector3(0.3, 0.3, -0.5))
            }.ToList();
            List<Triangle> cuttedTris = new Triangle[]
            {
                new Triangle(new Vector3(0, 0, 0), new Vector3(2, 0, 0), new Vector3(1, 3, 0)),
              new Triangle(new Vector3(0, 0, 0), new Vector3(0, 0, -2), new Vector3(1, 3, -1)),
                new Triangle(new Vector3(2, 0, 0), new Vector3(2, 0, -2), new Vector3(1, 3, -1)),
                new Triangle(new Vector3(0, 0, -2), new Vector3(2, 0, -2), new Vector3(1, 3, -1)),
                new Triangle(new Vector3(0, 0, 0), new Vector3(2, 0, 0), new Vector3(0, 0, -2)),
                new Triangle(new Vector3(2, 0, 0), new Vector3(2, 0, -2), new Vector3(0, 0, -2))
            }.ToList();
            //List<Triangle> cuttedTris = new Loader(100).Load(@"D:/zune.obj");
            Config.OnlyHoles = true;
            using (MainWindow window = new MainWindow(cutterTris, cuttedTris))
            {
                window.Run(30);
            }

        }
    }
}
