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

namespace Vr_Gl
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Triangle> cutterTris = new Triangle[] { new Triangle(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0.5, 0.2, -0.5)) }.ToList();
            List<Triangle> cuttedTris = new Triangle[] { new Triangle(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0.5, 0.5, 0)) }.ToList();
            using (MainWindow window = new MainWindow(cutterTris, cuttedTris))
            {
                window.Run(30);
            }
        }
    }
}
